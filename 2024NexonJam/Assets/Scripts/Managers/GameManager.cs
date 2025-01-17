using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum PlayerType
{
    None,
    Player1,
    Player2
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    public GameObject eventManager;
    public List<GameObject> GimicEventImg;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2.5f);
    
    public TextMeshProUGUI _player1Score;
    public TextMeshProUGUI _player2Score;
    
    public GameObject resultPanelP1;
    public GameObject resultPanelP2;
    public int winPlayerScore;
    
    public TextMeshProUGUI timerText;
    public Image timeGauge;
    private float elapsedTime;
    private int previousSecond;
    private bool _isGameEnded;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance= this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

    }
    private void Start()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundType.게임씬Bgm);
        elapsedTime = 0f;
        previousSecond = 0;
        UpdateTimerText();
    }

    private void Update()
    {
        if (_isGameEnded)
        {
            return;
        }
        
        elapsedTime += Time.deltaTime;
        int currentSecond = (int)elapsedTime;

        if (currentSecond != previousSecond)
        {
            previousSecond = currentSecond;
            CheckMilestones(currentSecond);
            UpdateTimerText();
        }
    }

    private void CheckMilestones(int seconds)
    {   //플레이 타임 총 160초
        if (seconds == 40)
        {
            // 40,80,120 초 도달
            RandomGimicEvent();
            StartCoroutine(DisplayGimicEventImg(1));
        }
        else if (seconds == 60)
        {
            eventManager.GetComponent<EventManager>().HelicopterEvent(1);
            StartCoroutine(DisplayGimicEventImg(2));
        }
        else if (seconds == 80)
        {
            RandomGimicEvent();
            StartCoroutine(DisplayGimicEventImg(3));
        }
        else if (seconds == 100)
        {
            eventManager.GetComponent<EventManager>().HelicopterEvent(2);
            StartCoroutine(DisplayGimicEventImg(4));
        }
        else if (seconds == 120)
        {
            RandomGimicEvent();
            StartCoroutine(DisplayGimicEventImg(5));
        }
        else if (seconds > 159)
        {   //90초 넘으면 게이지 초기화
            //게임 종료
            EndGame();
        }
    }
    private void EndGame()
    {
        _isGameEnded = true;
        elapsedTime = 0f;
        previousSecond = 0;

        int player1Score = int.Parse(_player1Score.text);
        int player2Score = int.Parse(_player2Score.text);

        if (player1Score > player2Score)
        {
            resultPanelP1.SetActive(true);
            winPlayerScore = player1Score;
        }
        else if(player1Score < player2Score)
        {
            resultPanelP2.SetActive(true);
            winPlayerScore = player2Score;
        }
        else
        {
            SceneConManager.Instance.MoveScene("MenuScene");
            Destroy(gameObject);
        }
    }
    private void UpdateTimerText()
    {
        int displaySeconds = (int)elapsedTime;
        timerText.text = displaySeconds.ToString();
        timeGauge.fillAmount = elapsedTime / 160f;
    }

    public void AddScore(PlayerType playerType,int val)
    {
        if (playerType == PlayerType.Player1)
        {
            
            if (SkillManager.Instance.IsDoubleScoreActive(0))
            {
                val *= 2;
            }
            
            int score = int.Parse(_player1Score.text);
            score += val;
            _player1Score.text = score.ToString();
        }
        else if (playerType == PlayerType.Player2)
        {
            
            if (SkillManager.Instance.IsDoubleScoreActive(1))
            {
                val *= 2;
            }
            
            int score = int.Parse(_player2Score.text);
            score += val;
            _player2Score.text = score.ToString();
        }
    }

    
    public void InitGameManager()
    {
        Destroy(this);
    }

    public IEnumerator DisplayGimicEventImg(int num)
    {   //기믹 이벤트 HUD 이미지 표시 함수
        if (num >= 1 && num <= GimicEventImg.Count)
        {
            GimicEventImg[num - 1].SetActive(true);
            yield return _waitForSeconds;
            GimicEventImg[num - 1].SetActive(false); // 비활성화
        }
    }
 
    public void RandomGimicEvent()
    {   //랜덤으로 기믹 이벤트 발생 함수
        // 40,80,120 초 도달
        int randomIdx = Random.Range(0, 2);

        if (randomIdx == 1)   //렌덤으로 호출
        {
            eventManager.GetComponent<EventManager>().CrabEvent();
        }
        else  //2일때
        {
            eventManager.GetComponent<EventManager>().WaveEvent();
        }
    }
}
