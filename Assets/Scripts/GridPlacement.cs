using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    [SerializeField] private Vector2Int m_GridSize = Vector2Int.one;
    [SerializeField] private float m_ChunkSize = 1;
    [SerializeField] private float m_Spacing = 1;
    //[SerializeField] private Vector3 m_Normal = Vector3.forward;

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
                Vector3 pos = GetPosition(x, y);
                (pos.y, pos.z) = (pos.z, pos.y);

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

    private void OnDrawGizmos()
    {
        Vector3 size = Vector3.one * m_ChunkSize;
        size.y = 0;

        for (int y = 0; y < m_GridSize.y; y++)
        {
            for (int x = 0; x < m_GridSize.x; x++)
            {
                Vector3 pos = GetPosition(x, y);
                //pos = Vector3.ProjectOnPlane(pos, m_Normal);
                (pos.y, pos.z) = (pos.z, pos.y);
                pos += transform.position;
                Gizmos.DrawWireCube(pos, size);
            }
        }
    }
}
