using UnityEngine;
using System.Collections;

// PLEASE NOTE! THIS SCRIPT IS FOR DEMO PURPOSES ONLY :) //

public class Rotate : MonoBehaviour {
	public GameObject rotationObject;
	public int speed;

	void Update () {
		rotationObject.transform.Rotate (speed * Time.deltaTime, 0, 0);
	}
}
