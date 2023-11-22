using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 싱글톤

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }

            return instance;
        }
    }
    
    #endregion

    [SerializeField] private CameraShake cameraShake;

    #region UI 관련 변수

    [Space(10)]
    [Header("UI 관련 변수")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform speedUpTextRect;
    [SerializeField] private RectTransform magnetUpTextRect;
    [SerializeField] private TextMeshProUGUI lifeText;
    
    [Space(10)]
    [Header("게임 오버 화면 UI")]
    [SerializeField] private Image gameOverWindow;
    [SerializeField] private TextMeshProUGUI gameOverText;
    
    #endregion

    #region 게임 관련 변수

    [Space(10)]
    [Header("게임 관련 변수")]
    public float mapMoveSpeed;
    public float scoreMultiply;
    public GameObject curPlayer;
    
    #endregion

    [HideInInspector] public int breakingWalls = 0;
    [HideInInspector] public int crystalCount = 0;

    [HideInInspector] public bool isStop;
    
    private float curScore;
    private float curTime = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverWindow.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        
        scoreText.text = Mathf.FloorToInt(curScore).ToString();
    }

    private void Update()
    {
        if (isStop)
        {
            return;
        }
        
        curTime += Time.deltaTime;
        
        //25초 마다 게임 스피드 업
        if (Mathf.FloorToInt(curTime) >= 25)
        {
            SpeedUp();

            curTime = 0f;
        }

        ScoreUpdate();
    }

    public void CameraShake(float duration, float magnitube)
    {
        cameraShake.ShakeStart(duration, magnitube);
    }

    public void GetScore(float score)
    {
        StartCoroutine(ScoreCount(curScore + score, curScore));

        curScore += score;
    }

    private IEnumerator ScoreCount(float target, float current)
    {
        float duration = 0.5f;
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            scoreText.text = ((int)current).ToString();
            yield return null;
        }

        current = target;
        scoreText.text = Mathf.FloorToInt(current).ToString();
    }

    private void ScoreUpdate()
    {
        curScore += Time.deltaTime * scoreMultiply;
        
        scoreText.text = Mathf.FloorToInt(curScore).ToString();
    }
    
    private void SpeedUp()
    {
        if (mapMoveSpeed >= 15)
        {
            return;
        }
        
        mapMoveSpeed += 1f;

        speedUpTextRect.anchoredPosition = new Vector2(0, -800);

        speedUpTextRect.DOAnchorPosY(320, 0.3f).SetEase(Ease.OutBack);
        speedUpTextRect.DOAnchorPosY(800, 0.2f).SetDelay(0.7f).SetEase(Ease.InBack);
    }

    public void MagnetUpTextAnimation()
    {
        magnetUpTextRect.anchoredPosition = new Vector2(1400, 200);
        
        magnetUpTextRect.DOAnchorPosX(0, 0.3f).SetEase(Ease.OutBack);
        magnetUpTextRect.DOAnchorPosX(-1400, 0.2f).SetDelay(0.7f).SetEase(Ease.InBack);
    }

    public void SetLifeText(int life)
    {
        lifeText.text = "X " + life.ToString();
    }

    public void GameOverEvent()
    {
        StartCoroutine(GameOverRoutine());
    }
    
    private IEnumerator GameOverRoutine()
    {
        gameOverWindow.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        isStop = true;
        mapMoveSpeed = 0f;
        
        StartCoroutine(FadeInObject(gameOverText, 1));
        yield return FadeInObject(gameOverWindow, 1);
    }

    private IEnumerator FadeInObject(Image _image, float time)
    {
        if (time == 0)
        {
            yield return null;
        }
        
        float targetAlpha = _image.color.a;
        float curAlpha = 0;
        float temp = 0;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);
        
        while (temp <= time)
        {
            curAlpha += Time.deltaTime * targetAlpha / time;
            
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);
            
            temp += Time.deltaTime;
            
            yield return null;
        }
    }
    
    private IEnumerator FadeInObject(TextMeshProUGUI _text, float time)
    {
        if (time == 0)
        {
            yield return null;
        }
        
        float targetAlpha = _text.color.a;
        float curAlpha = 0;
        float temp = 0;

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, curAlpha);
        
        while (temp <= time)
        {
            curAlpha += Time.deltaTime * targetAlpha / time;
            
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, curAlpha);
            
            temp += Time.deltaTime;
            
            yield return null;
        }
    }
}