using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppsTeamAIController : MonoBehaviour
{
    [SerializeField] private OppsTeam defensePlayer1;
    [SerializeField] private OppsTeam defensePlayer2;
    [SerializeField] private OppsTeam midPlayer;
    [SerializeField] private OppsTeam frontPlayer1;
    [SerializeField] private OppsTeam frontPlayer2;


    [SerializeField] private CourtAreas courtAreas;

    public float speed;

    public float followDistance;

    public void ManageTeamScenario(CourtAreas.PlayerStateOnCourt state)
    {
        switch (state)
        {
            case CourtAreas.PlayerStateOnCourt.DEFENSE1:
                frontPlayer1.state = OppsTeam.AIState.PRESSURE;
                defensePlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer2.state = OppsTeam.AIState.IDLE;
                midPlayer.state = OppsTeam.AIState.IDLE;
                frontPlayer2.state = OppsTeam.AIState.IDLE;
                break;
            case CourtAreas.PlayerStateOnCourt.DEFENSE2:
                frontPlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer2.state = OppsTeam.AIState.IDLE;
                midPlayer.state = OppsTeam.AIState.IDLE;
                frontPlayer2.state = OppsTeam.AIState.PRESSURE;

                break;
            case CourtAreas.PlayerStateOnCourt.MID1:
                frontPlayer1.state = OppsTeam.AIState.PRESSURE;
                defensePlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer2.state = OppsTeam.AIState.IDLE;
                midPlayer.state = OppsTeam.AIState.PRESSURE;
                frontPlayer2.state = OppsTeam.AIState.IDLE;
                break;
            case CourtAreas.PlayerStateOnCourt.MID2:
                frontPlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer2.state = OppsTeam.AIState.IDLE;
                midPlayer.state = OppsTeam.AIState.PRESSURE;
                frontPlayer2.state = OppsTeam.AIState.PRESSURE;
                break;
            case CourtAreas.PlayerStateOnCourt.FRONT1:
                frontPlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer1.state = OppsTeam.AIState.PRESSURE;
                defensePlayer2.state = OppsTeam.AIState.IDLE;
                midPlayer.state = OppsTeam.AIState.PRESSURE;
                frontPlayer2.state = OppsTeam.AIState.IDLE;
                break;
            case CourtAreas.PlayerStateOnCourt.FRONT2:
                frontPlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer1.state = OppsTeam.AIState.IDLE;
                defensePlayer2.state = OppsTeam.AIState.PRESSURE;
                midPlayer.state = OppsTeam.AIState.PRESSURE;
                frontPlayer2.state = OppsTeam.AIState.IDLE;
                break;

        }
    }


}
