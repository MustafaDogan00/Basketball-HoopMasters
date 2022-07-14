using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class OppsTeam : MonoBehaviour
{
    private GameObject _ball;
    [SerializeField] private GameObject[] _team;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed = .08f;

    List<GameObject> directions = new List<GameObject>();

    [SerializeField] private Transform _tr;

    private bool isBollOnAir;

    [SerializeField] private Ball _ballS;

    private DynamicJoystick _joyStick;

    private Animator _animator;
    void Awake()
    {
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        for (int i = 0; i < _team.Length; i++)
        {
            directions.Add(_team[i]);
        }
    }


    void Update()
    {
        if (_ballS.defense)
        Defense();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Basketball")
        {
            Attack();
        }
    }


    void Defense()
    {
       
        directions = directions.OrderBy(t => Vector3.Distance(t.transform.position, _ball.transform.position)).ToList();
        ClosestPlayer(directions[0].transform, _ball.transform,-1,2);
        ClosestPlayer(directions[1].transform, _ball.transform,2,3);
        if (directions[0]!=gameObject && directions[1] != gameObject)
        {
            _speed = 2.5f;
            Direction(_tr);
            transform.position = Vector3.MoveTowards(transform.position, _tr.position, _speed * Time.deltaTime);
            if (gameObject.transform.position==_tr.transform.position)
            {
                _animator.SetBool("Move", false);
                Direction(_ball.transform);
            }
        }     
    }

    void ClosestPlayer(Transform dir, Transform dir1,float x,float z)
    {
        Direction(_ball.transform);
        _animator.SetBool("Move", true);
        Vector3 offset = new Vector3(x, 0, z);      
        _speed = .8f;
        dir.position=Vector3.MoveTowards(dir.position,new Vector3(dir1.position.x,0, dir1.position.z)+offset,_speed*Time.deltaTime);
        if (Vector3.Distance(dir.position, dir1.position) <=2.5 && !_joyStick.move && dir.gameObject==gameObject)
        {
            _animator.SetBool("Move", false);
        }
    }

    void Direction(Transform dir)
    {
        Vector3 direction = dir.position - transform.position;
        direction.Normalize();
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);           
        }
    }

    void Attack()
    {

    }
}
