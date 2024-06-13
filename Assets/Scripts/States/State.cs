using System;
using System.Collections.Generic;
using System.Linq;

public abstract class State
{
  List<(State state, Func<bool> predicate, Action<State> action)> transitionFroms = new();
  public virtual void Enter() { }
  public virtual void Update() { }
  public virtual void Exit() { }

  public State AddTransitionFrom(State state, Func<bool> predicate)
  {
    transitionFroms.Add((state, predicate, null));
    return this;
  }

  public State AddTransitionFrom(State state, Func<bool> predicate, Action<State> action)
  {
    transitionFroms.Add((state, predicate, action));
    return this;
  }

  public State AddTransitionFrom(State state, Action<State> action)
  {
    transitionFroms.Add((state, () => true, action));
    return this;
  }
  public bool CanEnter(State currentState)
  {
    if (currentState == null) return true;
    var (state, predicate, action) = transitionFroms.Find((v) => v.state == currentState && v.predicate());
    if (state != null)
    {
      action?.Invoke(this);
      return true;
    }
    return false;
  }
}