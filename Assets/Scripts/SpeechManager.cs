using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Collections;
using System;

#if UNITY_EDITOR && !UNITY_ANDROID
using UnityEngine.Windows.Speech;
#endif
public class SpeechManager : MonoBehaviour
{
    public static SpeechManager InStance;
    public Text TimeLeftText;
    public Text DistText;
    private bool isScrolling = false;
    public EventHandler OnStartOfGame;
#if UNITY_EDITOR&& !UNITY_ANDROID
    private KeywordRecognizer keywordRecognizer;
#endif
    private string[] keywords = { "start", "go" };

    public bool IsstatredListening = false;

    private bool ISGameWin = false;

    [SerializeField] private GameObject HintObject;


    private void Awake()
    {
        if (InStance == null)
        {
            InStance = this;
        }

    }


    private void Start()
    {
        StartGame.OnStartGameClicked += StartListening;
        StartGame.OnWinorLoseButtonClicked += SpeechManager_OnWinButtonClicked;
        PlayerControl.OnWinGame += SpeechManager_OnWinGame;
        PlayerControl.OnLoseGame += SpeechManager_OnLoseGame;

        targetPosition = startPosition = scrollRect.horizontalNormalizedPosition;

#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Debug.Log("==>Requesting permission :");
            StartCoroutine(RequestPermissionAndStartListening());
            // Permission.RequestUserPermission(Permission.Microphone);
        }
        else
        {
            Debug.Log("==>permission given:");
            // StartListening();
            OnStartOfGame?.Invoke(this, EventArgs.Empty);
        }
#else
        Debug.Log("Calling Event:" + (OnStartOfGame == null));
        OnStartOfGame?.Invoke(this, EventArgs.Empty);
#endif
    }

    private void OnDisable()
    {
        PlayerControl.OnWinGame -= SpeechManager_OnWinGame;
        PlayerControl.OnLoseGame -= SpeechManager_OnLoseGame;
        StartGame.OnStartGameClicked -= StartListening;
        StartGame.OnWinorLoseButtonClicked -= SpeechManager_OnWinButtonClicked;


    }

    private void SpeechManager_OnWinButtonClicked(object sender, EventArgs e)
    {
        HintObject.SetActive(false);
        ISGameWin = false;
        scrollRect.horizontalNormalizedPosition = 0;
        IsstatredListening = true;
        TimeLeftText.text = "60";
        stratTime = 0;
        targetPosition = startPosition = scrollRect.horizontalNormalizedPosition;

    }

    private void SpeechManager_OnWinGame(object sender, EventArgs e)
    {
        HintObject.SetActive(false);
        currentSpeed = 0;
        ISGameWin = true;
        isScrolling = false;
    }

    private void SpeechManager_OnLoseGame(object sender, EventArgs e)
    {
        HintObject.SetActive(false);
        currentSpeed = 0;
        ISGameWin = true;
        isScrolling = false;
    }

    private void StartListening(object sender, EventArgs e)
    {
        Debug.Log("Called Start Listening");
        IsstatredListening = true;
        TimeLeftText.text = "60";
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject plugin = new AndroidJavaObject("com.example.speechrecognisation.SpeechRecognitionPlugin", activity))
                {
                    plugin.Call("StartListen");
                }
            }
        }
#elif UNITY_EDITOR && !UNITY_ANDROID
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
#endif
    }

    IEnumerator RequestPermissionAndStartListening()
    {
        Debug.Log("Enter into Enumerator:" + Permission.HasUserAuthorizedPermission(Permission.Microphone));
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);

            // Wait until the user grants permission
            while (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                yield return null; // Wait for the next frame
            }
        }

        // Call StartListening after permission is granted
        OnStartOfGame?.Invoke(this, EventArgs.Empty);
    }



    public void OnVoiceCommandReceived(string command)
    {
        command = command.ToLower();
        Debug.Log("Received Command: " + command);

        if (command.Contains("start") && !ISGameWin)
        {
            CompareText("start");
            // lastClickTime = Time.time;
            // if (!isScrolling)
            // {
            //     isScrolling = true;
            //     currentSpeed = 0f;  // Reset speed if it was stopped
            // }
        }


    }

    // private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    // {
    //     Debug.LogError("Text:" + args.text);

    //     // if (args.text == "start" && !ISGameWin)
    //     // {
    //     //     Debug.Log("==>start");
    //     //     lastClickTime = Time.time;

    //     //     if (!isScrolling)
    //     //     {
    //     //         isScrolling = true;
    //     //         currentSpeed = 0f;  // Reset speed if it was stopped
    //     //     }
    //     // }
    //     CompareText(args.text);
    // }


    private void CompareText(string text)
    {
        if (text == "start" && !ISGameWin)
        {
            Debug.Log("==>start");
            lastClickTime = Time.time;
            //  startPosition = scrollRect.horizontalNormalizedPosition;
            if (!isScrolling)
            {
                isScrolling = true;
                currentSpeed = 0f;  // Reset speed if it was stopped
            }



        }
    }



    private float startPosition;
    public float stratTime = 0f;
    public float maxDuration = 3f;
    public ScrollRect scrollRect;
    public float targetPosition;
    public float maxSpeed = 0.5f;  // Maximum scroll speedX
    public float acceleration = 0.2f;  // Acceleration rate
    public float deceleration = 0.05f; // Deceleration rate
    private float currentSpeed = 0f;
    private float lastClickTime = 0f;
    [SerializeField] private float TotalDuration = 60f;
    private void Update()
    {
        if (IsstatredListening)
        {
            TimeLeftText.text = (60f - stratTime).ToString();
            stratTime += Time.deltaTime;
        }

        if (stratTime > TotalDuration)
            return;


        if (isScrolling)
        {
            HintObject.SetActive(false);
            float elapsedTime = Time.time - lastClickTime;

            if (elapsedTime < maxDuration)
            {
                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
            }
            else
            {
                currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, 0);
                if (currentSpeed == 0)
                {
                    HintObject.SetActive(true);
                    isScrolling = false;
                    return;
                }
            }

            targetPosition += currentSpeed * Time.deltaTime;
            targetPosition = Mathf.Clamp(targetPosition, 0, 1); // Keep scroll within bounds
            scrollRect.horizontalNormalizedPosition = targetPosition;

            RotateWheels();

        }




    }

    public Transform WheelBack, WheelFront;
    public float wheelRotationSpeed = 500f;
    private void RotateWheels()
    {
        float rotationAmount = currentSpeed * wheelRotationSpeed * Time.deltaTime;
        WheelBack.Rotate(Vector3.back, rotationAmount);
        WheelFront.Rotate(Vector3.back, rotationAmount);

    }

}
