using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIController : MonoBehaviour
{

    [SerializeField] private Transform _aiTarget1;
    [SerializeField] private Transform _aiTarget2;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Animator _animator;

  
    void Start()
    {
        _animator=transform.GetChild(0).GetComponent<Animator>();   
    }

  
    void Update()
    {

        if (!PlayerController.Instance.forwOrBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, _aiTarget1.position, _speed * Time.deltaTime);
            _animator.SetBool("Move",true);
            if (transform.position==_aiTarget1.position)
            {
                _animator.SetBool("Move", false);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else if (PlayerController.Instance.forwOrBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, _aiTarget2.position, _speed * Time.deltaTime);
            _animator.SetBool("Move", true);
            if (transform.position == _aiTarget2.position)
            {
                _animator.SetBool("Move", false);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }



        Vector3 direction = _aiTarget1.position - transform.position;
        direction.Normalize();
        if (direction!=Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);
        }
      
       


    }

   
    
}
