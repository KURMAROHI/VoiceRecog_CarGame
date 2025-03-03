using UnityEngine;

// using UnityEngine.Windows;
// using UnityEngine.Windows.Speech;

public class SpeechControl : MonoBehaviour
{
//     private KeywordRecognizer keywordRecognizer;
//     private string[] keywords = { "start", "stop" };
//     private bool isMoving = false;
//     private Vector3 velocity = new Vector3(5f, 0f, 0f);  // Example speed
//     private float decelerationRate = 0.1f;

//     // private AndroidJavaObject speechRecognizer;

//     void Start()
//     {
//         // if (Application.platform == RuntimePlatform.Android)
//         // {
//         //     InitializeSpeechRecognizer();
//         // }
//         keywordRecognizer = new KeywordRecognizer(keywords);
//         keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
//         keywordRecognizer.Start();
//     }

//     void Update()
//     {
//         if (isMoving)
//         {
//             // Move the object when "go" is recognized
//             transform.Translate(velocity * Time.deltaTime);
//         }
//         else
//         {
//             // Gradually slow down when "go" is not said
//             velocity = Vector3.Lerp(velocity, Vector3.zero, decelerationRate * Time.deltaTime);
//         }
//     }

//     private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
//     {
//         Debug.LogError("Text:" + args.text);
//         if (args.text == "start")
//         {
//             Debug.Log("==>start");
//             isMoving = true;
//             velocity = new Vector3(5f, 0f, 0f);
//         }
//         else if (args.text == "stop")
//         {
//             Debug.Log("==>stop");
//             isMoving = false;
//             velocity = Vector3.zero;  // Stop movement immediately
//         }
//     }

//     void OnDisable()
//     {
//         keywordRecognizer.Stop();
//     }
}
