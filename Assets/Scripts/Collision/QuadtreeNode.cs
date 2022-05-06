using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode
{
    AABB nodeAABB;
    int nodeCapacity;
    int nodeLevel;
    List<Body> nodeBodies = new List<Body>();

    QuadtreeNode NorthEast;
    QuadtreeNode NorthWest;
    QuadtreeNode SouthEast;
    QuadtreeNode SouthWest;

    bool subdivided = false;

    public QuadtreeNode(AABB aabb, int capacity, int level)
    {
        nodeAABB = aabb;
        nodeCapacity = capacity;
        nodeLevel = level;
    }

    public void Insert(Body body)
    {
        if (!nodeAABB.Contains(body.position)) return;

        if (nodeBodies.Count < nodeCapacity)
        {
            nodeBodies.Add(body);
        }
        else
        {
            // exceeded capacity, subdivide node
            if (!subdivided) Subdivide();

            NorthEast.Insert(body);
            NorthWest.Insert(body);
            SouthEast.Insert(body);
            SouthWest.Insert(body);
        }
    }

    public void Query(AABB aabb, List<Body> results)
    {
        if (!nodeAABB.Contains(aabb)) return;

        results.AddRange(nodeBodies);

        if (subdivided)
        {
            NorthEast.Query(aabb, results);
            NorthWest.Query(aabb, results);
            SouthEast.Query(aabb, results);
            SouthWest.Query(aabb, results);
        }

    }

    public void Draw()
    {
        Color color = BroadPhase.colors[nodeLevel % BroadPhase.colors.Length];

        nodeAABB.Draw(color);
        nodeBodies.ForEach(body => Debug.DrawLine(nodeAABB.center, body.position, color));

        // draw northeast node
        if (NorthEast != null) NorthEast.Draw();
        // draw northwest node
        if (NorthWest != null) NorthWest.Draw();
        // draw southeast node
        if (SouthEast != null) SouthEast.Draw();
        // draw southwest node
        if (SouthWest != null) SouthWest.Draw();
    }

    private void Subdivide()
    {
        float xo = nodeAABB.extents.x * 0.5f;
        float yo = nodeAABB.extents.y * 0.5f;

        NorthEast = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        NorthWest = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x + xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        SouthEast = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y - yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        SouthWest = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x + xo, nodeAABB.center.y - yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);

        subdivided = true;
    }
}
