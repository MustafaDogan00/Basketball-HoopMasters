using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
   [SerializeField] private PlayerController _controller;

    public bool passBall;
    public bool shootBall;
   
    public void PassEvent()
    {
        passBall = true;
    }   
    public void ShootEvent()
    {
        shootBall = true;      
    }   
}
