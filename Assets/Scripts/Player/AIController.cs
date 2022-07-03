using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIController : MonoBehaviour
{

    [SerializeField] private Transform[] _aiTarget;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Animator _animator;

    private int _posNumber;



    void Start()
    {
        _animator=transform.GetChild(0).GetComponent<Animator>();   
    }


void Update()
{
    if (!PlayerController.Instance.forwOrBack)
    {
        _posNumber = 0;
    }
    else
    {
        _posNumber = 1;
    }
    Vector3 direction = _aiTarget[_posNumber].position - transform.position;
    direction.Normalize();
    if (direction != Vector3.zero)
    {
        Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);
    }


    if (!PlayerController.Instance.forwOrBack)
    {
        transform.position = Vector3.MoveTowards(transform.position, _aiTarget[_posNumber].position, _speed * Time.deltaTime);
        _animator.SetBool("Move", true);
        if (transform.position == _aiTarget[_posNumber].position)
        {
            _animator.SetBool("Move", false);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }      
    }
    else if (PlayerController.Instance.forwOrBack)
    {
        transform.position = Vector3.MoveTowards(transform.position, _aiTarget[_posNumber].position, _speed * Time.deltaTime);
        _animator.SetBool("Move", true);
        if (transform.position == _aiTarget[_posNumber].position)
        {
            _animator.SetBool("Move", false);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }       
    }








}

   
    
}
