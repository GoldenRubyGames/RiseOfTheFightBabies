using UnityEngine;
using System.Collections;

public class RocketEffect : MonoBehaviour {
	
	Player owner;
	
	//public float time;
	private float timer;
	
	public Vector3 moveForce;
	
	//exploding
	public Color explosionColor;
	private bool isExploding;
	public float explosionTime;
	public float explosionScale;
	public GameObject trigger;
	
	public void setup(Player _owner){
		owner = _owner;
		
		timer = explosionTime;
		
		moveForce.x *= owner.facingDir;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isExploding){
			rigidbody.AddForce(moveForce);
		}
		
		else{
			timer -= Time.deltaTime;
			if (timer <= 0){
				Destroy(gameObject);
			}
			
			float thisScale = (1-(timer/explosionTime)) * explosionScale;
			transform.localScale = new Vector3(thisScale, thisScale, thisScale);
		}
	}
	
	void explode(){
		isExploding = true;
		trigger.SetActive(true);
		timer = explosionTime;
		
		rigidbody.isKinematic = true;
		
		renderer.enabled = true;     //make sure it wasn't off from blinking
		renderer.material.color = explosionColor;
		
		transform.localScale = new Vector3(1,1,1);
		
		//flicker to create new collisions
		gameObject.SetActive(false);
		gameObject.SetActive(true);
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			//THIS CAN HIT THE OWNER BECAUSE IT'S A GODDAMN EXPLOSION
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			
			if (!isExploding){
				if (owner != thisPlayer){
					explode();
				}
			}else{
				thisPlayer.changeHealth(-1, owner);
			}
		}
		
	}
	
	
	void OnCollisionEnter(Collision collision) {
        
		//ignore player shells
		if (collision.gameObject.layer == LayerMask.NameToLayer("player")){
			explode();;
		}
		
		//did we touch a player?
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				explode();
			}else{
				return;
			}
		}
		
		explode();
        
    }
}