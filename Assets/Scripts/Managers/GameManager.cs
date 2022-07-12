using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject[] _players;

    [SerializeField] List<AIController> _controllers;
    private void Awake()
    {
       Instance = this;
    }


    public void MainPlayer(Transform targetPlayer)
    {     
        foreach (var item in _controllers)
        {
            item.mainPlayer = targetPlayer;
        }
    }

}
