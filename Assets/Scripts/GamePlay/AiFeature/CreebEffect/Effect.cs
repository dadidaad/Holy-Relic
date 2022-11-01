using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private class EffectSpecific
    {
        public string name; // Name of effect
        public GameObject fx; // Visual effect
        public Dictionary<float, float> effects = new Dictionary<float, float>(); // List with effect durations: 1d - effect modifier, 2st - expired time, 
    }
    // Start is called before the first frame update
    private enum EffectTrigger
    {
        Added,
        NewModifier,
        ModifierExpired,
        Removed
    }   

    private List<EffectSpecific> effects = new List<EffectSpecific>();

	public void AddEffect(string effectName, float modifier, float duration, GameObject fxPrefab)
	{
		// Get effect handler from effect name
		MethodInfo methodInfo = GetType().GetMethod(effectName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (methodInfo != null)
		{
			EffectSpecific hit = null;
			// Serch if same effect already active
			foreach (EffectSpecific sem in effects)
			{
				if (effectName == sem.name)
				{
					hit = sem;
					break;
				}
			}

			if (hit == null) // Have no such active effect
			{
				EffectSpecific newSem = new EffectSpecific();
				newSem.name = effectName;
				// Create effect
				newSem.effects.Add(modifier, Time.time + duration);
				// Add it to list
				effects.Add(newSem);
				// Play visual effect
				if (fxPrefab != null)
				{
					newSem.fx = Instantiate(fxPrefab, transform);
				}
				// Effect added
				methodInfo.Invoke(this, new object[] { EffectTrigger.Added, modifier });
			}
		}
		else
		{
			Debug.LogError("Unknown effect - " + effectName);
		}
	}
	public void AddConstantEffect(string effectName, float modifier, GameObject fxPrefab)
	{
		AddEffect(effectName, modifier, float.MaxValue, fxPrefab);
	}

	public bool RemoveConstantEffect(string effectName, float modifier)
	{
		bool res = false;
		foreach (EffectSpecific desc in effects)
		{
			if (desc.name == effectName)
			{
				List<float> expiredTimes = new List<float>(desc.effects.Keys);
				for (int i = 0; i < expiredTimes.Count; ++i)
				{
					if (expiredTimes[i] == modifier && desc.effects[expiredTimes[i]] == float.MaxValue)
					{
						res = true;
						desc.effects[expiredTimes[i]] = Time.time;
						break;
					}
				}
			}
		}
		return res;
	}

	void FixedUpdate()
	{
		float currentTime = Time.time;
		// List for empty effects
		List<EffectSpecific> emptyEffects = new List<EffectSpecific>();
		// Look at current active effects
		foreach (EffectSpecific desc in effects)
		{
			// List for expired effects duration
			List<float> expiredList = new List<float>();
			foreach (KeyValuePair<float, float> effectData in desc.effects)
			{
				// Effect duration expired (if duration == MaxValue - this is constant effect)
				if (effectData.Value != float.MaxValue && currentTime > effectData.Value)
				{
					// Add to expired list
					expiredList.Add(effectData.Key);
					// Call effect modifier expired handler
					MethodInfo methodInfo = GetType().GetMethod(desc.name, BindingFlags.Instance | BindingFlags.NonPublic);
					methodInfo.Invoke(this, new object[] { EffectTrigger.ModifierExpired, effectData.Key });
				}
			}
			// Remove expired effects from list
			foreach (float expired in expiredList)
			{
				desc.effects.Remove(expired);
			}
			// If effect has no more duration
			if (desc.effects.Count <= 0)
			{
				// Call effect removed handler
				MethodInfo methodInfo = GetType().GetMethod(desc.name, BindingFlags.Instance | BindingFlags.NonPublic);
				methodInfo.Invoke(this, new object[] { EffectTrigger.Removed, 0f });
				// Add it to empty effects list
				emptyEffects.Add(desc);
			}
		}
		// Remove empty effects from list
		foreach (EffectSpecific emptyEffect in emptyEffects)
		{
			if (emptyEffect.fx != null)
			{
				// Remove visual effect on effect end
				Destroy(emptyEffect.fx);
			}
			effects.Remove(emptyEffect);
		}
	}

	public bool HasActiveEffect(string effectName)
	{
		bool res = false;
		foreach (EffectSpecific effect in effects)
		{
			if (effect.name == effectName)
			{
				res = true;
				break;
			}
		}
		return res;
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
	}

	/// <summary>
	/// Stun effect handler (bool).
	/// </summary>
	/// <param name="trigger">Trigger.</param>
	/// <param name="modifier">Modifier.</param>
	private void Stun(EffectTrigger trigger, float modifier)
	{
		AiBehavior aiBehavior = GetComponent<AiBehavior>();
		NavAgent navAgent = GetComponent<NavAgent>();
		switch (trigger)
		{
			case EffectTrigger.Added:
				aiBehavior.enabled = false;
				navAgent.enabled = false;
				break;
			case EffectTrigger.Removed:
				aiBehavior.enabled = true;
				navAgent.enabled = true;
				break;
		}
	}

	/// <summary>
	/// Speed effect handler (float).
	/// </summary>
	/// <param name="trigger">Trigger.</param>
	/// <param name="modifier">Modifier (can not be 0f).</param>
	private void Speed(EffectTrigger trigger, float modifier)
	{
		NavAgent navAgent = GetComponent<NavAgent>();
		switch (trigger)
		{
			case EffectTrigger.Added:
			case EffectTrigger.NewModifier:
				navAgent.speed *= 1 + modifier;
				break;
			case EffectTrigger.ModifierExpired:
				navAgent.speed /= 1 + modifier;
				break;
		}
	}
    /// <summary>
    /// Speed effect handler (float).
    /// </summary>
    /// <param name="trigger">Trigger.</param>
    /// <param name="modifier">Modifier (can not be 0f).</param>
    private void Turn(EffectTrigger trigger, float modifier)
    {
        NavAgent navAgent = GetComponent<NavAgent>();
        switch (trigger)
        {
            case EffectTrigger.Added:
            case EffectTrigger.NewModifier:
                navAgent.speed *= 1 + modifier;
                break;
            case EffectTrigger.ModifierExpired:
                navAgent.speed /= 1 + modifier;
                break;
        }
    }
    /// <summary>
    /// Speed effect handler (float).
    /// </summary>
    /// <param name="trigger">Trigger.</param>
    /// <param name="modifier">Modifier (can not be 0f).</param>
    private void Attenuate(EffectTrigger trigger, float modifier)
    {
        DamageTaker damageTaker = GetComponent<DamageTaker>();
        switch (trigger)
        {
            case EffectTrigger.Added:
            case EffectTrigger.NewModifier:
				//print("Before:"+ damageTaker.currentHitpoints);
                damageTaker.currentHitpoints -= (int)(damageTaker.currentHitpoints * modifier);
                //print("After:"+ damageTaker.currentHitpoints);
                break;
        }
    }
}
