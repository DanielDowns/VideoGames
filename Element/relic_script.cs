using UnityEngine;
using System.Collections;

public class relic_script : enemy_hit{


	Vector3 target;
	Vector3 leave;

	float speed = 3f;

	int life = 1000;
	bool start = false;

	public static Sprite spr;

	// Use this for initialization
	void Start () {
		health = 25;
		target = pickPoint ();
		leave = new Vector3(Random.Range(-3,3),-10,0);

		loadSprite ();

	}
	
	// Update is called once per frame
	void Update () {
		if (start) {

			life--;
			gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, target, Time.deltaTime * speed);
			if (target == gameObject.transform.position) {
				target = pickPoint ();
			}

			if (life <= 0) {
				target = leave;
			}
			if(transform.position.y <= -9){
				Debug.Log("boss has it now");
				level_manager.bossHasRelic = true;
				Destroy(gameObject);
			}
		}

		//damage sprite issues
		if(frame_swap > 0){
			frame_swap--;
			if(frame_swap == 0){
				show_good();
			}
		}
		else if(frame_swap == 0){
			show_good ();
		}
	}

	Vector3 pickPoint(){
		float y = Random.Range (-3,5);
		float x = Random.Range (-5,5);
		return new Vector3(x,y,0);
	}

	void move(){
		start = true;
	}
}
