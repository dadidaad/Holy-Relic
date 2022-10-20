using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiState : MonoBehaviour
{
	// Allowed triiger types for AI state transactions
	public enum Trigger
	{
		TriggerEnter,   // On collider enter
		TriggerStay,    // On collider stay
		TriggerExit,    // On collider exit
		Damage,         // On damage taken
	}

	[Serializable]
	// Allows to specify AI state change on any trigger
	public class AiTransaction
	{
		public Trigger trigger;
		public AiState newState;
	}

	public AiTransaction[] specificTransactions;
	protected AiBehavior aiBehavior;

	public virtual void Awake()
	{
		aiBehavior = GetComponent<AiBehavior>();
		Debug.Assert(aiBehavior, "Setting Fail");
	}
	public virtual void OnStateEnter(AiState previousState, AiState newState)
	{

	}

	public virtual void OnStateExit(AiState previousState, AiState newState)
	{

	}

	public virtual bool OnTrigger(Trigger trigger, Collider2D my, Collider2D other)
	{
		bool res = false;
		// Check if this AI state has specific transactions for this trigger
		foreach (AiTransaction transaction in specificTransactions)
		{
			if (trigger == transaction.trigger)
			{
				aiBehavior.ChangeState(transaction.newState);
				res = true;
				break;
			}
		}
		return res;
	}
}
