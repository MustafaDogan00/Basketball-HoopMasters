using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject[] _players;

    [SerializeField] List<AIController> _ai;

    [SerializeField] private TextMeshPro _teamScore;
    [SerializeField] private TextMeshPro _enemyTeamScore;


    [SerializeField] private Image _fill;
    [SerializeField] private Image _image;

    private void Awake()
    {
       Instance = this;
    }


    public void MainPlayer(Transform targetPlayer)
    {     
        foreach (var item in _ai)
        {
            item.mainPlayer=targetPlayer ;
        }
    }

    public void Load(int level)
    {
        StartCoroutine(StartLoading(level));    
        _image.enabled = true;
    }
    IEnumerator StartLoading(int level)
    {
        AsyncOperation async =SceneManager.LoadSceneAsync(level);
        while(!async.isDone)
        {
            _fill.fillAmount = async.progress;
            yield return null;  
        }
        if(async.isDone)
            _image.enabled = false;
       
    }

}
