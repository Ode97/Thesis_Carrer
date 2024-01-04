using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool FSMCondition();

public delegate void FSMAction();

public class FSMTransition
{
    public FSMCondition condition;
    public List<FSMAction> actions = new List<FSMAction>();

    public FSMTransition(FSMCondition condition, FSMAction[] actions = null)
    {
        this.condition = condition;
        if (actions != null) this.actions.AddRange(actions);
    }

    public void Execute()
    {
        foreach (FSMAction action in actions) action();
    }
}

public class FSMState
{
    public List<FSMAction> enterActions = new List<FSMAction>();
    public List<FSMAction> stayActions = new List<FSMAction>();
    public List<FSMAction> exitActions = new List<FSMAction>();
    public string name;

    private Dictionary<FSMTransition, FSMState> links;

    public FSMState(string name)
    {
        links = new Dictionary<FSMTransition, FSMState>();
        this.name = name;
    }

    public void AddTransition(FSMTransition transition, FSMState target)
    {
        links[transition] = target;
    }

    public FSMTransition CheckTransitions()
    {
        foreach (FSMTransition transition in links.Keys)
        {
            
            if (transition.condition()) return transition;
        }
        return null;
    }

    public FSMState NextState(FSMTransition transition)
    {
        return links[transition];
    }

    public void Enter()
    {
        foreach (FSMAction action in enterActions) action();
    }
    public void Stay()
    {
        foreach (FSMAction action in stayActions) action();
    }
    public void Exit()
    {
        foreach (FSMAction action in exitActions) action();
    }
}


public class FSM
{
    public FSMState currentState;
    public bool running = false;

    public FSM(FSMState state)
    {
        currentState = state;
    }

    public void StartFSM()
    {
        running = true;
        currentState.Enter();
    }

    public void UpdateFSM()
    {
        if (running)
        {
            
            FSMTransition transition = currentState.CheckTransitions();
            
            if (transition != null)
            {
                
                currentState.Exit();
                transition.Execute();
                
                currentState = currentState.NextState(transition);
                
                currentState.Enter();
                
            }
            else
            {
                currentState.Stay();
            }
            //Debug.Log(currentState.name);
        }
    }
}