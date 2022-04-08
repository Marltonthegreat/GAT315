using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] List<Force> forces;
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData textFPS;

	float timeAccumulator = 0;
	float fixedDeltaTime { get => 1.0f / fixedFPS.value; }
	float previousFPS = 1.0f / 60;

	public List<Body> bodies = new List<Body>();
	Camera activeCamera;

	private void Start()
	{
		activeCamera = Camera.main;
	}

	private void Update()
	{
		float currentFPS = 1.0f / Time.deltaTime;
		float temp = currentFPS;
		float smoothing = .9f;
		currentFPS = (currentFPS * smoothing) + (previousFPS * (1.0f - smoothing));
		previousFPS = temp;
		textFPS.value = (currentFPS).ToString("F1");
		timeAccumulator += Time.deltaTime;

		forces.ForEach(force => force.ApplyForce(bodies));
		while (timeAccumulator > fixedDeltaTime)
		{
			bodies.ForEach(body =>
			{
				Integrator.SemiExplicitEuler(body, Time.deltaTime);
			});

			timeAccumulator -= fixedDeltaTime;
		}
		bodies.ForEach(body => body.acceleration = Vector2.zero);
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return world;
	}
}
