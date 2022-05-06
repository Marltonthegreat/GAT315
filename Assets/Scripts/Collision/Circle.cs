using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle
{
    public Vector2 Center { get; set; }
    public float Radius { get; set; }

    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public Circle(Body body)
    {
        Center = body.position;
        Radius = ((CircleShape)body.shape).radius;
    }

    public static bool Intersects(Circle circleA, Circle circleB)
    {
        Vector2 direction = circleA.Center - circleB.Center;
        float distance = direction.magnitude;
        float radius = circleA.Radius + circleB.Radius;

        return (distance <= radius);
    }
}
