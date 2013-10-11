using UnityEngine;
using System.Collections;

public class SwordEffect : MonoBehaviour {
	
	
	//public float size;
	
	public float angleRange;
	public float speed;
	
	private Player owner;
	
	private int angleDir;
	private float curAngle;
	private float endAngle;
	

	public void setup(Player _owner){
		owner = _owner;
		
		angleDir = owner.facingDir; //swip from front to back
		
		curAngle = angleRange/2 * -angleDir;
		endAngle = -curAngle;
		
		transform.position = owner.transform.position;
		
		//transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
		
	}

	
	// Update is called once per frame
	void Update () {
		
		//keep it on the player
		transform.position = owner.transform.position;
		
		//advance rotation
		curAngle += speed * Time.deltaTime * angleDir;
		
		//rotate into place
		transform.localEulerAngles = new Vector3(0,0, curAngle);
		
		//are we done?
		if ( (angleDir<0 && curAngle<endAngle) || (angleDir>0 && curAngle>endAngle) ){
			Destroy(gameObject);
		}
		
		
	}
	
	
	public Player Owner {
		get {
			return this.owner;
		}
	}
}
