using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




public class CardSuperStateMachine : MonoBehaviour
{
    protected float timeEnteredState;
    public class State
    {
        public Action DoSuperUpdate = DoNothing;
        public Action EnterState = DoNothing;
        public Action ExitState = DoNothing;
        public Enum currentState;

    }

    public State state = new State();

    [HideInInspector]

    public Enum currentState
    {
        get
        {
            return state.currentState;
        }
        set
        {
            if (state.currentState == value)
                return;
            ChangingState();
            state.currentState = value;
            ConfigureCurrentState();
        }

    }

    [HideInInspector]
    public Enum lastState;
    
    void ChangingState()
    {
        lastState = state.currentState;
        if (state.ExitState != null)
        {
            state.ExitState();
        }

        timeEnteredState = Time.time;
    }

    void ConfigureCurrentState()
    {
        
        //Now we need to configure all of the methods
        state.DoSuperUpdate = ConfigureDelegate<Action>("SuperUpdate", DoNothing);
        state.EnterState = ConfigureDelegate<Action>("EnterState", DoNothing);
        state.ExitState = ConfigureDelegate<Action>("ExitState", DoNothing);

        if (state.EnterState != null)
        {
            state.EnterState();
        }
    }

    Dictionary<Enum, Dictionary<string, Delegate>> _cache = new Dictionary<Enum, Dictionary<string, Delegate>>();

    T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
    {

        Dictionary<string, Delegate> lookup;
        if (!_cache.TryGetValue(state.currentState, out lookup))
        {
            _cache[state.currentState] = lookup = new Dictionary<string, Delegate>();
        }
        Delegate returnValue;
        if (!lookup.TryGetValue(methodRoot, out returnValue))
        {
            var mtd = GetType().GetMethod(state.currentState.ToString() + "_" + methodRoot, System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);

            if (mtd != null)
            {
                returnValue = Delegate.CreateDelegate(typeof(T), this, mtd);
            }
            else
            {
                returnValue = Default as Delegate;
            }
            lookup[methodRoot] = returnValue;
        }
        return returnValue as T;
    }

    void FixedUpdate()
    {
        EarlyGlobalSuperUpdate();

        state.DoSuperUpdate();

        LateGlobalSuperUpdate();
    }

    protected void DelegateMethod(string method, object[] parameters)
    {
        var mtd = GetType().GetMethod(state.currentState.ToString() + "_" + method, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        if (mtd == null)
        {
            mtd = GetType().GetMethod("Global" + "_" + method, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        mtd.Invoke(this, parameters);
    }

    protected virtual void EarlyGlobalSuperUpdate() { }

    protected virtual void LateGlobalSuperUpdate() { }


    static void DoNothing() { }

}
