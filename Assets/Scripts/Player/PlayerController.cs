using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [HideInInspector]
    public Animator _playerAnimator;

    [SerializeField] private float _movementSpeed = 4;
    [SerializeField] private float _rotationSpeed = 0.4f;
    [SerializeField] private float _ballSpeed;

    public int whichPlayer = 0;
    public int whichBasket;

    private Rigidbody _ballRb;

    private DynamicJoystick _joyStick;

    private GameObject _ball;
    [HideInInspector]
    public GameObject _basketPos, _leftBasketPos, _rightBasketPos;

   [SerializeField] private Transform _chest;

    public bool isBallOnHand;
    public bool forwOrBack;
    public bool example;
    public bool isTouchingBasket;
    [SerializeField] private bool _canPass;

    List<Vector3> directions=new List<Vector3>();
   
    private void Start()
    {
        Instance = this;
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();       
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _ballRb = _ball.GetComponent<Rigidbody>();
        _basketPos = GameObject.FindGameObjectWithTag("BasketPos");
        _leftBasketPos = GameObject.FindGameObjectWithTag("LeftColliderBasket");
        _rightBasketPos = GameObject.FindGameObjectWithTag("RightColliderBasket");
    }

    [System.Obsolete]
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

        
        Raycasting();

        if (Input.GetMouseButtonUp(0))
        {
            _canPass = true;
        }

        if (!_joyStick.move && _canPass && isBallOnHand && !isTouchingBasket)
        {
            PassBall();
        }
    }
   
    void Raycasting()
    {      
        RaycastHit hit;
        RaycastHit hit1;

        Debug.DrawLine(_chest.transform.position - new Vector3(0, .5f, 0), transform.forward * 5000, Color.red);

        if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "OppSeat")
            {
                forwOrBack = false;
            }
            if (hit.collider.tag == "AlleySeat")
            {
                forwOrBack = true;
            }
        }
        if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), transform.forward, out hit1, Mathf.Infinity))

            if (hit1.collider != null && transform.position.z >= 0)
            {
                if (hit1.collider.tag == "LeftCollider")
                {
                    whichBasket = 0;
                    isTouchingBasket = true;
                }               
               else if (hit1.collider.tag == "MiddleCollider")
                {
                    whichBasket = 1;
                    isTouchingBasket = true;
                    print("middle");
                }
               
               else if (hit1.collider.tag == "RightCollider")
                {
                    whichBasket = 2;
                    isTouchingBasket = true;
                }
                else
                {
                    isTouchingBasket = false;
                }
            }
    }
    void PassBall()
    {
        _ball.gameObject.transform.SetParent(null);
        _ball.transform.DOMove(FindClosestPlayer().GetChild(1).position, .2f);
        FindClosestPlayer().GetComponent<PlayerController>().enabled = true;
        FindClosestPlayer().GetComponent<AIController>().enabled = false;
        gameObject.tag = "Player";
        GetComponent<AIController>().enabled = true;
        _canPass = false;
        isBallOnHand = false;
        this.enabled = false;
    }
    Transform FindClosestPlayer()
    {
        float closestAngle = Mathf.Infinity;
        float currentAngle;
        Transform closestPlayer = null;

        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            currentAngle = Mathf.Abs(Vector3.Angle(transform.forward, item.transform.position - transform.position));
            if (currentAngle < closestAngle)
            {
                closestAngle = currentAngle;
                closestPlayer = item.transform;
            }
        }
        return closestPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Basketball" && !example)
        {
            isBallOnHand = true;
            gameObject.tag = "MainPlayer";
            _ball.transform.position = _chest.position;     
            _ball.transform.SetParent(gameObject.transform);
            _ballRb.isKinematic=true;
            _ballRb.useGravity=false;
        }
    }

}
