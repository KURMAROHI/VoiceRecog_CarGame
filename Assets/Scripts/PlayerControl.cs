using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static EventHandler OnWinGame;
    public static EventHandler OnLoseGame;
    [SerializeField] private float TotalDuration = 60f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.LogError("==>OnTriggerEnter2D Enter|" + col.gameObject.name);
        SpeechManager.InStance.IsstatredListening = false;
        Invoke("OnWin", 0.4f);
    }

    private void OnWin()
    {
        OnWinGame?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (SpeechManager.InStance.stratTime > TotalDuration)
        {
            SpeechManager.InStance.stratTime = 0f;
            SpeechManager.InStance.IsstatredListening = false;
            OnLoseGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
