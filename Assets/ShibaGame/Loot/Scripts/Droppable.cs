using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droppable : MonoBehaviour
{

	public void Drop(Vector3 location, Quaternion angle)
	{
		location.y = 1.7166f;
		location.z += 2.0f;
		transform.position = location;
		Instantiate(gameObject, location, angle);
		Debug.Log("Loot instantiated at enemy location: " + location.ToString("F4"));
	}
}
