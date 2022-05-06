using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] List<Force> forces;
	[SerializeField] BoolData simulate;
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData textFPS;

	BroadPhase broadPhase = new Quadtree();
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


		if (!simulate.value) return;

		forces.ForEach(force => force.ApplyForce(bodies));

		Vector2 screenSize = GetScreenSize();
		while (timeAccumulator > fixedDeltaTime)
		{
			broadPhase.Build(new AABB(Vector2.zero, screenSize), bodies);
			var contacts = new List<Contact>();
			Collision.CreateBroadPhaseContacts(broadPhase, bodies, contacts);
			Collision.CreateNarrowPhaseContacts(contacts);

			//Collision.CreateContacts(bodies, out var contacts);
			Collision.SeperateContacts(contacts);
			Collision.ApplyImpulses(contacts);

			bodies.ForEach(body =>
			{
				Integrator.SemiImplicitEuler(body, fixedDeltaTime);
				body.position = body.position.Wrap(screenSize * -0.5f, screenSize * 0.5f);
				body.shape.GetAABB(body.position).Draw(Color.red);
			});

			timeAccumulator -= fixedDeltaTime;

		}
		broadPhase.Draw();
		bodies.ForEach(body => body.acceleration = Vector2.zero);
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return world;
	}

	public Body GetScreenToBody(Vector2 position)
    {
		Body body = null;

		Ray ray = activeCamera.ScreenPointToRay(position);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

		if (hit.collider)
        {
			hit.collider.gameObject.TryGetComponent(out body);
        }

		return body;
    }

	public Vector2 GetScreenSize()
    {
		return activeCamera.ViewportToWorldPoint(Vector2.one) * 2;
    }

	public void Clear()
    {
		bodies.ForEach(body => Destroy(body.gameObject));
		bodies.Clear();
    }
}
