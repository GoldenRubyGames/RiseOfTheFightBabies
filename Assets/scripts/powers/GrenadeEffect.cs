using UnityEngine;
using System.Collections;

public class GrenadeEffect : PowerEffect {
	
	public tk2dSprite sprite;
	
	public float time;
	private float timer;
	
	public float startXVel, startYVel;
	
	public Color normColor, blinkColor;
	
	//exploding
	public GameObject explosionPrefab;

	public override void setupCustom(){
		
		timer = time;
		
		Vector3 startForce = new Vector3( startXVel*Owner.facingDir, startYVel, 0); 
		rigidbody.AddForce( startForce);
		
		sprite.color = normColor;
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
			sprite.color = (Time.time % blinkTime) < blinkTime*0.5f ? normColor : blinkColor;
			//sprite.gameObject.SetActive( (Time.time % blinkTime) < blinkTime*0.5f );
		}
		
	}
	
	void explode(){
		GameObject newExplosion = Instantiate(explosionPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
		newExplosion.GetComponent<Explosion>().setOwner(Owner, IsCloneKiller);
		Destroy(gameObject);
		
	}
	
	
}
