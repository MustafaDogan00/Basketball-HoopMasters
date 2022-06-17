using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [HideInInspector]
    public Animator _playerAnimator;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed = 0.4f;
    private float _ballSpeed=100f;

    private Rigidbody _playerRigidbody;

    private DynamicJoystick _joyStick;

    private Quaternion _lastRotation;

    private GameObject _ball;
    private GameObject _rightHand;
    private GameObject _basket1,_basket2;

    private bool _hasBall;
   

    private void Awake()
    {
        Instance = this;
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _rightHand = GameObject.FindGameObjectWithTag("RightHand");
        _basket1 = GameObject.FindGameObjectWithTag("Basket1");
        _basket2 = GameObject.FindGameObjectWithTag("Basket2");

    }



    void Update()
    {
        if (_hasBall)
        {
            if (_joyStick.move)
            {
                _playerAnimator.SetBool("Move", true);

                Vector3 direction = _joyStick.Vertical * Vector3.forward + _joyStick.Horizontal * Vector3.right;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);
                }
                transform.position += direction * _movementSpeed * Time.deltaTime;
            }
            else
            {
                _playerAnimator.SetBool("Move", false);
            }
        }
       
        BallDistance();
        Raycasting();
    }

    void BallDistance()
    {
        float distance =Vector3.Distance(gameObject.transform.position, _ball.transform.position);
        if (distance <=1)
        {
            _ball.transform.position=_rightHand.transform.position;

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Basketball")
        {
            _hasBall = true;
        }
        else
        {
            _hasBall = false;
        }

    }

    void Raycasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.collider.tag == "Basket1"  )
            {
                _ball.transform.position=Vector3.Lerp(_ball.transform.position,_basket1.transform.position, _ballSpeed*Time.deltaTime);
            }
            if (hit.collider.tag == "Basket2")
            {
                _ball.transform.position = Vector3.Lerp(_ball.transform.position, _basket2.transform.position, _ballSpeed * Time.deltaTime);
            }
        }

    }
}
