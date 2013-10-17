using UnityEngine;
using System.Collections;

public class PickupSpot : MonoBehaviour {
	
	
	private GameObject powerObject;
	
	private bool isActive;
	
	
	//timing
	public float time;
	private float timer;
	public float flashTime;
	public float flashSpeed;
	
	//sprites
	public tk2dSpriteAnimator doorSprite;
	private tk2dSpriteAnimationClip openClip, closeClip;
	public tk2dSprite pickupSprite;
	
	//text
	public TextMesh textObject;

	// Use this for initialization
	void Awake () {
		
		isActive = false;
		
		//cache the animations
		openClip = doorSprite.GetClipByName("pickupDoorOpen");
		closeClip = doorSprite.GetClipByName("pickupDoorClose");
	}
	
	public void activate(GameObject _powerObject){
		isActive = true;
		
		timer = time;
		
		powerObject = _powerObject;
		pickupSprite.gameObject.SetActive(true);
		
		//set the text
		textObject.gameObject.SetActive(true);
		textObject.text = powerObject.GetComponent<Power>().powerName;
		
		//open the door
		doorSprite.Play(openClip);
		
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
		if (isActive){
			timer-=Time.deltaTime;
			
			//should we be flahsing?
			if (timer <= flashTime){
				bool isOn = (timer%flashSpeed) < flashSpeed/2;
				pickupSprite.gameObject.SetActive(isOn);
				textObject.gameObject.SetActive(isOn);
			}
			
			//time to deactivate?
			if (timer <= 0){
				deactivate();
			}
			
		}
	}
	
	public void deactivate(){
		isActive = false;
		
		pickupSprite.gameObject.SetActive(false);
		textObject.gameObject.SetActive(false);
		
		doorSprite.Play(closeClip);
	}
	
	
	//picking up the power up when it's active
	void OnTriggerEnter(Collider other) {
		if (isActive){
			if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
				//get the player
				Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
				
				if (thisPlayer.canPickupPowers){
					//give them a power up!
					GameObject thisPower = Instantiate(powerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
					thisPower.GetComponent<Power>().assignToPlayer(thisPlayer);
					
					//close the doors
					deactivate();
				}
			}
		}
	}
	
	
	
	//setters getters
	public bool IsActive {
		get {
			return this.isActive;
		}
		set {
			isActive = value;
		}
	}
}
