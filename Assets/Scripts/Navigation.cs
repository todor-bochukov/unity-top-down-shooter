using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    private Collider2D[] obstacles;

    private List<Vector2> nodes = new List<Vector2>();

    private float[,] distances;

    private delegate Vector2 SampleGetter(int i, int j);

    public static Navigation Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        obstacles = GetComponentsInChildren<Collider2D>();

        var startTime = Time.realtimeSinceStartup;

        //
        var bounds = new Bounds();
        float minSize = Mathf.Infinity;
        foreach (var obstacle in obstacles)
        {
            bounds.Encapsulate(obstacle.bounds);

            minSize = Mathf.Min(obstacle.bounds.size.x, obstacle.bounds.size.y, minSize);
        }

        var step = minSize / 2;

        var matrixSizeX = Mathf.CeilToInt((bounds.max.x - bounds.min.x) / step);
        var matrixSizeY = Mathf.CeilToInt((bounds.max.y - bounds.min.y) / step);

        var samples = new bool[matrixSizeX, matrixSizeY];

        SampleGetter getSample = (i, j) => new Vector2(bounds.min.x + i * step, bounds.min.y + j * step);

        for (var i = 0; i < matrixSizeX; ++i)
        {
            for (var j = 0; j < matrixSizeY; ++j)
            {
                samples[i, j] = IsReachable(getSample(i, j));
            }
        }

        // simplify
        var horizontal = samples.Clone() as bool[,];
        var vertical = samples.Clone() as bool[,];

        for (var i = 0; i < matrixSizeX; ++i)
        {
            for (var j = 0; j < matrixSizeY; ++j)
            {
                if (!samples[i, j])
                    continue;

                if (i > 0 && samples[i - 1, j] && i < matrixSizeX - 1 && samples[i + 1, j])
                {
                    horizontal[i, j] = false; // not significant on the horizontal
                }

                if (j > 0 && samples[i, j - 1] && j < matrixSizeY - 1 && samples[i, j + 1])
                {
                    vertical[i, j] = false; // not significant on the vertical
                }
            }
        }

        var significantSamples = new List<Vector2Int>();

        for (var i = 0; i < matrixSizeX; ++i)
        {
            for (var j = 0; j < matrixSizeY; ++j)
            {
                if (horizontal[i, j] && vertical[i, j])
                    continue; // inner corner, never on the shortest path

                if (horizontal[i, j] && !(j > 0 && horizontal[i, j - 1] && j < matrixSizeY - 1 && horizontal[i, j + 1]))
                {
                    significantSamples.Add(new Vector2Int(i, j));
                }

                if (vertical[i, j] && !(i > 0 && vertical[i - 1, j] && i < matrixSizeX - 1 && vertical[i + 1, j]))
                {
                    significantSamples.Add(new Vector2Int(i, j));
                }
            }
        }

        for (var i = 0; i < significantSamples.Count; ++i)
        {
            for (var j = i + 1; j < significantSamples.Count; ++j)
            {
                var left = significantSamples[i];
                var right = significantSamples[j];

                if (Mathf.Abs(left.x - right.x) == 1 && Mathf.Abs(right.y - left.y) == 1)
                {
                    var leftSample = getSample(left.x, left.y);
                    var rightSample = getSample(right.x, right.y);

                    nodes.Add((leftSample + rightSample) / 2);
                }
            }
        }

        // build paths
        distances = new float[nodes.Count, nodes.Count];

        for (var i = 0; i < nodes.Count; ++i)
        {
            for (var j = i + 1; j < nodes.Count; ++j)
            {
                if (i == j)
                {
                    distances[i, j] = distances[j, i] = 0;
                }
                else if (IsVisible(nodes[i], nodes[j]))
                {
                    distances[i, j] = distances[j, i] = Vector2.Distance(nodes[i], nodes[j]);
                }
                else
                {
                    distances[i, j] = distances[j, i] = Mathf.Infinity;
                }
            }
        }

        for (var k = 0; k < nodes.Count; ++k)
        {
            for (var i = 0; i < nodes.Count; ++i)
            {
                for (var j = 0; j < nodes.Count; ++j)
                {
                    if (distances[i, j] > distances[i, k] + distances[k, j])
                    {
                        distances[i, j] = distances[i, k] + distances[k, j];
                    }
                }
            }
        }

        var endTime = Time.realtimeSinceStartup;

        Debug.Log("time " + (endTime - startTime) + " nodes " + nodes.Count);
    }

    public Vector2 FindPath(Vector2 start, Vector2 end)
    {
        if (IsVisible(start, end))
            return end;

        var bestDistance = Mathf.Infinity;
        var bestNode = end;
        for (var i = 0; i < nodes.Count; ++i)
        {
            if (!IsVisible(start, nodes[i]))
                continue;

            var startDistance = Vector2.Distance(start, nodes[i]);
            if (startDistance < 0.1f)
                continue;

            for (var j = 0; j < nodes.Count; ++j)
            {
                if (distances[i, j] == Mathf.Infinity)
                    continue;

                if (!IsVisible(end, nodes[j]))
                    continue;

                var endDistance = Vector2.Distance(end, nodes[j]);

                var distance = startDistance + distances[i, j] + endDistance;

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestNode = nodes[i];
                }
            }
        }

        return bestNode;
    }

    private bool IsVisible(Vector2 a, Vector2 b)
    {
        var ray = new Ray(a, b - a);

        foreach (var obstacle in obstacles)
        {
            float distance;

            if (!obstacle.bounds.IntersectRay(ray, out distance))
                continue;

            if (distance > Vector2.Distance(a, b))
                continue;

            return false;
        }

        return true;
    }

    private bool IsReachable(Vector2 sample)
    {
        foreach (var obstacle in obstacles)
        {
            if (obstacle.bounds.Intersects(new Bounds(sample, Vector2.one)))
            {
                return false;
            }
        }

        return true;
    }
}
