using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MouseDrawer : MonoBehaviour
{
    public class OnPointsGeneratedEventArgs
    {
        public List<Vector2> Points { get; }

        public OnPointsGeneratedEventArgs(List<Vector2> points)
        {
            Points = points;
        }
    }
    public event Action<OnPointsGeneratedEventArgs> OnPointsGenerated = null;

    [SerializeField] private RectTransform m_RectTransform = null;
    [SerializeField] private bool m_IsInside = false;

    private bool m_IsDrawing = false;

    private List<Vector2> m_Points = new List<Vector2>();

    private float m_DistanceThreshold = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_IsDrawing = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_IsDrawing = false;

            OnPointsGenerated?.Invoke(new OnPointsGeneratedEventArgs(m_Points));

            m_Points.Clear();
        }

        if (m_IsDrawing)
        {
            HandleDrawing(Input.mousePosition);
        }
    }

    public static Vector3 ScreenToWorld(Vector3 screenPoint)
    {
        Plane xy = new Plane(Vector3.forward, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        xy.Raycast(ray, out float enter);
        Vector3 point = ray.GetPoint(enter);
        return point;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < m_Points.Count - 1; i++)
        {
            Vector3 a = ScreenToWorld(m_Points[i]);
            Vector3 b = ScreenToWorld(m_Points[i + 1]);
            Gizmos.DrawLine(a, b);
        }
    }

    private void HandleDrawing(Vector3 screenPosition)
    {
        m_IsInside = RectTransformUtility.RectangleContainsScreenPoint(m_RectTransform, screenPosition);

        if (!m_IsInside)
        {
            return;
        }

        if (m_Points.Count == 0)
        {
            m_Points.Add(screenPosition);
        }
        else
        {
            Vector3 last = m_Points[m_Points.Count - 1];
            if (Vector3.Distance(screenPosition, last) > m_DistanceThreshold)
            {
                m_Points.Add(screenPosition);
            }
        }
    }
}
