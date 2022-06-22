using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private Transform[] aiLocation;

    [SerializeField] private float _speed=15;
  
    void Start()
    {
        
    }

  
    void Update()
    {
        if (PlayerController.Instance.isBallOnHand)
        {
            Destroy(gameObject.GetComponent<AIController>());
        }


        if (gameObject.name=="AI1")
        {
            transform.position = Vector3.MoveTowards(transform.position, aiLocation[0].transform.position, _speed*Time.deltaTime);

        }
      





    }
}
