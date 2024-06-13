using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

  List<State> states = new();
  State currentState;

  public StateMachine()
  {

  }

  public StateMachine AddState(State state)
  {
    states.Add(state);
    if (currentState == null)
    {
      ChangeState(state);
    }
    return this;
  }
  public void AddStates(params State[] states)
  {
    foreach (var state in states)
    {
      AddState(state);
    }
  }

  public void Update()
  {
    foreach (var state in states)
    {
      if (state == currentState) continue;
      if (state.CanEnter(currentState))
      {
        ChangeState(state);
        break;
      }
    }
    currentState.Update();
  }
  /// Forces state transition, ignoring shouldEnter or allowed transitions list
  public void ChangeState(State newState)
  {
    Debug.Log("Changing state from " + currentState + " to " + newState);
    currentState?.Exit();
    currentState = newState;
    currentState?.Enter();
  }



}
