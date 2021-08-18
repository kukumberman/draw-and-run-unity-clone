using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class DudeSpawner : MonoBehaviour
{
    public class OnDudeSpawnedEventArgs
    {
        public DudeObject Dude { get; }

        public OnDudeSpawnedEventArgs(DudeObject dude)
        {
            Dude = dude;
        }
    }

    public event Action<OnDudeSpawnedEventArgs> OnDudeSpawned = null;

    [SerializeField] private DudeObject m_DudePrefab = null;
    [Space]
    [SerializeField] private Vector3 m_Center = Vector3.zero;
    [SerializeField] private Vector2 m_Size = Vector2.one;

    private MouseDrawer m_Drawer = null;

    private Vector3[] m_Points = new Vector3[0];

    private List<DudeObject> m_Dudes = new List<DudeObject>();

    private void Awake()
    {
        m_Drawer = FindObjectOfType<MouseDrawer>();
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

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(ConvertPoint(new Vector2(0, 0)), 0.1f);
        //Gizmos.DrawWireSphere(ConvertPoint(new Vector2(0, 1)), 0.1f);
        //Gizmos.DrawWireSphere(ConvertPoint(new Vector2(1, 1)), 0.1f);
        //Gizmos.DrawWireSphere(ConvertPoint(new Vector2(1, 0)), 0.1f);

        //Gizmos.color = Color.black;
        //Vector3[] corners = new Vector3[4];
        //GetCorners(corners);
        //for (int i = 0; i < corners.Length; i++)
        //{
        //    Gizmos.DrawWireSphere(corners[i], 0.2f);
        //}
    }

    public void RecalculatePosition(int count)
    {
        List<Vector3> points = m_Drawer.Points
            .Select(v2 => ConvertPoint(v2))
            .ToList();

        m_Points = new TrajectoryPath(points).Subdivide(count).ToArray();

        for (int i = 0; i < m_Dudes.Count; i++)
        {
            DudeObject dude = m_Dudes[i];
            dude.transform.position = m_Points[i];
        }
    }

    public void SpawnAt(Vector3 position)
    {
        DudeObject dude = Instantiate(m_DudePrefab, position, Quaternion.identity, transform);

        dude.OnSpikeCollision += OnSpikeRemoveDude;

        m_Dudes.Add(dude);

        OnDudeSpawned?.Invoke(new OnDudeSpawnedEventArgs(dude));
    }

    private void OnSpikeRemoveDude(DudeObject.OnSpikeCollisionEventArgs args)
    {
        args.Sender.OnSpikeCollision -= OnSpikeRemoveDude;

        m_Dudes.Remove(args.Sender);

        Destroy(args.Sender.gameObject);
    }

    private void GetCorners(Vector3[] corners)
    {
        // bl, tl, tr, br;

        Vector2 halfSize = m_Size * 0.5f;

        corners[0] = m_Center + new Vector3(-halfSize.x, 0, -halfSize.y);
        corners[1] = m_Center + new Vector3(-halfSize.x, 0, halfSize.y);
        corners[2] = m_Center + new Vector3(halfSize.x, 0, halfSize.y);
        corners[3] = m_Center + new Vector3(halfSize.x, 0, -halfSize.y);
    }

    private Vector3 ConvertPoint(Vector2 point01)
    {
        Vector3[] corners = new Vector3[4];
        GetCorners(corners);

        Vector3 point = Vector3.zero;
        point.x = Mathf.Lerp(corners[0].x, corners[3].x, point01.x);
        point.z = Mathf.Lerp(corners[0].z, corners[1].z, point01.y);
        return point;
    }
}
