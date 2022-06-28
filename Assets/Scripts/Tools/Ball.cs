using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball Instance;

    private float _distanceY;
    private float _g=-18f;
    [SerializeField] private float _h=5f;

    private Vector3 _velocityY, _velocityXZ, _distanceXZ;

    private Rigidbody _rb;


    
    
    private void Awake()
    {
        Instance = this;
        _rb= GetComponent<Rigidbody>();
    }

  
   public void Shoot(GameObject t)
    {    
            _distanceY = t.transform.position.y - transform.position.y;
            _distanceXZ = new Vector3(t.transform.position.x - transform.position.x, 0, t.transform.position.z - transform.position.z);

            _velocityY = Vector3.up * Mathf.Sqrt(-2 * _g * _h);
            _velocityXZ = _distanceXZ / (Mathf.Sqrt(-2 * _h / _g) + Mathf.Sqrt(2 * (_distanceY - _h) / _g));

            Physics.gravity = Vector3.up * _g;
            GetComponent<SphereCollider>().enabled = true;
            gameObject.transform.SetParent(null);
           // PlayerController.Instance.isBallOnHand = false;
            _rb.useGravity = true;
            _rb.isKinematic = false;

            _rb.velocity = _velocityY + _velocityXZ; 
    }

   


}
