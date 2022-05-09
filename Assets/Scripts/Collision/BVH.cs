using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        queryResultCount = 0;
        // create BVH root node
        List<Body> sorted = bodies.OrderBy(body => body.position.x).ToList();

        rootNode = new BVHNode(sorted);
    }



    public override void Query(AABB aabb, List<Body> results)
    {
        rootNode.Query(aabb, results);
    }



    public override void Query(Body body, List<Body> results)
    {
        Query(body.shape.GetAABB(body.position), results);
    }



    public override void Draw()
    {
        rootNode?.Draw();
    }
}
