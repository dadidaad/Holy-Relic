using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SracBuff : AiFeature
{
    // Amount of healed hp
    public float speedAmount = 0.2f;
    // Delay between healing
    public float cooldown = 1f;
    // Visual effect for healing
    public GameObject healVisualPrefab;
    // Duration for heal visual effect
    public float healVisualDuration = 1f;
    //Creeb Object
    public GameObject buffScrac;
    // Allowed objects tags for collision detection

    public List<string> tags = new List<string>();
    


    // Counter for cooldown
    private float cooldownCounter;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        cooldownCounter = cooldown;
    }

    /// <summary>
    /// Fixeds the update.
    /// </summary>
    void FixedUpdate()
    {
        if (cooldownCounter < cooldown)
        {
            cooldownCounter += Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Determines whether this instance is tag allowed the specified tag.
    /// </summary>
    /// <returns><c>true</c> if this instance is tag allowed the specified tag; otherwise, <c>false</c>.</returns>
    /// <param name="tag">Tag.</param>
    private bool IsTagAllowed(string tag)
    {
        bool res = false;
        if (tags.Count > 0)
        {
            foreach (string str in tags)
            {
                if (str == tag)
                {
                    res = true;
                    break;
                }
            }
        }
        else
        {
            res = true;
        }
        return res;
    }

    /// <summary>
    /// Heal specified target if cooldown expired.
    /// </summary>
    /// <param name="target">Target.</param>
    private void BoostSpeedOrHP(DamageTaker targetDameTaken, NavAgent targetSpeed)
    {
        DamageTaker OBuffDameTaken = buffScrac.GetComponent<DamageTaker>();
        NavAgent OBuffSpeed = buffScrac.GetComponent<NavAgent>();
        //print("Before creeb speed:" + OBuffSpeed.speed);
        //print("creeb Before blood: " + OBuffDameTaken.currentHitpoints);

        // If cooldown expired
        if (cooldownCounter >= cooldown)
        {
            //Buff HP < target HP
            if(OBuffDameTaken.currentHitpoints < targetDameTaken.currentHitpoints)
            {
                cooldownCounter = 0f;
                targetDameTaken.TakeDamage(-targetDameTaken.currentHitpoints / 2);
                OBuffDameTaken.TakeDamage(targetDameTaken.currentHitpoints / 2);
            }
            else
            {
                cooldownCounter = 0f;
                targetSpeed.speed += OBuffSpeed.speed / 2;
                OBuffSpeed.speed -= OBuffSpeed.speed / 2 + 0.025f;
                
            }
            if (healVisualPrefab != null)
            {
                // Create visual healing effect on target
                GameObject effect = Instantiate(healVisualPrefab, targetDameTaken.transform);
                // And destroy it after specified timeout
                Destroy(effect, healVisualDuration);
              
            }
            //print("Target After speed: " + targetSpeed.speed);
            //print("Target After blood: " + targetDameTaken.currentHitpoints);
            //print("After  speed:" + OBuffSpeed.speed);
            //print("creeb Before blood: " + OBuffDameTaken.currentHitpoints);
        }
    }

    /// <summary>
    /// Raises the trigger stay2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            DamageTaker targetDameTaken = other.gameObject.GetComponent<DamageTaker>();
            NavAgent targetSpeed = other.gameObject.GetComponent<NavAgent>();
            // If it has Damege Taker component
            GameObject target = other.gameObject;
           
            if (targetDameTaken != null && targetSpeed != null && targetSpeed.name != "Etteolf"&& target.name != name)
            {
                // If target injured
                
                //print("Target name: " + targetSpeed.name);
                //print("Target Before speed: " + targetSpeed.speed);
                //print("Target before blood: " + targetDameTaken.currentHitpoints);
                BoostSpeedOrHP(targetDameTaken, targetSpeed);
            }
        }
    }
}
