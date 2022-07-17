using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppsTeam : MonoBehaviour
{
    public enum AIState
    {
        IDLE,
        PRESSURE
    }

    public AIState state=AIState.IDLE;

    private Ball _ball;

    private float _speed;

    private OppsTeamAIController _controller;

    private Vector3 ballPos=Vector3.one;

    private Vector3 firstPos;

    private Vector3 ballOffset;

    private Vector3 velo;

    private bool backPressure;

    private void Awake()
    {
        _ball = GameObject.FindGameObjectWithTag("Basketball").GetComponent<Ball>();

        _controller=transform.parent.GetComponent<OppsTeamAIController>();

        firstPos = transform.position;

        ballOffset = (transform.position - _ball.transform.position);
    }

    private void LateUpdate()
    {
        if (state==AIState.PRESSURE)
        {
            PressureMainPlayer();
        }
        else
        {
            Idle();
        }
    }
    public void PressureMainPlayer()
    {
        backPressure = true;
        Vector3 direction=_ball.transform.position-transform.position;
        direction.y = 0;
        if (direction.magnitude > 2f)
        {
            transform.position+=direction.normalized*_controller.speed*Time.deltaTime;
        }

    }

    public void Idle()
    {
        if (backPressure)
        {
            Vector3 direction = firstPos - transform.position;
            direction.y = 0;              
            transform.position += direction.normalized * _controller.speed * Time.deltaTime;
            if ((transform.position - firstPos).magnitude <1f)
            {
                backPressure=false;
            }
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _ball.transform.position + ballOffset, ref velo, 0.5f);
            Vector3 refPos=transform.position;
            refPos.y = 0.015f;
            transform.position = refPos;
        }


    }
}
