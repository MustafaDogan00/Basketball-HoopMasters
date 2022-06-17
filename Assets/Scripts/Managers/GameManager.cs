using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Update()
    {
        //Raycasting();
    }

   /* void Raycasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerController.Instance.gameObject.transform.position, PlayerController.Instance.gameObject.transform.forward, out hit,6f))
        {
            Debug.DrawLine(PlayerController.Instance.gameObject.transform.position, hit.point,Color.red);
            if (hit.collider.tag=="Player")
            {
                Destroy(hit.transform.gameObject);
            }

        }

    }*/
}
