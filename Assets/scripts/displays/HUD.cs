using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {
	
	public PlayerController player; 
	
	public GameManager gm;
	
	public tk2dTextMesh textSprite;
	public GameObject hudBox;
	
	public GameObject iconPrefab;
	public GameObject anchor;
	private List<GameObject> pickupIcons = new List<GameObject>();
	private Dictionary<string, int> iconIds = new Dictionary<string, int>();
	public Vector2 iconStartOffset;
	public float iconSpacing;
	
	public int maxNumIconsPerRow;
	public float iconSpacingVert;
	
	//showing clone kills
	private tk2dSprite cloneKillIcon;
	private int cloneKillIconID;
	public Color cloneKillIconColA, cloneKillIconColB;
	public float cloneKillIconFlashSpeed;
	
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
		
		iconIds.Add("Uppercut", sampleSprite.GetSpriteIdByName("pickupIconUppercut"));
		iconIds.Add("Spike Ball", sampleSprite.GetSpriteIdByName("pickupIconSpikeBarrel"));
		iconIds.Add("Fat Gun", sampleSprite.GetSpriteIdByName("pickupIconFatGun"));
		iconIds.Add("10 Ton Drop", sampleSprite.GetSpriteIdByName("pickupIconTenTonDrop"));
		iconIds.Add("Boomerang", sampleSprite.GetSpriteIdByName("pickupIconBoomerang"));
		iconIds.Add("Knives", sampleSprite.GetSpriteIdByName("pickupIconKnives"));
		iconIds.Add("Mines", sampleSprite.GetSpriteIdByName("pickupIconMine"));
		
		
		cloneKillIconID = sampleSprite.GetSpriteIdByName("cloneKillIcon");
		
	}
	
	public void reset(){
		if (!gm.DoingIntro){
			hudBox.SetActive(true);
		}
		
		//get rid of the icons
		for (int i=0; i<pickupIcons.Count; i++){
			Destroy(pickupIcons[i]);
		}
		pickupIcons.Clear();
	}
	
	public void addIcon(Power newPower){
		//make a new icon
		int row = pickupIcons.Count < maxNumIconsPerRow ? 0 : 1;
		int col = pickupIcons.Count % maxNumIconsPerRow;
		Vector3 newPos = new Vector3( anchor.transform.position.x+ iconStartOffset.x + iconSpacing*col, anchor.transform.position.y+iconStartOffset.y - row*iconSpacingVert, 0);
		
		GameObject newIconObj = Instantiate(iconPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		tk2dSprite newIconSprite = newIconObj.GetComponent<tk2dSprite>();
		newIconSprite.spriteId = iconIds[newPower.powerName];
		
		newIconSprite.transform.position = newPos;
		
		//turn if off if the clone kill is active
		if (player.NextAttackIsCloneKill){
			newIconObj.SetActive(false);
		}
		
		pickupIcons.Add(newIconObj);
	}
	
	public void addCloneKillIcon(){
		/*
		Vector3 newPos = new Vector3(0,0,0);
		newPos.x = anchor.transform.position.x + cloneKillX;
		newPos.y = anchor.transform.position.y + cloneKillYStart + cloneKillIcons.Count*cloneKillYSpacing;
		*/
		
		Vector3 newPos = new Vector3( anchor.transform.position.x+ iconStartOffset.x , anchor.transform.position.y+iconStartOffset.y, 0);
		
		
		GameObject newCloneKillObj = Instantiate(iconPrefab, newPos, new Quaternion(0,0,0,0)) as GameObject;
		cloneKillIcon = newCloneKillObj.GetComponent<tk2dSprite>();
		
		cloneKillIcon.spriteId = cloneKillIconID;
		cloneKillIcon.color = cloneKillIconColA;
		
		//make all other icons invisible
		for (int i=0; i<pickupIcons.Count; i++){
			pickupIcons[i].SetActive(false);
		}
	}
	
	public void removeCloneKillIcon(){
		Destroy( cloneKillIcon.gameObject );
		//bring the other icons back
		for (int i=0; i<pickupIcons.Count; i++){
			pickupIcons[i].SetActive(true);
		}
	}
	
	/*
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
		//round
		topText += "\nRound: "+gm.RoundNum;
		//score
		topText += "\nScore: "+player.Score;
		
		if (!gm.DoingIntro){
			GUI.Label(textPos, topText, textStyle);
		}
	}
	*/
	
	void Update(){
		
		//set the text
		string curText = "";
		//lives
		curText += "Lives: "+player.LivesLeft;
		//round
		curText += "\nRound: "+gm.RoundNum;
		//score
		curText += "\nScore: "+player.Score;
		
		if (curText != textSprite.text){
			textSprite.text = curText;
			textSprite.Commit();
		}
		
		//if the player is about to use the clone kill, make it flash
		if (player.NextAttackIsCloneKill){
			float prc = Mathf.Abs(Mathf.Sin(Time.time * cloneKillIconFlashSpeed)) ;
			Color curCol = prc*cloneKillIconColA + (1-prc)*cloneKillIconColB;
			cloneKillIcon.color = curCol;
		}
		
	}
	
	
	//deal with the icons
	
	//TESTING
	/*
	void LateUpdate(){
		for (int i=0; i<pickupIcons.Count; i++){
			int row = i < maxNumIconsPerRow ? 0 : 1;
			int col = i % maxNumIconsPerRow;
			Vector3 newPos = new Vector3( anchor.transform.position.x+ iconStartOffset.x + iconSpacing*col, anchor.transform.position.y+iconStartOffset.y - row*iconSpacingVert, 0);
			
			pickupIcons[i].gameObject.transform.position = newPos;
		}
	}
	*/
	
}
