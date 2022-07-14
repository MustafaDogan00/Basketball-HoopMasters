using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIController : MonoBehaviour
{
    [SerializeField] private Transform[] _aiTarget;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private GameObject[] _players;

    private Animator _animator;

    private int _posNumber;

    public Transform mainPlayer;

    void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        if (mainPlayer.transform.position.z <= -12)
        {
            _posNumber = 0;
            WhereToGo(_aiTarget[_posNumber]);
        }
        else if (mainPlayer.position.z <= -5.4f && mainPlayer.position.z >= -12f)
        {
            _posNumber = 1;
            WhereToGo(_aiTarget[_posNumber]);
        }
        else if (mainPlayer.position.z <= 0 && mainPlayer.position.z >= -5.4f)
        {
            _posNumber = 2;
            WhereToGo(_aiTarget[_posNumber]);
        }
        else if (mainPlayer.position.z >= 0 && mainPlayer.position.z <= 4.8f)
        {
            _posNumber = 3;
            WhereToGo(_aiTarget[_posNumber]);
        }
        else if (mainPlayer.position.z >= 4.8f)
        {
            _posNumber = 4;
            WhereToGo(_aiTarget[_posNumber]);
        }
        Direction(_aiTarget[_posNumber]);
    }

    void Direction(Transform dir)
    {
        Vector3 direction = dir.position - transform.position;
        direction.Normalize();
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);
        }
    }
    void WhereToGo(Transform dir)
    {
        transform.position = Vector3.MoveTowards(transform.position, dir.position, _speed * Time.deltaTime);
        _animator.SetBool("Move", true);
        if (transform.position == dir.position)
        {
            _animator.SetBool("Move", false);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Direction(mainPlayer.transform);
        }
    }

   
}
