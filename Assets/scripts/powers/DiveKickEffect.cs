using UnityEngine;
using System.Collections;

public class DiveKickEffect : MonoBehaviour {

	public Vector3 vel;
	public float velFriction;
	
	public Vector3 offset;
	
	public float time;
	private float timer;
	
	Player owner;
	
	public void setup(Player _owner){
		owner = _owner;
		
		timer = time;
		
		offset.x *= owner.facingDir;
		vel.x    *= owner.facingDir;
		
		transform.position = owner.transform.position + offset;
		
		renderer.material.color = owner.myColor;
		
		transform.localEulerAngles += new Vector3(0,0, 45 * owner.facingDir);
		
	}

	
	// Update is called once per frame
	void Update () {
		
		owner.push(vel);
		
		vel *= Mathf.Pow(velFriction, Time.deltaTime);
		
		transform.position = owner.transform.position + offset;
		
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("shield") ){
			ShieldEffect thisShield = other.gameObject.GetComponent<ShieldEffect>();
			if (thisShield.Owner != owner){
				Destroy(gameObject);
			}
		}
		
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				thisPlayer.changeHealth(-1, owner);
			}
		}
	}
	
}
