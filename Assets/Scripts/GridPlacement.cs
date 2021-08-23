using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    [SerializeField] private Vector2Int m_GridSize = Vector2Int.one;
    [SerializeField] private float m_ChunkSize = 1;
    [SerializeField] private float m_Spacing = 1;
    [SerializeField] private Vector3 m_Normal = Vector3.up;
    [Space]
    [SerializeField] private bool m_DrawGizmo = true;
    [SerializeField] private bool m_UseUpAsNormal = false;

    public Vector3 TotalSize()
    {
        Vector3 size = Vector3.zero;
        size.x = m_GridSize.x * m_ChunkSize + (m_GridSize.x - 1) * m_Spacing;
        size.y = 1;
        size.z = m_GridSize.y * m_ChunkSize + (m_GridSize.y - 1) * m_Spacing;
        return size;
    }

    public Vector3[] Calculate()
    {
        Vector3[] positions = new Vector3[m_GridSize.x * m_GridSize.y];

        for (int y = 0; y < m_GridSize.y; y++)
        {
            for (int x = 0; x < m_GridSize.x; x++)
            {
                Vector3 pos = EvaluateNormal(GetPosition(x, y));

                int index = y * m_GridSize.x + x;
                positions[index] = pos;
            }
        }

        return positions;
    }

    private Vector3 GetPosition(int x, int y)
    {
        Vector2 xy = new Vector2(x, y);

        Vector2 centerOffset = Vector2.one * 0.5f;

        Vector2 gridOffset = new Vector2(-m_GridSize.x * 0.5f, -m_GridSize.y * 0.5f);

        Vector3 pos = gridOffset + xy + centerOffset;

        Vector3 position = pos * m_ChunkSize + pos * m_Spacing;

        return position;
    }

    private Vector3 EvaluateNormal(Vector3 xy)
    {
        return Quaternion.FromToRotation(Vector3.forward, m_Normal) * xy;
    }

    private Vector3[] GetCorners(Vector2 xy)
    {
        Vector3[] corners = new Vector3[4];

        Vector2 halfSize = Vector2.one * m_ChunkSize * 0.5f;

        corners[0] = EvaluateNormal(xy + new Vector2(-halfSize.x, -halfSize.y));
        corners[1] = EvaluateNormal(xy + new Vector2(-halfSize.x, halfSize.y));
        corners[2] = EvaluateNormal(xy + new Vector2(halfSize.x, halfSize.y));
        corners[3] = EvaluateNormal(xy + new Vector2(halfSize.x, -halfSize.y));

        return corners;
    }

    private void OnDrawGizmos()
    {
        if (!m_DrawGizmo) return;

        if (m_UseUpAsNormal)
        {
            m_Normal = transform.up;
        }

        Vector3 origin = transform.position;

        for (int y = 0; y < m_GridSize.y; y++)
        {
            for (int x = 0; x < m_GridSize.x; x++)
            {
                Vector2 xy = GetPosition(x, y);

                Vector3 pos = EvaluateNormal(xy);
                Gizmos.DrawWireSphere(pos + origin, 0.01f);

                var corners = GetCorners(xy);
                for (int i = 0; i < corners.Length; i++)
                {
                    Gizmos.DrawLine(corners[i] + origin, corners[(i + 1) % corners.Length] + origin);
                }
            }
        }
    }
}
