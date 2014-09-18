using UnityEngine;
using System.Collections;

public class enemyFighter_script : enemyDamageBase_script {

	int frame_count = 0;

	string orders = "guard";
	GameObject mother;

	int speed = 40;
	int rotateSpeed = 80;

	int coverMax = 200;
	int scanLimiter = 20;
	int engageDistance = 150;
	int fireAngle = 10;
	int fireLimiter = 30;

	bool done = true;

	GameObject [] allTargets;
	GameObject target;

	Vector3 targetPos; //the location to flying to
	float idealAngle;

	// Use this for initialization
	void Start () {
		allTargets = GameObject.FindGameObjectsWithTag("Ally");
		health = 5;
	}
	
	// Update is called once per frame
	void Update () {
		frame_count++;
		gameObject.transform.Translate (Vector3.up * Time.deltaTime * speed);


		//check for targets
		acquireTarget();
		fire ();

		//generate random position
		if(target == null){
			if(orders.Equals("guard")){
				//pick out new cruiser
				if(Vector3.Distance(targetPos, mother.transform.position) > coverMax){
					genPos();
				}
				if (Vector3.Distance(gameObject.transform.position, targetPos) < 5){
					genPos();
				}
			}
			//determine if change in course is needed
			idealAngle = determineAngle(targetPos);
		}
		else{
			idealAngle = determineAngle(target.transform.position);
		}

		//angle left/right if needed
		if(idealAngle < 0){
			idealAngle += 360;
		}
		if(Mathf.Abs(idealAngle - gameObject.transform.rotation.eulerAngles.z) > 1){
			if(target == null){
				manuever(targetPos);
			}
			else{
				manuever(target.transform.position);
			}
		}

	}

	//generate random location near mother ship
	void genPos(){
		float X = Random.Range (mother.transform.position.x - 20, mother.transform.position.x + 20);
		float Y = Random.Range (mother.transform.position.y - 20, mother.transform.position.y + 20);
		targetPos = new Vector3 (X, Y, 0);
	}

	//the angle to rotate to be pointing at the target
	float determineAngle(Vector3 v){ 
		
		float adj = (gameObject.transform.position.y - v.y);
		float opp = (gameObject.transform.position.x - v.x);
		
		float cot = Mathf.Atan(opp/adj) * Mathf.Rad2Deg;
		
		cot *= -1;
		if(v.y > gameObject.transform.position.y){
			cot-=180;
		}
		cot += 180;

		return cot;
	}

	//moves left/right to better close on target
	void manuever(Vector3 loc){  

		float Z = 1;

		float comp = gameObject.transform.rotation.eulerAngles.z; 
		float old = comp;

		bool sentinal = false;
		if (comp < 180) {
		
			if((comp - 180) <= idealAngle && idealAngle <= comp){
				Z *= -1; //right
				sentinal = true;
			}
			else{
				comp += 360;
				if((comp - 180) <= idealAngle && idealAngle <= comp){
					Z *= -1; //right
				}
			}

		}
		else{
			if((comp - 180) <= idealAngle && idealAngle <= comp){
				Z *= -1; //right
				sentinal = true;
			}
		}

		gameObject.transform.Rotate (new Vector3 (0, 0, Z) * Time.deltaTime * rotateSpeed);
	}

	//scans for all viable targets and attempts to pick one in range
	int acquireTarget(){ 
		if(frame_count % scanLimiter == 0){
			allTargets = GameObject.FindGameObjectsWithTag("Ally");
		}

		foreach (GameObject t in allTargets){
			if(t != null){
				if(Vector3.Distance(gameObject.transform.position, t.transform.position) < engageDistance){
					target = t;
					return 0;
				}
			}
		}
		target = null;
		return 0;
	}

	int fire(){
		if(target == null){
			return 0;
		}

		if(Mathf.Abs(idealAngle - gameObject.transform.rotation.eulerAngles.z) <= 10){
			if(frame_count % fireLimiter == 0){
				Instantiate(Resources.Load("Prefabs/enemyBullet"), gameObject.transform.position, gameObject.transform.rotation);
			}
		}
		return 0;
	}

	public void setOrders(string s){
		orders = s;
	}

	public void setMother(GameObject m){
		mother = m;
	}
}
