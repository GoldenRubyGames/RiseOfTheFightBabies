using UnityEngine;
using System.Collections;

public class SwordEffect : PowerEffect {
	
	
	//public float size;
	
	public float angleRange;
	public float speed;
	
	private int angleDir;
	private float curAngle;
	private float endAngle;
	
	public tk2dSprite sprite;
	
	public override void setupCustom(){
		
		angleDir = Owner.facingDir; //swip from front to back
		
		curAngle = angleRange/2 * -angleDir;
		endAngle = -curAngle;
		
		transform.position = Owner.transform.position;
		
		if (angleDir == 1){
			sprite.FlipX = true;
		}
		
		//transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
		
	}

	
	// Update is called once per frame
	void Update () {
		
		//keep it on the player
		transform.position = Owner.transform.position;
		
		//advance rotation
		curAngle += speed * Time.deltaTime * angleDir;
		
		//rotate into place
		transform.localEulerAngles = new Vector3(0,0, curAngle);
		
		//are we done?
		if ( (angleDir<0 && curAngle<endAngle) || (angleDir>0 && curAngle>endAngle) ){
			Destroy(gameObject);
		}
		
		
	}
	
	
}
