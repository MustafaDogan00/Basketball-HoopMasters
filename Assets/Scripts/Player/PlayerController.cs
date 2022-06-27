using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [HideInInspector]
    public Animator _playerAnimator;

    [SerializeField] private float _movementSpeed = 4;
    [SerializeField] private float _rotationSpeed = 0.4f;
    [SerializeField] private float _rotation;
    [SerializeField] private float _jumpPower,_jumpDuration,_fallDuration;

    public int whichPlayer = 0;

    private Rigidbody _playerRigidbody;
    private Rigidbody _ballRb;

    private DynamicJoystick _joyStick;

    private GameObject _ball;
    private GameObject _basketPos, _ballFallPos,_leftBasketPos,_leftBasketFall,_rightBasketPos,_rightBasketFall;

    private Transform _chest;

    public bool isBallOnHand;
    public bool forwOrBack;
    public bool stopAI;
    public bool closestPlayer;
    public bool passBall;

    private Quaternion _q;
    private Vector3 _v;

    private Sequence _seq;

   


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
        _ballFallPos = GameObject.FindGameObjectWithTag("BallFallPos");
        _leftBasketPos = GameObject.FindGameObjectWithTag("LeftColliderBasket");
        _leftBasketFall = GameObject.FindGameObjectWithTag("LeftColliderFall");
        _rightBasketPos = GameObject.FindGameObjectWithTag("RightColliderBasket");
        _rightBasketFall = GameObject.FindGameObjectWithTag("RightColliderFall");
        _ballRb.useGravity = false;
        _ballRb.isKinematic = true;
    }

    [System.Obsolete]
    void Update()
    {

        _rotation += 500 * Time.deltaTime;
        _q = Quaternion.Euler(0, 0, _rotation);
        _v = _q.ToEulerAngles();
        if (_rotation >= 360)
        {
            _rotation = 0;
        }


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
        float distance = Vector3.Distance(gameObject.transform.position, _ball.transform.position);
        if (distance <= 2f)
        {
            _ball.transform.position = _chest.transform.position;
            // _ball.gameObject.transform.SetParent(transform);
            isBallOnHand = true;
            _ballRb.useGravity = false;
            gameObject.tag = "MainPlayer";
            _ball.GetComponent<SphereCollider>().enabled = false;
            passBall = false;
        }
        else
        {
            gameObject.tag = "Player";
        }


    }

    void Raycasting()
    {
        //RaycastHit hit;
        //RaycastHit hit2;
        //RaycastHit hit3;
        RaycastHit hit4;

        //Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), (transform.forward + _v) * 5000, Color.blue);
        //Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), (transform.forward + -_v) * 5000, Color.red);
        Debug.DrawLine(_chest.transform.position - new Vector3(0, .3f, 0), transform.forward * 5000, Color.red);

        //if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), (transform.forward + _v) * 5000, out hit, Mathf.Infinity))
        //{
        //    if (hit.collider.tag == "Player" && !_joyStick.move && isBallOnHand && !closestPlayer)
        //    {
        //        StartCoroutine(ChangingPlayer(hit.transform.GetChild(1).transform.position, hit.transform.gameObject));
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

        //if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), transform.forward, out hit3, Mathf.Infinity))
        //{
        //    if (hit3.collider.tag == "OppSeat")
        //    {
        //        forwOrBack = false;
        //    }
        //    if (hit3.collider.tag == "AlleySeat")
        //    {
        //        forwOrBack = true;
        //    }
        //}

        if (Physics.Raycast(_chest.transform.position - new Vector3(0, .5f, 0), transform.forward, out hit4, Mathf.Infinity))
        {
            if (hit4.collider.tag == "LeftCollider" && !_joyStick.move)
            {
                ShootingBall(_leftBasketPos.transform,_leftBasketFall.transform);
            }
            if (hit4.collider.tag == "MiddleCollider" && !_joyStick.move)
            {
                ShootingBall(_basketPos.transform,_ballFallPos.transform);
            }
            if (hit4.collider.tag == "RightCollider" && !_joyStick.move)
            {
                ShootingBall(_rightBasketPos.transform, _rightBasketFall.transform);
            }
        }
    }
    void ShootingBall(Transform desiredPos,Transform fallPos)
    {
        if (transform.position.z>=0)
        {
            if (isBallOnHand && !_joyStick.move)
            {
                _seq = DOTween.Sequence();
                _seq.Append(_ball.transform.DOJump(desiredPos.position, _jumpPower, 1, _jumpDuration));
                _seq.Append(_ball.transform.DOMove(fallPos.position, _fallDuration));
                _ballRb.useGravity = true;
                _ballRb.isKinematic = false;
                _ball.GetComponent<SphereCollider>().enabled = true;
                _ball.gameObject.transform.SetParent(null);
                isBallOnHand = false;
            }
        }
        
    }
    IEnumerator ChangingPlayer(Vector3 passVector, GameObject AI)
    {
        if (isBallOnHand)
        {
            //WhichPlayerNum(AI.transform.gameObject);
            _ball.GetComponent<SphereCollider>().enabled = true;
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
            _playerAnimator.SetBool("Move", false);
            AI.GetComponent<PlayerController>().enabled = true;
            AI.GetComponent<AIController>().enabled = false;
            GetComponent<AIController>().enabled = true;
            this.enabled = false;
        }
    }

    //public void WhichPlayerNum(GameObject g)
    //{
    //    switch (g.name)
    //    {
    //        case "PlayerHolder":
    //            whichPlayer = 0;
    //            break;
    //        case "AI1":
    //            whichPlayer = 1;
    //            break;
    //        case "AI2":
    //            whichPlayer = 2;
    //            break;
    //        case "AI3":
    //            whichPlayer = 3;
    //            break;
    //        case "AI4":
    //            whichPlayer = 4;
    //            break;
    //    }
    //}
}
