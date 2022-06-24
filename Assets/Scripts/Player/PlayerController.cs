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
    [SerializeField] private float _rotation;

    public int whichPlayer=0;

    private Rigidbody _playerRigidbody;
    private Rigidbody _ballRb;

    private DynamicJoystick _joyStick;

    private GameObject _ball;
    private GameObject _basketPos;

    private Transform _chest;
   
    public bool isBallOnHand;
    public bool forwOrBack;
    public bool stopAI;
    public bool closestPlayer;

    private Quaternion _q;
    private Vector3 _v;



    private void Start()
    {
        Instance = this;
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _chest = gameObject.transform.GetChild(1).transform;
        _ballRb = _ball.GetComponent<Rigidbody>();
        _basketPos = GameObject.FindGameObjectWithTag("BasketPos");
        _ballRb.useGravity = false;
        _ballRb.isKinematic = true;       
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

       
        BallDistance();
        Raycasting();
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
        _rotation+=500*Time.deltaTime;
         _q = Quaternion.Euler(0, 0, _rotation);
        _v=_q.ToEulerAngles();
        if (_rotation>=360)
        {
            _rotation=0;
        }
      
    }

    void BallDistance()
    {
        float distance = Vector3.Distance(gameObject.transform.position, _ball.transform.position);
        if (distance <= 2f)
        {
            _ball.transform.position = _chest.transform.position;
           // _ball.gameObject.transform.SetParent(transform);
            isBallOnHand = true;
            _ballRb.useGravity = false;
            gameObject.tag = "MainPlayer";
            _ball.GetComponent<SphereCollider>().enabled = false;
        }    

       
    }

    void Raycasting()
    {      
        RaycastHit hit;      
        RaycastHit hit2;      
        RaycastHit hit3;
        RaycastHit hit4;
     
        Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), (transform.forward+_v)*5000 , Color.blue);
        Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), (transform.forward + -_v)*5000 , Color.red);

        if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), (transform.forward + _v)*5000 , out hit, Mathf.Infinity))
        {

            if (hit.collider.tag == "Player" && !_joyStick.move && isBallOnHand && !closestPlayer)
            {
                StartCoroutine(PassingBall(hit.transform.GetChild(1).transform.position, hit.transform.gameObject));
            }
        }
        if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), (transform.forward + -_v)*5000 , out hit2, Mathf.Infinity))
        {

            if (hit2.collider.tag == "Player" && !_joyStick.move && isBallOnHand)
            {
                if (Vector3.Distance(hit2.collider.gameObject.transform.position, transform.position) <= Vector3.Distance(hit.collider.gameObject.transform.position, transform.position))
                {
                    StartCoroutine(PassingBall(hit.transform.GetChild(1).transform.position, hit.transform.gameObject));
                    closestPlayer = true;
                }

            }
        }



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
        {
            if (hit3.collider.tag == "Basket2" && !_joyStick.move)
            {
                ShootingBall();
            }
        }

    }
    void ShootingBall()
    {
        if (isBallOnHand && !_joyStick.move)
        {
            _ball.transform.DOJump(_basketPos.transform.position, 2, 1, 1.5f);
            _ballRb.useGravity = true;
            _ballRb.isKinematic = false;
            _ball.GetComponent<SphereCollider>().enabled = true;

            _ball.gameObject.transform.SetParent(null);
            isBallOnHand=false;
        }
    }
    IEnumerator PassingBall(Vector3 passVector,GameObject AI)
    {
        if (isBallOnHand )
        {
            _ball.GetComponent<SphereCollider>().enabled=true;
            _ball.transform.DOMove(passVector,.1f);
            _ballRb.useGravity = true;
            _ballRb.isKinematic = false;
            _ball.gameObject.transform.SetParent(AI.transform);
            isBallOnHand = false;           
            stopAI = true;
            gameObject.tag = "Player";       
            yield return new WaitForSeconds(.5f);
            isBallOnHand = true;
            stopAI = false;
            _playerAnimator.SetBool("Move",false);
            AI.GetComponent<PlayerController>().enabled = true;
            AI.GetComponent<AIController>().enabled = false;
            GetComponent<AIController>().enabled = true; 
            this.enabled = false;

            
           
        }
    }
}
