using System;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public static EventHandler OnStartGameClicked;
    public static EventHandler OnWinorLoseButtonClicked;

    public void OnStartButtonClicked()
    {
        Debug.Log("==>OnStartButtonClicked");
        OnStartGameClicked?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject, 0.2f);
    }

    public void OnWinOkButtonClicked()
    {
        Debug.Log("==>OnStartButtonClicked");
        OnWinorLoseButtonClicked?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject, 0.2f);
    }
}
