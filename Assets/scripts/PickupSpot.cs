using UnityEngine;
using System.Collections;

public class PickupSpot : MonoBehaviour {
	
	
	private GameObject powerObject;
	
	private bool isActive;
	
	//giving the cloneKillPower
	private bool cloneKillActive;
	
	
	//timing
	public float time;
	private float timer;
	public float flashTime;
	public float flashSpeed;
	
	//sprites
	public tk2dSpriteAnimator doorSprite;
	private tk2dSpriteAnimationClip openClip, closeClip;
	public tk2dSprite pickupSprite;
	
	int pickupSpriteNormalID, pickupSpriteCloneKillID;
	
	
	//text
	public tk2dTextMesh textObject;
	
	private HUD hud;

	// Use this for initialization
	void Awake () {
		
		isActive = false;
		
		//cache the animations
		openClip = doorSprite.GetClipByName("pickupDoorOpen");
		closeClip = doorSprite.GetClipByName("pickupDoorClose");
		
		pickupSpriteNormalID = pickupSprite.GetSpriteIdByName("pickup");
		pickupSpriteCloneKillID = pickupSprite.GetSpriteIdByName("pickupCloneKill");
		
		//find the HUd
		//hud = GameObject.Find("HUD").GetComponent<HUD>();
	}
	
	public void activate(GameObject _powerObject){
		isActive = true;
		cloneKillActive = false;
		
		timer = time;
		
		powerObject = _powerObject;
		pickupSprite.gameObject.SetActive(true);
		pickupSprite.spriteId = pickupSpriteNormalID;
		
		//set the text
		textObject.gameObject.SetActive(true);
		textObject.text = powerObject.GetComponent<Power>().powerName;
		textObject.Commit();
		
		//open the door
		doorSprite.Play(openClip);
	}
	
	public void activateCloneKill(){
		isActive = true;
		cloneKillActive = true;
		
		timer = time;
		powerObject = null;
		pickupSprite.gameObject.SetActive(true);
		pickupSprite.spriteId = pickupSpriteCloneKillID;
		
		//set the text
		textObject.gameObject.SetActive(true);
		textObject.text = "EXORCISM";
		textObject.Commit();
		
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
					if (powerObject != null){
						GameObject thisPower = Instantiate(powerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
						Power powerToAssign = thisPower.GetComponent<Power>();
						if (powerToAssign.assignToPlayer(thisPlayer)){
							hud.addIcon(powerToAssign);
						}
					}
					//or give them the gift of a clone kill
					else{
						thisPlayer.activateCloneKill();
						hud.addCloneKillIcon();
					}
					
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
	
	public HUD Hud {
		get {
			return this.hud;
		}
		set {
			hud = value;
		}
	}
	
	public GameObject PowerObject {
		get {
			return this.powerObject;
		}
	}
	
	public float Timer {
		get {
			return this.timer;
		}
		set {
			timer = value;
		}
	}
}
