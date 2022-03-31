using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action onShooted;

    public static event Action onGameOver;

    public static event Action onRestart;

    public static void RaiseOnShooted()
    {
        onShooted?.Invoke();
    }

    public static void RaiseGameOver()
    {
        onGameOver?.Invoke();
    }

    public static void RaiseRestart()
    {
        onRestart?.Invoke();
    }
}
