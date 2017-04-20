﻿using UnityEngine;
using System.Collections;


public class PlayerControl : MonoBehaviour {

	public static double PlayerHealth;
	public static bool enableinput=true;
	public float turning;
	public float movementSpeed;
	private Rigidbody rb;
	private Animation an;
	Vector3 movement;
	private double nextAction;
	private bool running;
	private bool jumping;
	public double hitCheck;
	public GameObject[] UIExtraLives;
	public GameObject UIGamePaused;

	// Use this for initialization
	void Start () {

		PlayerHealth = 10;
		movementSpeed = 7;
		nextAction = Time.time;
		rb = GetComponent<Rigidbody>();
		an = GetComponent<Animation>();
	}




	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftShift)){
			if (running==true){
				running = false;
			}else{
				running = true;
			}
		}
		if (PlayerHealth == 0) {
			if (Time.timeScale > 0f) {
				UIGamePaused.SetActive(true); // this brings up the pause UI
				Time.timeScale = 0f; // this pauses the game action
			}// else {
			//	Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
			//	UIGamePaused.SetActive(false); // remove the pause UI
			//}
		}else{
			if (enableinput) {
				transform.Rotate (new Vector3 (0, Input.GetAxis ("Mouse X") * turning, 0));
			}
		}
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");




		if (Time.time > nextAction) {
			if (Input.GetMouseButtonDown (1)) {
				an["Attack"].time = 0.0f;
				nextAction = Time.time + 0.75;
				an.CrossFade ("Attack");
				running=false;
				movementSpeed = 7;
				
			
			} else if (Input.GetMouseButtonDown (0)) {
					an["Attack"].time = 0.75f;
					nextAction = Time.time + 0.5;
					an.CrossFade ("Attack");
				running = false;
				movementSpeed = 7;
	
			} else if (Input.GetKeyDown(KeyCode.Space)){
				an.CrossFade("Jump");
				nextAction = Time.time + 0.7;
				movementSpeed = 7;

			}else {
				// Store the input axes.
				if ((h != 0) || (v != 0)) {
					if (running == false){
						movementSpeed = 7;
						an.CrossFade ("Walk");

					}else{
						movementSpeed = 13;	
						an.CrossFade("Run");
					}
				} else {
					
					an.CrossFade ("idle");
				}

			}


		}
		MoveFoward (h, v);

	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Hand") {
			PlayerHealth = PlayerHealth - 1;
			//Debug.Log (PlayerHealth);
			for (int i = 0; i < UIExtraLives.Length; i++) {
				if (i < (PlayerHealth)) { // show one less than the number of lives since you only typically show lifes after the current life in UI
					UIExtraLives [i].SetActive (true);
				} else {
					UIExtraLives [i].SetActive (false);
				}
			}
		}
	}
	void MoveFoward (float h, float v)
	{

		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v);
		movement = movement.normalized;



		float actualHorizontal;
		float actualVertical;



		actualVertical = Mathf.Cos(transform.rotation.eulerAngles.y/180*Mathf.PI)*movement.z - Mathf.Sin(transform.rotation.eulerAngles.y/180*Mathf.PI)*movement.x;
		actualHorizontal = Mathf.Sin(transform.rotation.eulerAngles.y/180*Mathf.PI)*movement.z + Mathf.Cos(transform.rotation.eulerAngles.y/180*Mathf.PI)*movement.x;




		// Normalise the movement vector and make it proportional to the speed per second.
		movement = new Vector3(actualHorizontal,0,actualVertical) * movementSpeed * Time.deltaTime;
		
		// Move the player to it's current position plus the movement.
		rb.MovePosition (transform.position + movement);
	}

}
