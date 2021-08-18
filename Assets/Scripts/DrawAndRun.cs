using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrawAndRun : MonoBehaviour
{
    public event Action OnGameStarted = null;

    public event Action<bool> OnGameFinished = null;

    private bool m_IsFinished = false;

    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    public void FinishGame(bool success)
    {
        if (!m_IsFinished)
        {
            m_IsFinished = true;

            OnGameFinished?.Invoke(success);
        }
    }
}
