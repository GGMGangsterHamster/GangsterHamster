// -------------------------------------------
// Door script copyright 2017 by Creepy Cat :)
// -------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDouble : MonoBehaviour {
	public enum Direction {X, Y, Z};
	public Direction directionType;

	public Transform doorL;
	public Transform doorR;

	private Vector3 initialDoorL;
	private Vector3 initialDoorR;
	private Vector3 doorDirection;

	//Control Variables
	public float speed = 2.0f;
	public float openDistance = 2.0f;

	//Internal... stuff
	private float point = 0.0f;
	private bool opening = false;

	// Use this for initialization
	void Start () {
		initialDoorL = doorL.localPosition;
		initialDoorR = doorR.localPosition;		
	}
	
	// Update is called once per frame
	void Update () {
		// Direction selection
		if (directionType == Direction.X) {
			doorDirection = Vector3.right;
		}else if(directionType == Direction.Y){
			doorDirection = Vector3.up;
		}else if(directionType == Direction.Z){
			doorDirection = Vector3.back;
		}

		// If opening
		if(opening) {
			point = Mathf.Lerp(point,1.0f,Time.deltaTime * speed);
		}else{
			point = Mathf.Lerp(point,0.0f,Time.deltaTime * speed);
		}

		// Move doors
		if (doorL) {
			doorL.localPosition = initialDoorL + (doorDirection * point * openDistance);
		}

		if (doorR) {
			doorR.localPosition = initialDoorR + (-doorDirection * point * openDistance);
		}
	}

	void OnTriggerEnter(Collider other) {
		opening = true;
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}

	void OnTriggerExit(Collider other) {
		opening = false;
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}
}
