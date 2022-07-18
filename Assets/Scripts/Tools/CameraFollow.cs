using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraFollow : MonoBehaviour
{
    public enum WhichPlayer
    {
        PLAYERHOLDER,
        AI1,
        AI2,
        AI3,
        AI4
    }
    public  WhichPlayer whichPlayer;

    public Transform[] target;

	public float smoothSpeed = 0.125f;

	public Vector3 offset;

	private Vector3 velocity;

	public bool follow;

    private int playerNum;
   
    void LateUpdate()
	{
        PlayerChange();
		if (follow)
		{
            Vector3 desiredPosition = target[playerNum].position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
            //transform.LookAt(target[playerNum]);
        }
	}

   void PlayerChange()
    {
        switch(whichPlayer)
        {

            case WhichPlayer.PLAYERHOLDER:
                playerNum=0;
                break;
            case WhichPlayer.AI1:
                playerNum = 1;
                break;
            case WhichPlayer.AI2:
                playerNum = 2;
                break;
            case WhichPlayer.AI3:
                playerNum = 3;
                break;
            case WhichPlayer.AI4:
                playerNum = 4;
                break;
        }
    }
}