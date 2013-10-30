using UnityEngine;
using System.Collections;

public class DefenseBotEffect : PowerEffect {
	
	public float distFromPlayer;
	public float speed;
	
	public int maxNumOrbs;
	
	private float angle;

	public override void setupCustom(){
		
		angle = 0;
		
		//figure out how many other dense orbs this player has
		GameObject[] otherOrbs = GameObject.FindGameObjectsWithTag("defenseBot");
		for (int i=0; i<otherOrbs.Length; i++){
			DefenseBotEffect otherOrb = otherOrbs[i].GetComponent<DefenseBotEffect>();
			if (otherOrb.Owner == Owner && otherOrb != this){
				angle = otherOrb.Angle + (Mathf.PI*2)/maxNumOrbs;
			}
		}
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//move it
		angle += speed*Time.deltaTime;
		
		//figure out where that would put it
		float newX = Owner.transform.position.x + Mathf.Cos(angle) * distFromPlayer;
		float newY = Owner.transform.position.y + Mathf.Sin(angle) * distFromPlayer;
		
		transform.position = new Vector3(newX, newY, 0);
		
	}
	
	
	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.takeDamage(Owner);
			}
		}
	}
	
	
	public float Angle {
		get {
			return this.angle;
		}
		set {
			angle = value;
		}
	}
}
