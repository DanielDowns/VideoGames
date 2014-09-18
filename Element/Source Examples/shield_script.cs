using UnityEngine;
using System.Collections;

public class shield_script : MonoBehaviour {

	bool needMove = true;
	public bool on = true;

	string owner;

	GameObject player;
	GameObject player2;

	void Start(){
		player = GameObject.Find("player");
		player2 = GameObject.Find("player2(Clone)");
	}

	// Update is called once per frame
	void Update () {
		if(owner.Equals("player")){
			GameObject player = GameObject.Find("player");
			on = player.GetComponent<player_behavior> ().getShieldPower();
			if (on) {
				needMove = true;
				gameObject.transform.position = player.GetComponent<player_behavior> ().getLocus ();
				
			} else if (needMove == true) {
				needMove = false;
				gameObject.transform.Translate (new Vector3 (-300,0,0));
			}
		}
		else if(owner.Equals("player2")){
			GameObject player = GameObject.Find("player2(Clone)");
			on = player.GetComponent<player2_behavior> ().getShieldPower();
			if (on) {
				needMove = true;
				gameObject.transform.position = player.GetComponent<player2_behavior> ().getLocus ();
				
			} else if (needMove == true) {
				needMove = false;
				gameObject.transform.Translate (new Vector3 (-300,0,0));
			}
		}

	}


	void OnTriggerEnter(Collider c){
		if(owner.Equals("player")){
			if (c.ToString ().Contains ("boss")) {
				player.GetComponent<player_behavior>().setHealth(0);
				player.GetComponent<player_behavior>().setShield(0);
			}
			else if(c.ToString ().Contains ("enemy_shot_beam")){
				player.GetComponent<player_behavior>().setShield(0);
			}
			else if (!c.gameObject.ToString ().Contains ("player") && !c.gameObject.ToString ().Contains ("shield")) {  //hit by anything
				if(c.gameObject.ToString().Contains("shot")){
					DestroyObject(c.gameObject);
				}
				player.GetComponent<player_behavior>().setDamageTimer(0);

				player.GetComponent<player_behavior>().setShield(player.GetComponent<player_behavior>().getShield()-1);
			}
		}
		else if(owner.Equals("player2")){
			if (c.ToString ().Contains ("boss")) {
				player2.GetComponent<player2_behavior>().setHealth(0);
				player2.GetComponent<player2_behavior>().setShield(0);
			}
			else if(c.ToString ().Contains ("enemy_shot_beam")){
				player2.GetComponent<player2_behavior>().setShield(0);
			}
			else if (!c.gameObject.ToString ().Contains ("player") && !c.gameObject.ToString ().Contains ("shield")) {  //hit by anything
				if(c.gameObject.ToString().Contains("shot")){
					DestroyObject(c.gameObject);
				}
				player2.GetComponent<player2_behavior>().setDamageTimer(0);
				
				player2.GetComponent<player2_behavior>().setShield(player2.GetComponent<player2_behavior>().getShield()-1);
			}
		}
	}

	public void setOwner(string s){
		owner = s;
	}

	public string getOwner(){
		return owner;
	}

	public bool getOn(){
		return on;
	}
}
