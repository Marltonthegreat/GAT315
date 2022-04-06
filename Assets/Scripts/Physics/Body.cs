using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public enum eForceMode
    {
        Force,
        Velocity,
        Acceleration
    }

    public Shape shape;

    public Vector2 position { get => transform.position; set => transform.position = value; }
    public Vector2 velocity = Vector2.zero;
    public Vector2 acceleration = Vector2.zero;
    public Vector2 force = Vector2.zero;
    public float mass => shape.mass;
    public float inverseMass { get => (mass == 0) ? 0 : 1 / mass; }

    public void ApplyForce(Vector2 force, eForceMode forceMode)
    {
        switch (forceMode)
        {
            case eForceMode.Force:
                acceleration += force * inverseMass;
                break;
            case eForceMode.Acceleration:
                acceleration += force;
                break;
            case eForceMode.Velocity:
                velocity = force;
                break;
            default:
                break;
        }
    }

    public void Step(float dt)
    {
        //Acceleration = Simulator.Instance.gravity + force * inverseMass * dt;
    }
}
