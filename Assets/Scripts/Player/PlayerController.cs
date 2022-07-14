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
    [SerializeField] private float _h = 5f;
    private float _distanceY;
    private float _g = -18f;
  
    public int whichPlayer = 0;
    public int whichBasket;

    private Rigidbody _ballRb;

    private DynamicJoystick _joyStick;

    private GameObject _ball;
    [HideInInspector]
    public GameObject _leftBasketPos, _rightBasketPos;

   [SerializeField] private Transform _chest;
    [SerializeField] private Transform[] _basketPos;

    public bool isBallOnHand;
    public bool forwOrBack;
    public bool example;
    public bool isTouchingBasket;
    [SerializeField] private bool _canPass;
    public bool mainPlayerCheck=true;

    List<Vector3> directions=new List<Vector3>();

    private Vector3 _velocityY, _velocityXZ, _distanceXZ;

   public Ease myEase;

    [SerializeField] private Ball _ballS;

    public Collider coll;

    private void Awake()
    {        
        Instance = this;
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();       
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _ballRb = _ball.GetComponent<Rigidbody>();
        _leftBasketPos = GameObject.FindGameObjectWithTag("LeftColliderBasket");
        _rightBasketPos = GameObject.FindGameObjectWithTag("RightColliderBasket");
    }

    void Update()
    {                             
        if (_joyStick.move)         
        {
            _playerAnimator.SetBool("Move",true);

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
            GameManager.Instance.MainPlayer(FindClosestPlayer());
            PassBall();
            
        }
        if (!_joyStick.move && isBallOnHand && _canPass && isTouchingBasket)
        {
            StartCoroutine(ShootBall());
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
        _ballS.WhereToGo(FindClosestPlayer().GetChild(1));
       // _ball.gameObject.tag = "BasketballChild";
        FindClosestPlayer().GetComponent<PlayerController>().enabled = true;
       FindClosestPlayer().GetComponent<AIController>().enabled = false;
        FindClosestPlayer().GetComponent<PlayerController>().coll.enabled = true;
        coll.enabled=false;
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
        if (other.gameObject.tag=="Basketball" && !example )
        {
            isBallOnHand = true;
            gameObject.tag = "MainPlayer";
           // _ball.gameObject.tag = "Basketball";
            _ball = GameObject.FindGameObjectWithTag("Basketball");
            _ball.transform.position = _chest.position;
            _ballS.go = false;
            _ball.transform.SetParent(gameObject.transform);
            _ball.GetComponent<Rigidbody>().isKinematic=true;
            _ball.GetComponent<Rigidbody>().useGravity=false;           
        }
    }
    IEnumerator ShootBall()
    {
        example = true;
        isBallOnHand = false;
        _ball.transform.SetParent(null);
        _ballRb.isKinematic = false;
        _ballRb.useGravity = true;
        Physics.gravity = Vector3.up * _g;
        _ballRb.velocity = CalculateBallVelocity();
        yield return new WaitForSeconds(.5f);
        example = false;
    }
    Vector3 CalculateBallVelocity()
    {
        _distanceY = _basketPos[whichBasket].transform.position.y - _ball.transform.position.y;
        _distanceXZ = new Vector3(_basketPos[whichBasket].transform.position.x - _ball.transform.position.x, 0, _basketPos[whichBasket].transform.position.z - _ball.transform.position.z);

        _velocityY = Vector3.up * Mathf.Sqrt(-2 * _g * _h);
        _velocityXZ = _distanceXZ / (Mathf.Sqrt(-2 * _h / _g) + Mathf.Sqrt(2 * (_distanceY - _h) / _g));
        return _velocityY + _velocityXZ;
    }

    //public GameObject MainPlayer()
    //{
    //    if (mainPlayerCheck)
    //    {
    //        GameObject player = null;
    //        switch (gameObject.name)
    //        {
    //            case "PlayerHolder":
    //                player = gameObject;
    //                break;
    //            case "AI1":
    //                player = gameObject;
    //                break;
    //            case "AI2":
    //                player = gameObject;
    //                break;
    //            case "AI3":
    //                player = gameObject;
    //                break;
    //            case "AI4":
    //                player = gameObject;
    //                break;
    //        }
    //        return player;
    //    }
    //    else
    //        return null;
    //}
}
