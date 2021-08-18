using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeController : MonoBehaviour
{
    [SerializeField] private int m_Count = 10;

    public int Count => m_Count;

    private MouseDrawer m_Drawer = null;
    private DudeSpawner m_DudeSpawner = null;

    private void Awake()
    {
        m_Drawer = FindObjectOfType<MouseDrawer>();
        m_DudeSpawner = FindObjectOfType<DudeSpawner>();
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

        //todo: recalculate position;
    }

    private void OnDudeSpikeCollision(DudeObject.OnSpikeCollisionEventArgs args)
    {
        args.Sender.OnSpikeCollision -= OnDudeSpikeCollision;

        m_Count -= 1;

        if (m_Count == 0)
        {
            Debug.Log("game finished - bad");
        }
        else
        {
            //RecalculatePositions();
        }
    }

    private void RecalculatePositions()
    {
        m_DudeSpawner.RecalculatePosition(m_Count);
    }
}
