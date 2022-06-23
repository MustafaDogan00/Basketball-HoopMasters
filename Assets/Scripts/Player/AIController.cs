using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private Transform[] aiLocation;

    [SerializeField] private float _speed;

    private Animator _animator;

    private GameObject _mainPlayer;
  
    void Start()
    {
        _animator=transform.GetChild(0).GetComponent<Animator>();
        _mainPlayer = GameObject.FindGameObjectWithTag("MainPlayer");
        
    }

  
    void Update()
    {
       

        if (gameObject.name=="AI1")
        {
            if (PlayerController.Instance.whichPlayer == 1)
            {
                Destroy(gameObject.GetComponent<AIController>());
            }
            if (gameObject.GetComponent<AIController>()!=null && gameObject.GetComponent<PlayerController>() != null)
            {
                gameObject.AddComponent<AIController>();
            }
            if (!PlayerController.Instance.forwOrBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, aiLocation[0].transform.position, _speed * Time.deltaTime);
            }
            if (PlayerController.Instance.forwOrBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, aiLocation[3].transform.position, _speed * Time.deltaTime);
            }           
        }
        if (gameObject.name == "AI2")
        {
            if (PlayerController.Instance.whichPlayer == 2)
            {
                Destroy(gameObject.GetComponent<AIController>());
            }
            if (!PlayerController.Instance.forwOrBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, aiLocation[1].transform.position, _speed * Time.deltaTime);
            }
            if (PlayerController.Instance.forwOrBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, aiLocation[4].transform.position, _speed * Time.deltaTime);
            }
        }





    }
}
