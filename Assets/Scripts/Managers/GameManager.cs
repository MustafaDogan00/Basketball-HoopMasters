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

    [SerializeField] private TextMeshProUGUI _teamScore;
    [SerializeField] private TextMeshProUGUI _enemyTeamScore;
    [SerializeField] private TextMeshProUGUI _timer;


    [SerializeField] private Image _fill;
    [SerializeField] private Image _image;

    private float _time = 25;    



    private void Awake()
    {
       Instance = this;
        _timer.text = _time.ToString("F2");
    }

    private void Update()
    {
        _teamScore.text = Ball.Instance.teamScore.ToString();
        Timer();
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

    void Timer()
    {
        _time -= Time.deltaTime;
        _timer.text = _time.ToString("F2");
    }
}
