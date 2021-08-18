using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrajectoryPath
{
    public ICollection<Vector3> Points { get; }

    private List<Point> segments = new List<Point>();

    public TrajectoryPath(ICollection<Vector3> points)
    {
        Points = points;

        // error
        //if (points.Count == 0) return Vector3.zero;

        float d = GetLength(points.ToArray());

        float temp = 0;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 a = Points.ElementAt(i);
            Vector3 b = Points.ElementAt(i + 1);
            float percentage01 = temp / d;

            Point p = new Point(a, percentage01, temp);
            segments.Add(p);

            temp += (b - a).magnitude;
        }

        // last point
        segments.Add(new Point(Points.ElementAt(Points.Count - 1), 1, d));
    }

    public class Point
    {
        public Vector3 Position { get; }
        public float Percentage01 { get; }
        public float Length { get; }

        public Point(Vector3 position, float percentage01, float length)
        {
            Position = position;
            Percentage01 = percentage01;
            Length = length;
        }
    }

    private float GetLength(ICollection<Vector3> points)
    {
        float d = 0;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 a = points.ElementAt(i);
            Vector3 b = points.ElementAt(i + 1);
            d += (b - a).magnitude;
        }

        return d;
    }

    public ICollection<Vector3> Subdivide(int count)
    {
        Vector3[] result = new Vector3[count];

        float stepSize = 1f / (count - 1);

        for (int i = 0; i < count; i++)
        {
            float t = i * stepSize;
            Vector3 evaluated = Evaluate(t);
            result[i] = evaluated;
        }

        return result;
    }

    public Vector3 Evaluate(float time01)
    {
        time01 = Mathf.Clamp01(time01);

        int prevIndex = 0, nextIndex = 0;

        for (int i = 0; i < segments.Count - 1; i++)
        {
            Point next = segments[i + 1];

            if (time01 <= next.Percentage01)
            {
                prevIndex = i;
                nextIndex = i + 1;

                break;
            }
        }

        Point p1 = segments[prevIndex];
        Point p2 = segments[nextIndex];

        float time = Mathf.InverseLerp(p1.Percentage01, p2.Percentage01, time01);
        //time = (t - a) / (b - a);

        Vector3 result = Vector3.Lerp(p1.Position, p2.Position, time);

        return result;
    }
}
