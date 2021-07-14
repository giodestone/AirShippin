using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShip : MonoBehaviour
{

	public Rigidbody rb;

	private bool IsMoving = false;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (IsMoving)
		{
			Vector3 Drag = GetDrag();
			rb.AddForce(Drag);
		}
    }

	public Vector3 GetDrag()
	{
		Vector3 Velocity = rb.velocity;
		float VelX = Velocity.x;
		float VelY = Velocity.y;
		float VelZ = Velocity.z;
		/*Assume Sphere for time being (assume beetle aerodynamics)*/
		Area = Math.PI * 80 * 80;
		float ForceX = GetAirResistance<float>(VelX, 0.38, Area);
		float ForceY = GetAirResistance<float>(VelY, 0.38, Area);
		float ForceZ = GetAirResistance<float>(VelZ, 0.38, Area);
		Vector3 Force = new Vector3(ForceX, ForceY, ForceZ);
		return Force;
	}

	public static float GetAirResistance<T>(T Velocity, float DragCoefficient, float Area)
	{
		float AmbientDensity = AtmosphereManager.GetAmbientDensity();
		float k = AmbientDensity * DragCoefficient * Area / 2;
		return (-1) * k * Velocity * Velocity;
	}
}
