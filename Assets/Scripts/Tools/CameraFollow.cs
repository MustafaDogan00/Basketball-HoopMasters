using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//KAMERAYA BU SCRÝPTÝ ATIN , TARGET'A TAKÝP ETMEK ÝSTEDÝÐÝNÝZ OBJEYÝ ATIN.FOLLOW BOOLUNU TRUE YAPIP PLAYE BASIN VE EDITORDEN OFFSET AYARLAYIN. OFFSETÝ KAYDEDÝP ÇIKIN.
public class CameraFollow : MonoBehaviour
{
	public Transform[] target;

	public float smoothSpeed = 0.125f;

	public Vector3 offset;

	private Vector3 velocity;

	public bool follow;

    void LateUpdate()
	{
		if (follow)
        {
			Vector3 desiredPosition = target[PlayerController.Instance.whichPlayer].position + offset;
			Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
			transform.position = smoothedPosition;

			transform.LookAt(target[PlayerController.Instance.whichPlayer]);
		}
	
	}


}