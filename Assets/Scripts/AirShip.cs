using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShip : MonoBehaviour
{

	public Rigidbody rb;

	private float height;

	private bool IsMoving = false;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		height = 1f;
    }

	void Update()
	{
		height = transform.position.y;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.AddForce(GetWindForce());
		if (IsMoving)
		{
			Vector3 Drag = this.GetDrag();
			rb.AddForce(Drag);
		}
    }

	Vector3 GetWindForce()
	{
		Vector3 Force = new Vector3(0f, 0f, 0f) /*WindManager.Wind(height)*/;
		return Force;
	}

	Vector3 GetDrag()
	{
		Vector3 Velocity = rb.velocity;
		float VelX = Velocity.x;
		float VelY = Velocity.y;
		float VelZ = Velocity.z;
		/*Assume Sphere for time being (assume beetle aerodynamics)*/
		float Area = Mathf.PI * 80f * 80f;
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
		return (-1) * k * Velocity * Velocity;
	}
}
