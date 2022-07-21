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
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private GameObject _inGamePanel;

    [SerializeField] List<AIController> _ai;

    [SerializeField] private TextMeshProUGUI _teamScore;
    [SerializeField] private TextMeshProUGUI _enemyTeamScore;
    [SerializeField] private TextMeshProUGUI _timer;


    [SerializeField] private Image _fill;


    private bool load;
    private float _time = 25;    



    private void Awake()
    {
       Instance = this;
        _timer.text = _time.ToString("F2");
       // Load(0);
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
        _loadScreen.SetActive(true);
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
            _loadScreen.SetActive(false);
    }

    void Timer()
    {
        _time -= Time.deltaTime;
        _timer.text = _time.ToString("F2");
    }


    public void Loading()
    {
        _loadScreen.SetActive(false);
        _inGamePanel.SetActive(true);
       
    }
}
