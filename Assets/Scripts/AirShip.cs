using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShip : MonoBehaviour
{

	public Rigidbody rb;

	private float height;

	[SerializeField] GameObject WindPoint1;
	[SerializeField] GameObject WindPoint2;

	WindManager windManager;

	private Rigidbody envelope;

	private bool IsMoving = false;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		height = 1f;
		WindPoint1 = GameObject.Find("WindPoint1");
		WindPoint2 = GameObject.Find("WindPoint2");
		windManager = GameObject.FindObjectOfType<WindManager>();
		envelope = GameObject.Find("Envelope").GetComponent<Rigidbody>();
	}

	void Update()
	{
		height = transform.position.y;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.AddForce(GetWindForce(WindPoint1));
		rb.AddForce(GetWindForce(WindPoint2));
		Vector3 Drag = this.GetDrag();
		envelope.AddForce(Drag);
    }

	Vector3 GetWindForce(GameObject Point)
	{
		Vector3 windSpeed = windManager.GetWind(Point.transform.position);
		float Area = 0.5f * Mathf.PI * 45f * 45f;
		float airDensity = AtmosphereManager.GetAmbientDensity(transform.position.y);
		Vector3 Force = Area * airDensity * windSpeed;
		return Force;
	}

	Vector3 GetDrag()
	{
		Vector3 Velocity = rb.velocity;
		float VelX = Velocity.x;
		float VelY = Velocity.y;
		float VelZ = Velocity.z;
		/*Assume Sphere for time being (assume beetle aerodynamics)*/
		float Area = Mathf.PI * 45f * 45f;
		float ForceX = GetAirResistance(VelX, 0.38f, height, Area);
		float ForceY = GetAirResistance(VelY, 0.38f, height, Area);
		float ForceZ = GetAirResistance(VelZ, 0.38f, height, Area);
		Vector3 Force = new Vector3(ForceX, ForceY, ForceZ);
		return Force;
	}

	public static float GetAirResistance(float Velocity, float DragCoefficient, float height, float Area)
	{
		float AmbientDensity = AtmosphereManager.GetAmbientDensity(height);
		float k = AmbientDensity * DragCoefficient * Area / 2;
		return (-1) * k * Velocity * Velocity * Mathf.Sign(Velocity);
	}
}
