using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class OppsTeam : MonoBehaviour
{
    private GameObject _ball;
   [SerializeField] private GameObject[] _team;

    [SerializeField] private float _speed;

    List<GameObject> directions = new List<GameObject>();

   [SerializeField] private Transform _tr;
    void Awake()
    {
        _ball = GameObject.FindGameObjectWithTag("Basketball");
        for (int i = 0; i < _team.Length; i++)
        {
            directions.Add(_team[i]);
        }     
    }

  
    void Update()
    {
        Defense();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Basketball")
        {

        }
    }

    //private Transform ClosestToBall()
    //{
    //    float closestDistance=Mathf.Infinity;
    //    float currentDistance;
    //    Transform closestPlayer = null;

    //    foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
    //    {
    //        currentDistance=Vector3.Distance(item.transform.position, _ball.transform.position);
    //        if (currentDistance<=closestDistance)
    //        {
    //           closestDistance=currentDistance;
    //            closestPlayer = item.transform;
    //        }
    //    }
    //    return closestPlayer;
    //}


    void Defense()
    {
        directions = directions.OrderBy(t => Vector3.Distance(t.transform.position,_ball.transform.position)).ToList();       
        WhereTo(directions[0].transform,_ball.transform);
    }

    void WhereTo(Transform dir,Transform dir1)
    {
        Vector3 distance = dir1.position-dir.position;
        dir.position+=distance.normalized*_speed*Time.deltaTime;
        if (Vector3.Distance(dir.position,dir1.position)<=2)
        {
            dir.position -= distance.normalized * _speed * Time.deltaTime;
        }
    }
}
