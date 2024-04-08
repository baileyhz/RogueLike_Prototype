using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] PlayerState[] states;
    
    PlayerController controller;

    Character character;

    Animator animator;

    PlayerInput input;

	private void Awake()
    {
        animator = GetComponent<Animator>();

        controller = GetComponent<PlayerController>();

        input = GetComponent<PlayerInput>();
        
        character = GetComponent<Character>();

        //ª¬ºAªì©l¤Æ
        stateTable = new Dictionary<System.Type, IState>(states.Length);

        foreach (PlayerState state in states)
        {
            state.Initialize(animator ,this ,input ,controller ,character);
            stateTable.Add(state.GetType(), state);
        }
    }

    private void Start()
    {
        SwitchOn(stateTable[typeof(PlayerState_Idle)]);
    }

    public void ChangeState(System.Type stateType, PlayerState newstate)
    {
        stateTable[stateType] = newstate;
	}
}
