using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public static Ball Instance;

    private GameObject _player;

    public float _speed;

    public Transform target;

    public bool go;

    public SphereCollider ballCollider;

    public bool isBallOnGround;
    public bool defense;   

    public int teamScore=0;

    private void Awake()
    {
        Instance = this;
        ballCollider=GetComponent<SphereCollider>();
    }
    private void Update()
    {
        if (go)
        {            
            Vector3 direction = target.transform.position - transform.position;
            transform.position += direction.normalized * _speed * Time.deltaTime;          
        }    
    }   
    public void WhereToGo(Transform targetT)
    {
        target=targetT;
        go=true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Swish")
        {
           GetComponent<AudioSource>().Play();
            if (PlayerController.Instance.twoPoints)
            {
                teamScore +=2;
            }
            else
            {
                teamScore += 3;
            }
        }

        if (other.gameObject.tag == "Ground")
        {
            isBallOnGround = true;
        }
        else
            isBallOnGround = false;


        if (other.gameObject.tag=="Player" || other.gameObject.tag=="Ground")
        {
            defense=true;
        }
        else
        {
            defense = false;
        }
    }    
}
