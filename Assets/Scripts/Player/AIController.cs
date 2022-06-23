using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIController : MonoBehaviour
{

    [SerializeField] private Transform _aiTarget1;
    [SerializeField] private Transform _aiTarget2;

    [SerializeField] private float _speed;

    private Animator _animator;

    private GameObject _mainPlayer;
    private GameObject _ai1;
  
    void Start()
    {
        _animator=transform.GetChild(0).GetComponent<Animator>();
        _mainPlayer = GameObject.FindGameObjectWithTag("MainPlayer");
        _ai1 = GameObject.Find("AI1");
        
    }

  
    void Update()
    {


        //if (gameObject.name=="AI1")
        //{
        //    if (PlayerController.Instance.whichPlayer == 1)
        //    {
        //        this.enabled = false;
        //        print("false");
        //    }
        //    if ()
        //    {

        //    }
        //   else if (PlayerController.Instance.whichPlayer != 1 && _ai1.GetComponent<AIController>()==null)
        //    {
        //        _ai1.AddComponent<AIController>();
        //        print("true");
        //    }
        //    if (!PlayerController.Instance.forwOrBack)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, aiLocation[0].transform.position, _speed * Time.deltaTime);
        //        //transform.DOMove(aiLocation[0].transform.position, _speed);
        //    }
        //    if (PlayerController.Instance.forwOrBack)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, aiLocation[3].transform.position, _speed * Time.deltaTime);
        //    }           
        //}
        //if (gameObject.name == "AI2")
        //{
        //    if (PlayerController.Instance.whichPlayer == 2)
        //    {
        //        Destroy(gameObject.GetComponent<AIController>());
        //        print("false");
        //    }
        //    if (PlayerController.Instance.whichPlayer == 2)
        //    {
        //        Destroy(gameObject.GetComponent<AIController>());
        //    }
        //    if (!PlayerController.Instance.forwOrBack)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, aiLocation[1].transform.position, _speed * Time.deltaTime);
        //        _animator.SetBool("Move",true);
        //        if (Vector3.Distance(transform.position, aiLocation[1].transform.position)<=.2f)
        //        {
        //            _animator.SetBool("Move",false);
        //        }
        //    }
        //    if (PlayerController.Instance.forwOrBack)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, aiLocation[4].transform.position, _speed * Time.deltaTime);
        //        _animator.SetBool("Move", true);
        //        if (Vector3.Distance(transform.position, aiLocation[4].transform.position) <= .2f)
        //        {
        //            _animator.SetBool("Move", false);
        //        }
        //    }
        //}
        //if (gameObject.name == "AI3")
        //{
        //    if (PlayerController.Instance.whichPlayer == 3)
        //    {
        //        Destroy(gameObject.GetComponent<AIController>());
        //        print("false");
        //    }
        //    if (PlayerController.Instance.whichPlayer == 3)
        //    {
        //        Destroy(gameObject.GetComponent<AIController>());
        //    }
        //    if (!PlayerController.Instance.forwOrBack)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, aiLocation[5].transform.position, _speed * Time.deltaTime);
        //    }
        //    if (PlayerController.Instance.forwOrBack)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, aiLocation[7].transform.position, _speed * Time.deltaTime);
        //    }
        //}
        //if (gameObject.name == "AI4")
        //{
        //    if (PlayerController.Instance.whichPlayer == 4)
        //    {
        //        Destroy(gameObject.GetComponent<AIController>());
        //        print("false");
        //    }
        //    if (PlayerController.Instance.whichPlayer == 4)
        //    {
        //        Destroy(gameObject.GetComponent<AIController>());
        //    }
        //    if (!PlayerController.Instance.forwOrBack)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, aiLocation[6].transform.position, _speed * Time.deltaTime);
        //    }
        //    if (PlayerController.Instance.forwOrBack)
        //    {
        //       transform.position = Vector3.MoveTowards(transform.position, aiLocation[6].transform.position, _speed * Time.deltaTime);
        //    }


        if (!PlayerController.Instance.forwOrBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, _aiTarget1.position, _speed * Time.deltaTime);
        }
        else if (PlayerController.Instance.forwOrBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, _aiTarget2.position, _speed * Time.deltaTime);
        }
        //else if (PlayerController.Instance.stopAI)
        //{
        //    transform.position = transform.position;

        //}
        
    }

   
    
}
