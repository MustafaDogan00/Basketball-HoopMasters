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
    private float _rotation;
    private float _rotationLimit=60;
    private float _rotationPower=150;
    private float[] _angle;
  


    public int whichPlayer = 0;
    public int whichBasket;

    private Rigidbody _ballRb;

    private DynamicJoystick _joyStick;

    private GameObject _ball;
    [HideInInspector]
    public GameObject _basketPos, _leftBasketPos, _rightBasketPos;
    private PlayerController[] _teammates;

    private Transform _chest;

    public bool isBallOnHand;
    public bool forwOrBack;
    public bool example;
    public bool isTouchingBasket;

    private Quaternion _q;
    private Vector3 _v;

    private void Awake()
    {
        _teammates=FindObjectsOfType<PlayerController>().Where(t => t != this).ToArray();
    }
    private void Start()
    {
        Instance = this;
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();       
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _chest = gameObject.transform.GetChild(1).transform;
        _ballRb = _ball.GetComponent<Rigidbody>();
        _basketPos = GameObject.FindGameObjectWithTag("BasketPos");
        _leftBasketPos = GameObject.FindGameObjectWithTag("LeftColliderBasket");
        _rightBasketPos = GameObject.FindGameObjectWithTag("RightColliderBasket");
    }

    [System.Obsolete]
    void Update()
    {                         
        //_rotation += _rotationPower * Time.deltaTime;
        //_q = Quaternion.Euler(0, 0, _rotation);
        //_v = _q.ToEulerAngles();
        //if (_rotation >= _rotationLimit)
        //{
        //    _rotation = 0;
        //}

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

        if (!_joyStick.move && isBallOnHand)
        {
            var targetPlayer = ClosestAngle();
            if (targetPlayer != null)
            {
             PassBallToPlayer(targetPlayer);
            }
        }
        BallDistance();
        Raycasting();
        ClosestAngle();
        WhichPlayer();

    }

    void BallDistance()
    {
        float distance = Vector3.Distance(gameObject.transform.position, _ball.transform.position);
        if (distance <= 2f)
        {
            if (!example)
            {
                _ball.transform.position = _chest.transform.position;
            }
            isBallOnHand = true;
            gameObject.tag = "MainPlayer";
        }
        else
        {
            isBallOnHand = false;
            example = false;
        }
    }
    void WhichPlayer()
    {

        if (isBallOnHand)
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
        else
        {
            whichPlayer = 5;
        }
    }
    void Raycasting()
    {
        //RaycastHit hit;
        //RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;

        //Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), (transform.forward + _v) * 500, Color.blue);
        //Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), (transform.forward + -_v) * 5000, Color.red);
        //Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), transform.forward * 5000, Color.red);

        //if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), (transform.forward + _v) * 500, out hit, Mathf.Infinity))
        //{
        //    if (hit.collider.tag == "Player" && !_joyStick.move && isBallOnHand)
        //    {
        //        SwitchPlayer(hit.collider.gameObject);
        //    }
        //}


        //if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), (transform.forward + -_v) * 5000, out hit2, Mathf.Infinity))
        //{
        //    if (hit2.collider.tag == "Player" && !_joyStick.move && isBallOnHand)
        //    {
        //        if (Vector3.Distance(hit2.collider.gameObject.transform.position, transform.position) <= Vector3.Distance(hit.collider.gameObject.transform.position, transform.position))
        //        {
        //            StartCoroutine(ChangingPlayer(hit.transform.GetChild(1).transform.position, hit.transform.gameObject));
        //            closestPlayer = true;
        //            passBall = true;
        //        }
        //    }
        //}

        if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), transform.forward, out hit3, Mathf.Infinity))
        {
            if (hit3.collider.tag == "OppSeat")
            {
                forwOrBack = false;
            }
            if (hit3.collider.tag == "AlleySeat")
            {
                forwOrBack = true;
            }
        }

        if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), transform.forward, out hit4, Mathf.Infinity))

            if (hit4.collider!=null)
            {
                if (hit4.collider.tag == "LeftCollider" && !_joyStick.move)
                {

                    if (transform.position.z >= 0 && isBallOnHand)
                    {
                        whichBasket = 0;
                        isTouchingBasket = true;
                    }
                    else
                    {
                        isTouchingBasket = false;
                    }

                }

                if (hit4.collider.tag == "MiddleCollider" && !_joyStick.move)
                {
                    if (transform.position.z >= 0 && isBallOnHand)
                    {
                        whichBasket = 1;
                        isTouchingBasket = true;
                    }
                    else
                    {
                        isTouchingBasket = false;
                    }

                }


                if (hit4.collider.tag == "RightCollider" && !_joyStick.move)
                {
                    if (transform.position.z >= 0 && isBallOnHand)
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
    }


    //IEnumerator ChangingPlayer(Vector3 passVector, GameObject AI)
    //{
    //    if (isBallOnHand)
    //    {
    //        _ball.GetComponent<SphereCollider>().enabled = true;
    //        _ball.transform.DOMove(passVector, .1f);
    //        _ballRb.useGravity = true;
    //        _ballRb.isKinematic = false;
    //        _ball.gameObject.transform.SetParent(AI.transform);
    //        isBallOnHand = false;
    //        gameObject.tag = "Player";
    //        yield return new WaitForSeconds(.5f);
    //        isBallOnHand = true;
    //        _playerAnimator.SetBool("Move", false);
    //        AI.GetComponent<PlayerController>().enabled = true;
    //        AI.GetComponent<AIController>().enabled = false;
    //        GetComponent<AIController>().enabled = true;
    //        this.enabled = false;
    //    }
    //}

       private Vector3 DirectionTo(PlayerController player)
       {
         return Vector3.Normalize(player.transform.position - transform.position);
       }
       private PlayerController ClosestAngle()
       {
            PlayerController selectedPlayer = null;
            float angle = Mathf.Infinity;
            foreach (var player in _teammates)
            {
                var directionToPlayer = DirectionTo(player);
                Debug.DrawRay(transform.position, directionToPlayer, Color.blue);
                var playerAngle=Vector3.Angle(transform.forward, directionToPlayer);
                if (playerAngle < angle)
                {
                    selectedPlayer = player;
                    angle = playerAngle;
                }              
            }
              return selectedPlayer;
       }

    private void PassBallToPlayer(PlayerController targetPlayer)
    {
        var direction = DirectionTo(targetPlayer);
        _ball.transform.SetParent(null);
        _ball.GetComponent<Rigidbody>().isKinematic = false;
        _ball.GetComponent<Rigidbody>().AddForce(direction*_ballSpeed*Time.deltaTime);
    }
}
