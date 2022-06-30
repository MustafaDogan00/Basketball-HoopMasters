using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public static Ball Instance;

    [SerializeField] private Transform[] _basketPos;

    private float _distanceY;
    private float _g = -18f;
    private float _passSpeed= 18f;
    [SerializeField] private float _h = 5f;

    private Vector3 _velocityY, _velocityXZ, _distanceXZ;

    private Rigidbody _rb;

    private DynamicJoystick _joyStick;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
    }

    private void Update()
    {
        if (!_joyStick.move && PlayerController.Instance.isBallOnHand && PlayerController.Instance.isTouchingBasket)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Physics.gravity = Vector3.up * _g;
        PlayerController.Instance.example = true;
        gameObject.transform.SetParent(null);
        _rb.velocity = CalculateVelocity();
    }
    Vector3 CalculateVelocity()
    {
        _distanceY = _basketPos[PlayerController.Instance.whichBasket].transform.position.y - transform.position.y;
        _distanceXZ = new Vector3(_basketPos[PlayerController.Instance.whichBasket].transform.position.x - transform.position.x, 0, _basketPos[PlayerController.Instance.whichBasket].transform.position.z - transform.position.z);

        _velocityY = Vector3.up * Mathf.Sqrt(-2 * _g * _h);
        _velocityXZ = _distanceXZ / (Mathf.Sqrt(-2 * _h / _g) + Mathf.Sqrt(2 * (_distanceY - _h) / _g));
        return _velocityY + _velocityXZ;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Swish")
        {
           GetComponent<AudioSource>().Play();
        }
    }

    public void Pass(GameObject g,GameObject a)
    {
        PlayerController.Instance.example = true;
        gameObject.transform.SetParent(a.transform);
        gameObject.transform.DOMove(g.transform.position, .1f);


    }


}
