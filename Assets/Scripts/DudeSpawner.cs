using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DudeSpawner : MonoBehaviour
{
    [SerializeField] private int m_Count = 10;
    [SerializeField] private Vector3 m_Center = Vector3.zero;
    [SerializeField] private Vector2 m_Size = Vector2.one;

    private MouseDrawer m_Drawer = null;

    private Vector3[] m_Points = new Vector3[0];

    private void Awake()
    {
        m_Drawer = FindObjectOfType<MouseDrawer>();
    }

    private void OnEnable()
    {
        m_Drawer.OnPointsGenerated += OnPointsGenerated;
    }

    private void OnDisable()
    {
        m_Drawer.OnPointsGenerated -= OnPointsGenerated;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < m_Points.Length; i++)
        {
            Gizmos.DrawWireSphere(m_Points[i], 0.1f);
        }

        Gizmos.color = Color.green;

        Vector3 size = new Vector3(m_Size.x, 0, m_Size.y);
        Gizmos.DrawWireCube(m_Center, size);
    }

    private void OnPointsGenerated(MouseDrawer.OnPointsGeneratedEventArgs args)
    {
        List<Vector3> points = args.Points
            .Select(v2 => new Vector3(v2.x, v2.y))
            .Select(v3 => MouseDrawer.ScreenToWorld(v3))
            .ToList();

        TrajectoryPath path = new TrajectoryPath(points);
        m_Points = path.Subdivide(m_Count).ToArray();
    }
}
