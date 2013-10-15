using UnityEngine;
using System.Collections;

public class GrenadeEffect : MonoBehaviour {
	
	Player owner;
	
	public float time;
	private float timer;
	
	public float startXVel, startYVel;
	
	//exploding
	public Color explosionColor;
	private bool isExploding;
	public float explosionTime;
	public float explosionScale;
	public GameObject trigger;
	
	public void setup(Player _owner){
		owner = _owner;
		
		timer = time;
		
		Vector3 startForce = new Vector3( startXVel*owner.facingDir, startYVel, 0); 
		rigidbody.AddForce( startForce);
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0){
			if(!isExploding){
				explode();
			}else{
				Destroy(gameObject);
			}
		}
		
		if (isExploding){
			float thisScale = (1-(timer/explosionTime)) * explosionScale;
			transform.localScale = new Vector3(thisScale, thisScale, thisScale);
		}else if (timer < 1f){
			//when not exploding, blink
			float blinkTime = 0.1f;
			renderer.enabled = (Time.time % blinkTime) < blinkTime*0.5f;
		}
	}
	
	void explode(){
		isExploding = true;
		trigger.SetActive(true);
		timer = explosionTime;
		
		rigidbody.isKinematic = true;
		
		renderer.enabled = true;     //make sure it wasn't off from blinking
		renderer.material.color = explosionColor;
	}
	
	
	void OnTriggerEnter(Collider other) {
		//Debug.Log("hit something");
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			//THIS CAN HIT THE OWNER BECAUSE IT'S A GODDAMN EXPLOSION
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			thisPlayer.changeHealth(-1, owner);
			
		}
	}
}
