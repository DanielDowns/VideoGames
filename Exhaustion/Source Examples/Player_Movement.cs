using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour {

	Animator animate;
	float direction;
	int boost_count;
	Vector3 speed;
	bool intro;
	public static bool hasTNT;
	public static bool isDead;
	private Vector3 introv = new Vector3(0,(float)68.5,-1);
	// Use this for initialization
	void Start () {
		direction = 270.0f;
		boost_count = -50;
		hasTNT = false;
		isDead = false;
		intro = true;
		animate = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(intro){
			transform.position = Vector3.Lerp(transform.position,introv,Time.deltaTime);
			if(transform.position.y <= 70)
			{
				animate.SetTrigger ("begin");
				intro = false;
			}
		}
		rigidbody.freezeRotation = true;

		bool left = false;
		bool right = false;
		bool down = false;
		bool up = false;
		bool space = false;

		if (!isDead) {
			//player input
			left = Input.GetKey (KeyCode.LeftArrow);
			right = Input.GetKey (KeyCode.RightArrow);
			down = Input.GetKey (KeyCode.DownArrow);
			up = Input.GetKey (KeyCode.UpArrow);

			space = Input.GetKeyDown (KeyCode.Space);	
		}

		if (space) {
			if(boost_count < -50){
				game_timer.count -= 2;
				boost_count = 30;
			}
		}

		if (boost_count > 0) {
			speed = Vector3.down * Time.deltaTime * 25;
		}
		else {
			speed = Vector3.down * Time.deltaTime * 10;
		}
		boost_count--;

		if (down) {
			if(right){
				transformer(225.0f, speed);
			}
			else if(left){
				transformer(315.0f, speed);
			}
			else{
				transformer(270.0f, speed);
			}
		}
		else if (up) {
			if(right){
				transformer(135.0f, speed);
			}
			else if(left){
				transformer(45.0f, speed);
			}
			else{
				transformer(90.0f, speed);
			}
		}
		else if (left) {
			transformer(0.0f, speed);
		}
		else if (right) {
			transformer(180.0f, speed);
		}


		if (animate) {
			/*if (Time.timeSinceLevelLoad.CompareTo(2.35)){
				animate.SetBool ("begin", false);
			}*/
			if (down || left || right || up) {
				animate.SetTrigger ("start");
			} else {
				animate.SetTrigger ("stop");
				
			}
		}


	}

	void OnCollisionEnter(Collision c){
		string name = c.gameObject.name.ToString ();
		if(name.StartsWith("lava") || name.StartsWith("Guard") || name.StartsWith("Bug")){
			deathScript.show = true;
			transform.Translate(new Vector3(0,0,1));
		}
		if(name.Equals("TNT")){
			hasTNT = true;
		}
	}



	public void transformer(float angle, Vector3 speed){ //robots in disguise....
		transform.Translate(speed);
		float math = direction - angle;
		transform.Rotate(0,0,math);
		direction = angle;
	}
}
