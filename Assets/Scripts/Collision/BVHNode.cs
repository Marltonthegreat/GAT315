using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
    AABB nodeAABB;
    List<Body> nodeBodies = new List<Body>();

    BVHNode Left;
    BVHNode Right;

    public BVHNode(List<Body> bodies)
    {
        nodeBodies = bodies;
        ComputeBoundaries();
        Split();
    }

    public void ComputeBoundaries()
    {
        if (nodeBodies.Count > 0) 
  {
            nodeAABB.center = nodeBodies[0].position;
            nodeAABB.size = Vector3.zero;

            nodeBodies.ForEach(body => this.nodeAABB.Expand(body.shape.GetAABB(body.position)));
        }
    }

    public void Split()
    {
        int length = nodeBodies.Count;
        int half = length / 2;
        if (half >= 1)
        {
            Left = new BVHNode(nodeBodies.GetRange(0,half));
            Right = new BVHNode(nodeBodies.GetRange(half, length - half));

            nodeBodies.Clear();
        }
    }

    public void Query(AABB aabb, List<Body> results)
    {
        if (!nodeAABB.Contains(aabb)) return;

        if (this.nodeBodies.Count > 0)
        {
            results.AddRange(nodeBodies);
        }

        Left?.Query(aabb, results);
        Right?.Query(aabb, results);
    }

    public void Draw()
    {
        nodeAABB.Draw(Color.white);

        Left.Draw();
        Right.Draw();
    }
}
