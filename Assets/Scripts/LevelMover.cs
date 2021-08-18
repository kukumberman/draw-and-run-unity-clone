using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMover : MonoBehaviour
{
    [SerializeField] private float m_Speed = 2;

    private bool m_IsActive = false;

    private DrawAndRun m_Game = null;

    private void Awake()
    {
        m_Game = FindObjectOfType<DrawAndRun>();
    }

    private void OnEnable()
    {
        m_Game.OnGameStarted += OnGameStarted;
        m_Game.OnGameFinished += OnGameFinished;
    }

    private void OnDisable()
    {
        m_Game.OnGameStarted -= OnGameStarted;
        m_Game.OnGameFinished -= OnGameFinished;
    }

    private void OnGameStarted()
    {
        m_IsActive = true;
    }

    private void OnGameFinished(bool success)
    {
        m_IsActive = false;
    }

    private void Update()
    {
        if (m_IsActive)
        {
            transform.Translate(Vector3.back * m_Speed * Time.deltaTime);
        }
    }
}
