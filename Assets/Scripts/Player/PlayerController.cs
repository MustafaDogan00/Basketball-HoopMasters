using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [HideInInspector]
    public Animator _playerAnimator;

    [SerializeField] private float _movementSpeed=4;
    [SerializeField] private float _rotationSpeed = 0.4f;
    private float _ballSpeed = 15f;
    public float shootPower;

    private Rigidbody _playerRigidbody;
    private Rigidbody _ballRb;

    private DynamicJoystick _joyStick;

    private GameObject _ball;

    [SerializeField] private Transform _basketPos;

    private bool _isBallOnHand;
   

    private void Start()
    {
        Instance = this;
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _ballRb = _ball.GetComponent<Rigidbody>();
        _ballRb.useGravity = false;
        _ballRb.isKinematic = true;       
    }



    void Update()
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
        
       
        BallDistance();

        Raycasting();
    }

    void BallDistance()
    {
        float distance =Vector3.Distance(gameObject.transform.position, _ball.transform.position);
        if (distance <=.5f)
        {
            _ball.transform.position= transform.position+new Vector3(0,1,.5f);
            _ball.gameObject.transform.SetParent(transform);
            _isBallOnHand = true;
        }
    }

   void Raycasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

            if (hit.collider.tag == "Player" && !_joyStick.move)
            {
                PassingBall(hit.transform.position, hit.transform.gameObject);
            }
          
        }
    }
    void ShootingBall()
    {
        if (_isBallOnHand && !_joyStick.move)
        {
            _ball.transform.DOJump(_basketPos.position, 1, 1, 1);
            _ballRb.useGravity = true;
            _ballRb.isKinematic = false;
            _ball.gameObject.transform.SetParent(null);
            _isBallOnHand=false;
        }
    }
    void PassingBall(Vector3 passVector,GameObject AI)
    {
        if (_isBallOnHand && !_joyStick.move)
        {
            _ball.transform.DOMove(passVector, 1);
            _ballRb.useGravity = true;
            _ballRb.isKinematic = false;
            _ball.gameObject.transform.SetParent(null);
            _isBallOnHand = false;
            Destroy(gameObject.GetComponent<PlayerController>());
            AI.AddComponent<PlayerController>();
        }
    }

}
