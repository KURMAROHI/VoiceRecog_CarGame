using System;
using System.Net.Http.Headers;
using UnityEngine;

public class GameUIControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameUIControl InStance;
    [SerializeField] private GameObject _startGame;
    [SerializeField] private GameObject _winGame;
    [SerializeField] private GameObject _loseGame;
    [SerializeField] private Transform _canvas;

    [SerializeField] private float actualAspect;
    [SerializeField] private RectTransform _track, _start, _finish, _content;


    private void Awake()
    {
        if (InStance == null)
        {
            InStance = this;
        }

    }

    private void Start()
    {
        // actualAspect = ((float)1920/(float)1080);
        Rect sizeOfRect = _canvas.GetComponent<RectTransform>().rect;
        float CurrentAspect = sizeOfRect.width / sizeOfRect.height;

        if (CurrentAspect < actualAspect)
        {
            Debug.LogError("==>rect|" + _canvas.GetComponent<RectTransform>().rect + "::" + CurrentAspect);
            _content.sizeDelta = new Vector2(_content.rect.width, _content.rect.height * CurrentAspect);
            _track.sizeDelta = new Vector2(_track.rect.width, _track.rect.height * CurrentAspect);
            _start.sizeDelta = new Vector2(_start.rect.width, _start.rect.height * CurrentAspect);
            _finish.sizeDelta = new Vector2(_finish.rect.width, _finish.rect.height * CurrentAspect);
        }
        else if (CurrentAspect > actualAspect)
        {
            _content.sizeDelta = new Vector2(_content.rect.width, _content.rect.height * (actualAspect / CurrentAspect));
            _track.sizeDelta = new Vector2(_track.rect.width, _track.rect.height * (actualAspect / CurrentAspect));
            _start.sizeDelta = new Vector2(_start.rect.width, _start.rect.height * (actualAspect / CurrentAspect));
            _finish.sizeDelta = new Vector2(_finish.rect.width, _finish.rect.height * (actualAspect / CurrentAspect));
        }
    }
    private void OnEnable()
    {
        SpeechManager.InStance.OnStartOfGame += GameUIControl_SpwanStartGameObject;
        PlayerControl.OnWinGame += GameUIControl_OnWinGame;
        PlayerControl.OnLoseGame += GameUIControl_OnLoseGame;
    }
    private void OnDisable()
    {
        SpeechManager.InStance.OnStartOfGame -= GameUIControl_SpwanStartGameObject;
        PlayerControl.OnWinGame -= GameUIControl_OnWinGame;
        PlayerControl.OnLoseGame -= GameUIControl_OnLoseGame;
    }

    private void GameUIControl_OnLoseGame(object sender, EventArgs e)
    {
        Debug.Log("==>Spwaning Object");
        Instantiate(_loseGame, _canvas);
    }

    private void GameUIControl_OnWinGame(object sender, EventArgs e)
    {
        Debug.Log("==>Spwaning Object");
        Instantiate(_winGame, _canvas);
    }

    private void GameUIControl_SpwanStartGameObject(object sender, EventArgs e)
    {
        Debug.Log("==>Spwaning Object");
        Instantiate(_startGame, _canvas);
    }






}
