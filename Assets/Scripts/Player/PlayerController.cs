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
    public float shootPower;

    public int whichPlayer=0;

    private Rigidbody _playerRigidbody;
    private Rigidbody _ballRb;

    private DynamicJoystick _joyStick;

    private GameObject _ball;
    private Transform _chest;

    [SerializeField] private Transform _basketPos;

    public bool _isBallOnHand;
   

    private void Start()
    {
        Instance = this;
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _chest = gameObject.transform.GetChild(1).transform;
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
        if (_isBallOnHand)
        {
            switch (gameObject.name)
            {
                case "PlayerHolder":
                    whichPlayer = 0;
                    break;
                case "AI1":
                    whichPlayer = 1;
                    break;
                case "AI2":
                    whichPlayer = 2;
                    break;
                case "AI3":
                    whichPlayer = 3;
                    break;
                case "AI4":
                    whichPlayer = 4;
                    break;
            }
        }
    }

    void BallDistance()
    {
        float distance =Vector3.Distance(gameObject.transform.position, _ball.transform.position);
        if (distance <=.5f)
        {
            _ball.transform.position = _chest.transform.position + Vector3.forward;
            _ball.gameObject.transform.SetParent(transform);
            _isBallOnHand = true;
        }
    }

   void Raycasting()
    {
        Quaternion spreadAnglePos = Quaternion.AngleAxis(30.0f, new Vector3(0, 1, 0));
        Quaternion spreadAngleNeg = Quaternion.AngleAxis(-30.0f, new Vector3(0, 1, 0));


        RaycastHit hit;
        RaycastHit hit1;
        RaycastHit hit2;
        if (Physics.Raycast(_chest.transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawLine(_chest.transform.position, transform.TransformDirection(Vector3.forward), Color.red);

            if (hit.collider.tag == "Player" && !_joyStick.move)
            {
                PassingBall(hit.transform.position, hit.transform.gameObject);              
            }
        }

        if (Physics.Raycast(_chest.transform.position, transform.TransformDirection(spreadAnglePos* Vector3.forward), out hit1, Mathf.Infinity))
        {
              Debug.DrawLine(_chest.transform.position, transform.TransformDirection(spreadAnglePos * Vector3.forward) * 6, Color.blue);
       
            if (hit.collider.tag == "Player" && !_joyStick.move)
            {
                PassingBall(hit.transform.position, hit.transform.gameObject);
            }
        }

        if (Physics.Raycast(_chest.transform.position, transform.TransformDirection(spreadAngleNeg * Vector3.forward), out hit2, Mathf.Infinity))
        {
            Debug.DrawLine(_chest.transform.position, transform.TransformDirection(spreadAngleNeg * Vector3.forward) * 6, Color.blue);

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
        if (_isBallOnHand )
        {
            _ball.transform.DOJump(passVector,1,1,1);
            //_ballRb.useGravity = true;
            //_ballRb.isKinematic = false;
            _ball.gameObject.transform.SetParent(null);
            _isBallOnHand = false;
            Destroy(gameObject.GetComponent<PlayerController>());
            AI.AddComponent<PlayerController>();
            print("passingBall");
        }
    }

}
