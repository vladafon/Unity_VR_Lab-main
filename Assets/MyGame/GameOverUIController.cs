using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
        EventManager.onGameOver += () => SetObjectStatus(true);
        EventManager.onRestart += () => SetObjectStatus(false);
    }

    private void SetObjectStatus(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
