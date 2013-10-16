using UnityEngine;
using System.Collections;

public class GrenadeEffect : MonoBehaviour {
	
	Player owner;
	
	public tk2dSprite sprite;
	
	public float time;
	private float timer;
	
	public float startXVel, startYVel;
	
	//exploding
	public GameObject explosionPrefab;

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
			explode();
		}
		
		//when not exploding, blink
		if (timer < 1f){
			float blinkTime = 0.1f;
			sprite.gameObject.SetActive( (Time.time % blinkTime) < blinkTime*0.5f );
		}
		
	}
	
	void explode(){
		GameObject newExplosion = Instantiate(explosionPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
		newExplosion.SendMessage("setOwner", owner);
		
		Destroy(gameObject);
		
	}
	
	
}
