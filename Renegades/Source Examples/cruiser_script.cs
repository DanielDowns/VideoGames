using UnityEngine;
using System.Collections;

public class cruiser_script : enemyDamageBase_script {

	int speed = 5;
	int rotateSpeed = 5;

	GameObject anchor;

	string orders; //go left, right, or straight
	int duration; //how long to do orders
	int timer; //how long you've been doing orders

	int fighterReserves = 10;
	int guardNumber = 0;
	int patNumber = 0;
	int launchLimiter = 500;

	int moveMultiplier = 50;

	bool flip = false; //need to flip
	bool doneFlip = false;
	float startAngle = -1000;
	int burnTime = 1000;
	int burnTimer;

	int frame_counter = 0;

	// Use this for initialization
	void Start () {
		anchor = GameObject.Find ("centerAnchor");
		health = 25;
	}
	
	// Update is called once per frame
	void Update () {
		frame_counter++;

		if(flip){
			ivan();
		}
		else{
			move ();
		}

		if(Vector3.Distance(anchor.transform.position, gameObject.transform.position) > 1000){
			flip = true;
		}
		launchFighter ();

	}

	void move(){
		//movement
		timer--;
		gameObject.transform.Translate (Vector3.up * Time.deltaTime * speed);
		if(timer > 0){
			if(orders.Equals("left")){
				transform.Rotate(new Vector3(0,0,1) * Time.deltaTime * rotateSpeed);
			}
			else if(orders.Equals("right")){
				transform.Rotate(new Vector3(0,0,-1) * Time.deltaTime * rotateSpeed);
			}
		}
		else{
			genOrders ();
		}

	}

	void genOrders(){
		int val = Random.Range (0, 5); //randomize action
		int min = 0;
		int max = 0;
		if(val <= 1){
			orders = "straight";
			min = 5;
			max = 11;
		}
		else if(val == 3){
			orders = "left";
			min = 5;
			max = 20;
		}
		else if(val == 4){
			orders = "right";
			min = 5;
			max = 20;
		}
		timer = duration = moveMultiplier * Random.Range(min,max);
	}

	void ivan(){
		if(startAngle == -1000){
			burnTimer = burnTime;
			startAngle = gameObject.transform.rotation.eulerAngles.z;
		}
		if(doneFlip != true){
			transform.Rotate(new Vector3(0,0,1) * Time.deltaTime * rotateSpeed);
		}
		else{
			burnTimer--;
		}

		float end = startAngle + 180;
		if(end > 360){
			end -= 360;
		}
		if(end - gameObject.transform.rotation.eulerAngles.z < 2){
			doneFlip = true;
		}
		gameObject.transform.Translate (Vector3.up * Time.deltaTime * speed);
		if(burnTimer <= 0){
			startAngle = -1000;
			flip = false;
		}
	}

	void launchFighter(){
		if(fighterReserves > 0){
			if(frame_counter % launchLimiter == 0){

				if(guardNumber + patNumber < 5){
					GameObject fighter = (GameObject)Instantiate(Resources.Load("Prefabs/enemyFighter"), gameObject.transform.position, gameObject.transform.rotation);
					fighter.SendMessage("setOrders", "guard");
					fighter.SendMessage("setMother", gameObject);
				}

			}
		}
	}
}
