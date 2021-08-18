using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPlacement))]
public class DudeController : MonoBehaviour
{
    [SerializeField] private int m_Count = 10;

    public int Count => m_Count;

    private MouseDrawer m_Drawer = null;
    private DudeSpawner m_DudeSpawner = null;
    private DrawAndRun m_Game = null;

    private void Awake()
    {
        m_Drawer = FindObjectOfType<MouseDrawer>();
        m_DudeSpawner = FindObjectOfType<DudeSpawner>();
        m_Game = FindObjectOfType<DrawAndRun>();
    }

    private void OnEnable()
    {
        m_Drawer.OnPointsGenerated += RecalculatePositions;
        m_DudeSpawner.OnDudeSpawned += OnDudeSpawned;
    }

    private void OnDisable()
    {
        m_Drawer.OnPointsGenerated -= RecalculatePositions;
        m_DudeSpawner.OnDudeSpawned -= OnDudeSpawned;
    }

    private void Start()
    {
        SpawnInitial();
    }

    private void SpawnInitial()
    {
        GridPlacement grid = GetComponent<GridPlacement>();

        Vector3[] positions = grid.Calculate();

        for (int i = 0; i < positions.Length; i++)
        {
            m_DudeSpawner.SpawnAt(positions[i]);
        }
        
    }

    private void OnDudeSpawned(DudeSpawner.OnDudeSpawnedEventArgs args)
    {
        m_Count += 1;

        args.Dude.OnSpikeCollision += OnDudeSpikeCollision;
    }

    private void OnDudeSpikeCollision(DudeObject.OnSpikeCollisionEventArgs args)
    {
        args.Sender.OnSpikeCollision -= OnDudeSpikeCollision;

        m_Count -= 1;

        if (m_Count == 0)
        {
            m_Game.FinishGame(false);
        }
    }

    private void RecalculatePositions()
    {
        m_DudeSpawner.RecalculatePosition(m_Count);
    }
}
