using UnityEngine;
using System.Collections;

public class DefenseOrbEffect : MonoBehaviour {
	
	Player owner;
	
	public float distFromPlayer;
	public float speed;
	
	public int maxNumOrbs;
	
	private float angle;

	public void setup(Player _owner){
		owner = _owner;
		
		angle = 0;
		
		//figure out how many other dense orbs this player has
		GameObject[] otherOrbs = GameObject.FindGameObjectsWithTag("defenseOrb");
		Debug.Log(otherOrbs.Length + " other orbs");
		for (int i=0; i<otherOrbs.Length; i++){
			DefenseOrbEffect otherOrb = otherOrbs[i].GetComponent<DefenseOrbEffect>();
			if (otherOrb.Owner == owner && otherOrb != this){
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
		float newX = owner.transform.position.x + Mathf.Cos(angle) * distFromPlayer;
		float newY = owner.transform.position.y + Mathf.Sin(angle) * distFromPlayer;
		
		transform.position = new Vector3(newX, newY, 0);
		
	}
	
	
	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				thisPlayer.changeHealth(-1, owner);
			}
		}
	}
	
	public Player Owner {
		get {
			return this.owner;
		}
		set {
			owner = value;
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
