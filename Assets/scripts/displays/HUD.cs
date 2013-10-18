using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {
	
	public PlayerController player; 
	
	public GameManager gm;
	
	public Vector2 boxPos;
	public Vector2 boxSize;
	
	public GUIStyle textStyle;
	
	public GameObject iconPrefab;
	public GameObject anchor;
	private List<GameObject> pickupIcons = new List<GameObject>();
	private Dictionary<string, int> iconIds = new Dictionary<string, int>();
	public Vector2 iconStartOffset;
	public float iconSpacing;
		

	// Use this for initialization
	void Awake () {
	
		//get all of the sprite IDs
		tk2dSprite sampleSprite = iconPrefab.GetComponent<tk2dSprite>();
		
		//go through and cache all of the sprite ids
		iconIds.Add("Bat", sampleSprite.GetSpriteIdByName("pickupIconBat"));
		iconIds.Add("Death Orb", sampleSprite.GetSpriteIdByName("pickupIconDeathOrb"));
		iconIds.Add("Defense Bot", sampleSprite.GetSpriteIdByName("pickupIconDefBot"));
		iconIds.Add("Kick Dive", sampleSprite.GetSpriteIdByName("pickupIconKickDive"));
		iconIds.Add("Extra Jump", sampleSprite.GetSpriteIdByName("pickupIconJump"));
		iconIds.Add("Freeze Beam", sampleSprite.GetSpriteIdByName("pickupIconFreeze"));
		iconIds.Add("Grenade", sampleSprite.GetSpriteIdByName("pickupIconGrenade"));
		iconIds.Add("Gun", sampleSprite.GetSpriteIdByName("pickupIconGun"));
		iconIds.Add("Lightning", sampleSprite.GetSpriteIdByName("pickupIconLightning"));
		iconIds.Add("Quick Gun", sampleSprite.GetSpriteIdByName("pickupIconMachineGun"));
		iconIds.Add("Ricochet Gun", sampleSprite.GetSpriteIdByName("pickupIconRicochetGun"));
		iconIds.Add("Rocket", sampleSprite.GetSpriteIdByName("pickupIconRocket"));
		iconIds.Add("Speed Shoes", sampleSprite.GetSpriteIdByName("pickupIconSpeedShoes"));
		iconIds.Add("Sword", sampleSprite.GetSpriteIdByName("pickupIconSword"));
		
	}
	
	public void reset(){
		//get rid of the icons
		for (int i=0; i<pickupIcons.Count; i++){
			Destroy(pickupIcons[i]);
		}
		pickupIcons.Clear();
	}
	
	public void addIcon(Power newPower){
		//make a new icon
		Vector3 newPos = new Vector3( anchor.transform.position.x+ iconStartOffset.x + iconSpacing*pickupIcons.Count, anchor.transform.position.y+iconStartOffset.y, 0);
		GameObject newIconObj = Instantiate(iconPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		tk2dSprite newIconSprite = newIconObj.GetComponent<tk2dSprite>();
		newIconSprite.spriteId = iconIds[newPower.powerName];
		
		pickupIcons.Add(newIconObj);
	}
	
	void OnGUI(){
		
		//don't do anything when paused
		if (gm.Paused){
			return;
		}
		
		float xPos = boxPos.x;
		float yPos = boxPos.y;
		
		if (player.HudShakeTimer > 0){
			float shakeRange = 5;
			xPos += Random.Range(-shakeRange, shakeRange);
			yPos += Random.Range(-shakeRange, shakeRange);
		}
		
		Rect textPos = new Rect(xPos,yPos,boxSize.x,boxSize.y);
		
		//int dispNum = 
		string topText = "";
		
		//lives
		topText += "Lives: "+player.LivesLeft;
		//score
		topText += "\nScore: "+player.Score;
		
		/*
		//then list powers
		topText += "\n";
		for (int k=0; k<player.Powers.Count; k++){
			topText += "\n"+player.Powers[k].powerName;
		}
		*/
		
		
		GUI.Label(textPos, topText, textStyle);
	}
	
	
	//deal with the icons
	
	//TETSING
	void LateUpdate(){
		for (int i=0; i<pickupIcons.Count; i++){
			Vector3 newPos = new Vector3( anchor.transform.position.x+iconStartOffset.x + iconSpacing*i, anchor.transform.position.y+iconStartOffset.y, 0);
			pickupIcons[i].transform.position = newPos;
		}
	}
	
}
