using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGoomba : Player {

	public float minTimeForDirChange, maxTimeForDirChange;
	private float dirChangeTimer;
	
	public float minTimeForJump, maxTimeForJump;
	private float jumpTimer;
	
	
	public override void customStart(){
		isPlayerControlled = false;
		canPickupPowers = false;
		powers = new List<Power>();
	}
	
	
	public override void customReset(){
		
		dirChangeTimer = Random.Range(minTimeForDirChange, maxTimeForDirChange);
		jumpTimer = Random.Range(minTimeForJump, maxTimeForJump);
		
		//set the pos
		float spawnX = Random.Range(spawnLeft.transform.position.x, spawnRight.transform.position.x);
		transform.position = new Vector3(spawnX, spawnLeft.transform.position.y, 0);
		
		
		//clear velocity
		pushVel = new Vector3(0,0,0);
		curVel = new Vector3(0,0,0);
		
		//reset movement info
		speed = baseSpeed;
		fallingGrav = fallingGravBase;
		numDoubleJumpsUsed = 0;
		
	}
	
	
	public override void customUpdate(){
		
		//check the timers
		dirChangeTimer -= Time.deltaTime;
		jumpTimer -= Time.deltaTime;
		
		//was anything pressed?
		bool jumpNow = false;
		
		//also change direction if we hit a wall with our feet on the gorund
		bool forceDirChange = false;
		if ( (controller.collisionFlags & CollisionFlags.CollidedSides) != 0 && (controller.collisionFlags & CollisionFlags.Below) != 0){
			forceDirChange = true;
		}
		
		//check them timers!
		if (dirChangeTimer <= 0 || forceDirChange){
			dirChangeTimer = Random.Range(minTimeForDirChange, maxTimeForDirChange);
			facingDir *= -1;
		}
		if (jumpTimer <= 0){
			jumpNow = true;
			jumpTimer = Random.Range(minTimeForJump, maxTimeForJump);
		}
		
		
		
		//running
		float curSpeed = baseSpeed * facingDir;
		curVel.x = curSpeed;
		
		//jumping
		if (jumpNow && (controller.isGrounded || numDoubleJumpsUsed < numDoubleJumps)){
			startJump();
			if (!controller.isGrounded){
				numDoubleJumpsUsed++;
			}
		}
		
		//gravity
		if (!controller.isGrounded){
			if (curVel.y>0){
				curVel.y -= jumpGrav*Time.deltaTime;
			}else{
				curVel.y -= fallingGrav*Time.deltaTime;	
				isJumping = false;
			}
		}else if (!isJumping){
			curVel.y = -1; //need to keep pushing down for it to register as grounded so we can jump
		}
		
		if (controller.isGrounded){
			numDoubleJumpsUsed = 0;
		}
		
		//friciton if they were pushed
		pushVel *= Mathf.Pow(pushFric, Time.deltaTime);
		
		//actually move this guy
		controller.Move(curVel*Time.deltaTime + pushVel*Time.deltaTime);
	}
	
	
	//kill player son touch
	
	void OnTriggerEnter(Collider other) {
		//if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			Debug.Log("shave your dad");
		//}
	}
	
	void OnCollisionEnter(Collision collision) {
		Debug.Log("toot toot ");
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
            audio.Play();
        
    }
}
