using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehavior : MonoBehaviour
{
    [HideInInspector]
    public NavAgent navAgent;
    // This state will be activate on start
    public AiState defaultState;
    // Start is called before the first frame update
    // List with all states for this AI
    private List<AiState> aiStates = new List<AiState>();
    // The state that was before
    private AiState previousState;
    // Active state
    private AiState currentState;

    void Awake()
    {
        if (navAgent == null)
        {
            navAgent = GetComponentInChildren<NavAgent>();
        }
    }

    void OnEnable()
    {
        // Enable AI on AiBehavior enabling
        if (currentState != null && currentState.enabled == false)
        {
            EnableNewState();
        }
    }
    void OnDisable()
    {
        // Disable AI on AiBehavior disabling
        DisableAllStates();
    }
    void Start()
    {
        // Get all AI states from this gameobject
        AiState[] states = GetComponents<AiState>();
        if (states.Length > 0)
        {
            foreach (AiState state in states)
            {
                // Add state to list
                aiStates.Add(state);
            }
            if (defaultState != null)
            {
                // Set active and previous states as default state
                previousState = currentState = defaultState;
                if (currentState != null)
                {
                    // Go to active state
                    ChangeState(currentState);
                }
                else
                {
                    Debug.LogError("Incorrect default AI state " + defaultState);
                }
            }
            else
            {
                Debug.LogError("AI have no default state");
            }
        }
        else
        {
            Debug.LogError("No AI states found");
        }
    }
    public void ChangeState(AiState state)
    {
        if (state != null)
        {
            // Try to find such state in list
            foreach (AiState aiState in aiStates)
            {
                if (state == aiState)
                {
                    previousState = currentState;
                    currentState = aiState;
                    NotifyOnStateExit();
                    DisableAllStates();
                    EnableNewState();
                    NotifyOnStateEnter();
                    return;
                }
            }
            Debug.Log("No such state " + state);
            // If have no such state - go to default state
            GoToDefaultState();
            Debug.Log("Go to default state " + aiStates[0]);
        }
    }
    public void GoToDefaultState()
    {
        previousState = currentState;
        currentState = defaultState;
        NotifyOnStateExit();
        DisableAllStates();
        EnableNewState();
        NotifyOnStateEnter();
    }
    private void EnableNewState()
    {
        currentState.enabled = true;
    }
    private void DisableAllStates()
    {
        foreach (AiState aiState in aiStates)
        {
            aiState.enabled = false;
        }
    }
    

    private void NotifyOnStateExit()
    {
        previousState.OnStateExit(previousState, currentState);
    }
    private void NotifyOnStateEnter()
    {
        currentState.OnStateEnter(previousState, currentState);
    }
    public void OnTrigger(AiState.Trigger trigger, Collider2D my, Collider2D other)
    {
        if (currentState == null)
        {
            Debug.Log("Current state is null");
        }
        currentState.OnTrigger(trigger, my, other);
    }
}
