using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MouseDrawer : MonoBehaviour
{
    public event Action OnPointsGenerated = null;

    [SerializeField] private RectTransform m_RectTransform = null;
    [SerializeField] private float m_DistanceThreshold = 10;

    private bool m_IsDrawing = false;

    private List<Vector2> m_Points = new List<Vector2>();

    public List<Vector2> Points => m_Points;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_Points.Clear();

            m_IsDrawing = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_IsDrawing = false;

            if (m_Points.Count >= 2)
            {
                OnPointsGenerated?.Invoke();
            }
        }

        if (m_IsDrawing)
        {
            HandleDrawing(Input.mousePosition);
        }
    }

    //public static Vector3 ScreenToWorld(Vector3 screenPoint)
    //{
    //    Plane xy = new Plane(Vector3.forward, Vector3.zero);
    //    Ray ray = Camera.main.ScreenPointToRay(screenPoint);
    //    xy.Raycast(ray, out float enter);
    //    Vector3 point = ray.GetPoint(enter);
    //    return point;
    //}

    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying) return;

    //    Gizmos.color = Color.red;

    //    for (int i = 0; i < m_Points.Count - 1; i++)
    //    {
    //        Vector3 a = ScreenToWorld(m_Points[i]);
    //        Vector3 b = ScreenToWorld(m_Points[i + 1]);
    //        Gizmos.DrawLine(a, b);
    //    }
    //}

    private Vector2 GetPosition01(Vector3 screenPosition)
    {
        //Vector2 position = m_RectTransform.position;
        //Vector2 size = m_RectTransform.sizeDelta * FindObjectOfType<Canvas>().scaleFactor;
        //Vector2 horizontal = new Vector2(position.x - size.x / 2, position.x + size.x / 2);
        //Vector2 vertical = new Vector2(position.y - size.y / 2, position.y + size.y / 2);

        Vector3[] corners = new Vector3[4];
        m_RectTransform.GetWorldCorners(corners);

        Vector2 horizontal = new Vector2(corners[0].x, corners[3].x);
        Vector2 vertical = new Vector2(corners[0].y, corners[1].y);

        Vector2 position01 = new Vector2(
            Mathf.InverseLerp(horizontal.x, horizontal.y, screenPosition.x),
            Mathf.InverseLerp(vertical.x, vertical.y, screenPosition.y)
        );

        return position01;
    }

    private void HandleDrawing(Vector3 screenPosition)
    {
        bool inInside = RectTransformUtility.RectangleContainsScreenPoint(m_RectTransform, screenPosition);

        if (!inInside)
        {
            return;
        }

        Vector3 pos = GetPosition01(screenPosition);

        if (m_Points.Count == 0)
        {
            m_Points.Add(pos);
        }
        else
        {
            Vector3 last = m_Points[m_Points.Count - 1];
            if (Vector3.Distance(pos, last) > m_DistanceThreshold * 0.01f)
            {
                m_Points.Add(pos);
            }
        }
    }
}
