using UnityEngine;
using System.Collections;

public class Bullet : PowerEffect {
	
	public float time;
	float timer;
	
	public float speed;
	Vector3 vel;
	
	public Vector3 pushForce;
	
	public bool canRicochet;
	public int numRicochets;
	private int ricochetsLeft;
	
	public tk2dSprite sprite;

	public override void setupCustom(){
		
		timer = time;
		
		pushForce.x *= Owner.facingDir;
		
		vel = new Vector3( speed*Owner.facingDir, 0, 0);
		
		float spriteAngle = Mathf.Atan2( vel.y, vel.x);
		sprite.gameObject.transform.localEulerAngles = new Vector3(0,0,  spriteAngle*Mathf.Rad2Deg);
		
		//vel.x += Owner.CurVel.x;
		transform.position = Owner.transform.position + new Vector3(0.5f*Owner.facingDir, 0, 0);
		
		ricochetsLeft = numRicochets;
		
		//rigidbody.AddForce(vel);
	}
	
	// Update is called once per frame
	void Update () {
		
		//move it
		
		rigidbody.velocity = vel;
		//transform.position += vel * Time.deltaTime;
		
		//is it time to die?
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		
	}
	
	
	void OnCollisionEnter(Collision collision) {
        
		//ignore player shells
		if (collision.gameObject.layer == LayerMask.NameToLayer("player")){
			return;
		}
		
		//did we touch a player?
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != Owner){
				thisPlayer.takeDamage(Owner);
			}else{
				return;
			}
		}
		
		//destroy or ricochet after hitting something
		if (!canRicochet){
			Destroy(gameObject);
		}else{
			ricochet();
		}
    }
	
	void ricochet(){
		numRicochets--;
		
		if (numRicochets == 0){
			Destroy(gameObject);
		}else{
			
			//try random angles until we find one that is not bloack
			
			bool goodAngle = false;
			float distToCheck = 1;
			
			int numChecks = 0;
			int maxNumChecks = 15;
			
			while (!goodAngle && numChecks<maxNumChecks){
				numChecks++;
				float newAngle = Random.Range(0, Mathf.PI*2);
				
				Vector3 testVec = new Vector3( Mathf.Cos(newAngle), Mathf.Sin(newAngle), 0);
				
				if (Physics.Raycast(transform.position, testVec, distToCheck)){
					goodAngle = false;
				}else{
					goodAngle = true;
					vel = new Vector3( Mathf.Cos(newAngle)*speed, Mathf.Sin(newAngle)*speed, 0);
				}
			}
			
			//if we're spending too much time trying to find a safe new angle, just kill this thing
			if (numChecks >= maxNumChecks){
				Destroy(gameObject);
			}
		}
		
		float spriteAngle = Mathf.Atan2( vel.y, vel.x);
		sprite.gameObject.transform.localEulerAngles = new Vector3(0,0,  spriteAngle*Mathf.Rad2Deg);
	}
}
