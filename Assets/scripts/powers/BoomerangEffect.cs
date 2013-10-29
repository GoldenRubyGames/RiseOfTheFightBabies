using UnityEngine;
using System.Collections;

public class BoomerangEffect : PowerEffect {
	
	public float timeToTarget; //how long it takes to hit the target
	public float moveCurve; //have it slow down at target
	public float rotSpeed;
	
	public Vector3 targetOffset;
	private Vector3 startPos;
	
	private float curAngle;
	private Vector3 targetPos;
	private bool isComingBack;
	
	private float timer;
	
	
	public override void setupCustom(){
		
		//spin with the player
		rotSpeed *= Owner.facingDir;
		curAngle = 0;
		
		targetOffset.x *= Owner.facingDir;
		
		startPos = Owner.transform.position;
		
		//set the target pos
		targetPos = Owner.transform.position + targetOffset;
		
		timer = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//move it
		timer += Time.deltaTime;
		
		float prc = timer/timeToTarget;
		
		//did we finish?
		if (prc >= 1){
			if (!isComingBack){
				isComingBack = true;
				timer = 0;
				prc = 0;
			}else{
				Destroy(gameObject);
			}
		}
		
		//curve it
		if (!isComingBack){
			prc = Mathf.Pow(prc, moveCurve);
		}else{
			prc = Mathf.Pow(prc, (1.0f/moveCurve));
		}
		
		if (!isComingBack){
			transform.position = Vector3.Lerp(startPos, targetPos, prc);
		}else{
			transform.position = Vector3.Lerp(targetPos, Owner.transform.position, prc);
		}
		
		
		//spin it!
		curAngle += rotSpeed*Time.deltaTime;
		transform.localEulerAngles = new Vector3( 0, 0, curAngle);
	
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
}
