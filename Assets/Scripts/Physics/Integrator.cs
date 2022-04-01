using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    public static void ExplicitEuler(Body body, float dt)
    {
        body.position += body.Velocity * dt;
        body.Velocity += body.Acceleration * dt;
    }

    public static void SemiExplicitEuler(Body body, float dt)
    {
        body.Velocity += body.Acceleration * dt;
        body.position += body.Velocity * dt;
    }

}
