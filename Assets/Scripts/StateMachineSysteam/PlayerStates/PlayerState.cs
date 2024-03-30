using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : ScriptableObject,IState
{
    private float stateStartTime;

    protected float currentSpeed;

    protected Animator animator;

    protected PlayerController controller;

    protected Character character;

    protected PlayerStateMachine stateMachine;

    protected PlayerInput input;


    protected bool IsAinmationFinished => 1 < animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

    protected float stateDuration => Time.time - stateStartTime;

    public void Initialize(Animator animator , PlayerStateMachine stateMachine,PlayerInput input , PlayerController playerController , Character character)
    {
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.controller = playerController;
        this.input = input;
        this.character = character;

	}

    public virtual void Enter()
    {
		//stateStartTime = Time.time;
	}

    public virtual void Exit()
    {
       
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicUpdate()
    {
       
    }

   
}
