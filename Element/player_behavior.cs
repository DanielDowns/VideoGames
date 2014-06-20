using UnityEngine;
using System.Collections;

public class player_behavior: MonoBehaviour {

    public AudioSource sound;

	public int count = 0; //frame counter

	//audio shield variables
	//bool bool_shield = true;

	public int fire_limiter = 10; //higher = slower fire rate
	public int missile_limiter = 100;
	public int beam_limiter = 100;
	public int regen_shield = 300;

	public int speed = 5; //how fast the ship moves

	public string element = "nothing";

	private int el = 0; //0=water, 1=fire, 2=earth

	public bool shield_power = false; //draw shield or not
	public GameObject shield_object;

	public int shield_max = 6;
	public int health_max = 3;
	public int shield;  //auto regens
	public int health;  //permanant damage
	public static int lives = 4;   //number of chances

	public int damage_timer = 0; //how long since last damage

	public int self_index; //used for shield location

	public float score = 0;
	public Vector3 locus; //current position
	public Vector3 respawnPoint;

	//
	public int frame_swap = 0;
	public int dead_frame = 0;

	public int invinc_count = 0;
	public bool respawning = true;
	public static bool hasRelic = false;

	//the boundaries that the player ship can move
	public float xMin = -6f;
	public float xMax = 6f;
	public float yMin = -3f;
	public float yMax = 4f;

	//input controls
	public KeyCode u;
	public KeyCode l;
	public KeyCode r;
	public KeyCode d;
	public KeyCode f;

	//sprites used for damage
	public Sprite damage;
	public Sprite ship;

	// Use this for initialization
	void Start () {
		lives = 4;

		u = KeyCode.W;
		l = KeyCode.A;
		r = KeyCode.D;
		d = KeyCode.S;
		f = KeyCode.Space;

		shield = shield_max;
		health = health_max;
		self_index = 0;
		
		ship = Resources.Load<Sprite>("ship");
		damage = Resources.Load<Sprite>("ship-red");

		shield_object = (GameObject)Instantiate(Resources.Load("shield_prefab"), gameObject.transform.position, gameObject.transform.rotation);
		shield_object.SendMessage("setOwner", "player");

		respawnPoint = new Vector3 (0,-2,0);
		cycleElement ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (lives);

		locus = gameObject.transform.position;

		count ++;
		if (count == 1000000) {
			count = 0;
		}

		bool up = Input.GetKey (u);
		bool left = Input.GetKey (l);
		bool right = Input.GetKey (r);
		bool down = Input.GetKey (d);

		bool space = Input.GetKeyDown (f);

		//ship must be moving to be hit by bullets
		transform.Translate(Vector3.up * Time.deltaTime * .005f);


		if(invinc_count > 0){
			invinc_count--;
		}

		if(respawning ){
			gameObject.transform.position = Vector3.MoveTowards(locus, respawnPoint, .05f);
			if(gameObject.transform.position == respawnPoint){
				respawning = false;
				invinc_count = 40;
			}

		}
		else{
			//movement
			if (up && gameObject.transform.position.y < yMax) {
				transform.Translate(Vector3.up * Time.deltaTime * speed);
			}
			if (left && gameObject.transform.position.x > xMin) {
				transform.Translate(Vector3.left * Time.deltaTime * speed);
			}
			if (right && gameObject.transform.position.x < xMax) {
				transform.Translate(Vector3.right * Time.deltaTime * speed);
			}
			if (down && gameObject.transform.position.y > yMin) {
				transform.Translate(Vector3.down * Time.deltaTime * speed);
			}
		}
		//element swapping
		if (space) {
			cycleElement();
		}

		//firing//////////
		Vector3 v = new Vector3(0,0,0);
		transform.Translate(v);		
		transform.Translate(new Vector3(0,0,0));		
		Vector3 vector = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
	

		//standard fire
		if(count % fire_limiter == 0){
			if(element.Equals("water")){
				GameObject shot = (GameObject)Instantiate(Resources.Load("player_shot_water"), vector, gameObject.transform.rotation);
				shot.SendMessage("setElement", element);
				shot.SendMessage("setShooter", gameObject.ToString());
			} 
			else if(element.Equals("fire")){
				GameObject shot = (GameObject)Instantiate(Resources.Load("player_shot_fire"), vector, gameObject.transform.rotation);
				shot.SendMessage("setElement", element);
				shot.SendMessage("setShooter", gameObject.ToString());
			} 
			else if(element.Equals("earth")){
				GameObject shot = (GameObject)Instantiate(Resources.Load("player_shot_earth"), vector, gameObject.transform.rotation);
				shot.SendMessage("setElement", element);
				shot.SendMessage("setShooter", gameObject.ToString());
			}
			else{
				GameObject shot = (GameObject)Instantiate(Resources.Load("player_shot_neutral"), vector, gameObject.transform.rotation);
				shot.SendMessage("setElement", element);
				shot.SendMessage("setShooter", gameObject.ToString());
			}

		}

		//missile firing
		if(count % missile_limiter == 0 && element.Equals("earth")){
			GameObject mis = (GameObject)Instantiate(Resources.Load("player_shot_missile"), vector, gameObject.transform.rotation);
			mis.SendMessage("setShooter", gameObject.ToString());
		}

		//shield deploy
		if (element.Equals ("water")) {
			waterShield ();
		}else {
			shield_power = false;
		}

		//fire blast firing
		if(count % beam_limiter == 0 && element.Equals("fire")){
			GameObject beam = (GameObject)Instantiate(Resources.Load("player_fireLauncher"), vector, gameObject.transform.rotation);
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



	//controls how the water shield works
	void waterShield(){
		if (damage_timer <= regen_shield) {
			damage_timer++;
		}
		if (damage_timer >= regen_shield) {
			shield = shield_max;
		}

		if (shield > 0) {
			if (shield_power == false) {
                sound = (AudioSource)gameObject.AddComponent("AudioSource");
                AudioClip soundClip;
                soundClip = (AudioClip)Resources.Load("sound_fx/energy_shield1");
                sound.clip = soundClip;
                sound.volume = 0.1f;
                sound.Play();
            }
            shield_power = true;
		} else {
			shield_power = false;
		}

		if (shield_object == null) {
			Debug.Log("wtf.....");
			shield_object = (GameObject)Instantiate(Resources.Load("shield_prefab"), new Vector3(-100,0,0), gameObject.transform.rotation);
		}
	}

	public void cycleElement(){
		Debug.Log (upgradeLORD.total);

		if(upgradeLORD.total == 0){
			element = "nothing";
		}

		if(upgradeLORD.total == 1){
			if(upgradeLORD.getEarth){
				element = "earth";
			}
			else if(upgradeLORD.getFire){
				element = "fire";
			}
			else{
				element = "water";
			}
		}

		if(upgradeLORD.total == 2){
			el++;
			if(el == 2){
				el = 0;
			}
			Debug.Log (el);
			if(upgradeLORD.getEarth && upgradeLORD.getFire){
				if(el == 0){
					element = "fire";
				}
				else{
					element = "earth";
				}
			}
			else if(upgradeLORD.getWater && upgradeLORD.getFire){
				if(el == 0){
					element = "water";
				}
				else{
					element = "fire";
				}
			}
			else{
				if(el == 0){
					element = "water";
				}
				else{
					element = "earth";
				}
			}
		}

		if(upgradeLORD.total == 3){
			el++;
			if (el == 3) {
				el = 0;
			}
			if (el == 0) {
				element = "water";
			}
			else if (el == 1) {
				element = "fire";
			}
			else if (el == 2) {
				element = "earth";
			}
		}


	}

	//collision detection
	void OnTriggerEnter(Collider c){
		if(invinc_count == 0){
			if (c.gameObject.ToString ().Contains ("boss")) {
				die ();
			}
			else if (!c.gameObject.ToString ().Contains ("player") && !respawning && !c.gameObject.ToString ().Contains ("Relic") && !c.gameObject.ToString ().Contains ("shield")) {  //hit by anything
				show_damage();

                if (c.gameObject.ToString().Contains("enemy")) {
                    Instantiate(Resources.Load("expl 3"), c.gameObject.transform.position, gameObject.transform.rotation);
                }

                if (!c.gameObject.ToString().Contains("enemy_shot_beam") && !c.gameObject.ToString().Contains("boss")) {
                    DestroyObject(c.gameObject);
                }
				damage_timer = 0;

				GameObject s = null;
				if(gameObject.ToString().StartsWith("player2")){
					s = GameObject.Find("shield_prefab2(Clone)");
				}
				else if(gameObject.ToString().StartsWith("player")){
					s = GameObject.Find("shield_prefab(Clone)");
				}

				if(shield > 0 && s.GetComponent<shield_script>().getOn()){
					shield--;
				}
				else{
					health--;
					if(health == 0){
						die ();				
					}
				}
			}
		}
	}

	void show_damage(){
		gameObject.GetComponent<SpriteRenderer>().sprite = damage;
		frame_swap = 5;
	}

	void show_good(){
		gameObject.GetComponent<SpriteRenderer>().sprite = ship;
	}

	void die(){
		Instantiate(Resources.Load("expl 3"), gameObject.transform.position, gameObject.transform.rotation);

		lives--;

		if(lives == 0){
			
			Debug.Log("sgame over");
			//game over
			Application.LoadLevel(11);
		}

		float X = 0 - locus.x;
		float Y = 0 - locus.y;
		float Z = 0 - locus.z;
		gameObject.transform.Translate(new Vector3(X, Y - 5, Z));

		
		shield = shield_max;
		health = health_max;
		respawning = true;
	}

	public int getHealth(){
		return health;
	}
	public int getShield(){
		return shield;
	}

	public void setHealth(int h){
		health = h;
	}

	public void setShield(int s){
		shield = s;
	}

	public Vector3 getLocus(){
		return locus;
	}

	public string getElement(){
		return element;
	}

	public void setElement(string s){
		element = s;
	}

	public void setDamageTimer(int i){
		damage_timer = i;
	}

	public float getScore(){
		return score;
	}

	public void setScore(float i){
		score = i;
	}

	public bool getShieldPower(){
		return shield_power;
	}
}
