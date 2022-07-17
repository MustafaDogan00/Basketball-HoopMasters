using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtAreas : MonoBehaviour
{
    public enum PlayerStateOnCourt
    {
        DEFENSE1,
        DEFENSE2,
        MID1,
        MID2,
        FRONT1,
        FRONT2,
    }

    [SerializeField] private OppsTeamAIController controller;

    private PlayerStateOnCourt _playerState;
    public PlayerStateOnCourt PlayerState
    {
        get => _playerState;
        set
        {
            _playerState = value;
            OnStateChanged(value);
        }
    }

    public void OnStateChanged(PlayerStateOnCourt state)
    {
        controller.ManageTeamScenario(state);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
