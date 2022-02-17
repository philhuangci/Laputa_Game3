using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judger : CardSuperStateMachine
{

    public enum JudgerState
    {
        Idle,
        PutUpCard,
        PutUpCardIdle,
        PutDownCard,
        FlipCard
    }

    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
        currentState = JudgerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PutUpCard_EnterState()
    {
        animator.Play("PutUpCard");
    }

    public void PutUpCard_ExitState()
    {
        // currentState = JudgerState.PutUpCardIdle;
    }

    public void Idle_EnterState()
    {
        animator.Play("Idle");
    }

    public void Idle_SuperState()
    {

    }

    public void Idle_ExitState()
    {

    }

    public void  PutUpCardIdle_EnterState()
    {
        animator.Play("PutUpCardIdle");
    }

    public void PutUpCardIdle_SuperState()
    {

    }

    public void PutUPCardIdle_ExitState()
    {

    }

    public void FlipCard_EnterState()
    {
        animator.Play("FlipCard");
    }

    public void FlipCard_SuperSate()
    {

    }
    public void FlipCard_ExitState()
    {
        // currentState = JudgerState.PutUpCardIdle;
    }

    public void PutDownCard_EnterState()
    {
        animator.Play("PutDownCard");
    }

    public void PutDownCard_SuperState()
    {

    }

    public void PutDownCard_ExitState()
    {
        // currentState = JudgerState.Idle;
    }




    public void PutUpCard()
    {
        currentState = JudgerState.PutUpCard;
    }

    public void FlipCard()
    {
        currentState = JudgerState.FlipCard;
    }

    public void PutDownCard()
    {
        currentState = JudgerState.PutDownCard;
    }

   







}

