using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Vector2 position { get => transform.position; set => transform.position = value; }
    public Vector2 Velocity = Vector2.zero;
    public Vector2 Acceleration = Vector2.zero;
    public float mass = 1;

    public void ApplyForce(Vector2 force)
    {
        Acceleration = force / mass;
    }
}
