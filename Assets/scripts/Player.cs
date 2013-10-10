using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	//character controller
	public CharacterController controller;
	
	//showing the player
	public GameObject avatar;
	public Color myColor;
	
	[System.NonSerialized]
	public int facingDir;
	
	//health
	public int baseHealth;
	private int health;
	
	//invincibility
	public float invincibilityTime;
	float invincibilityTimer;
	
	[System.NonSerialized]
	public Vector3 startPos;
	
	//powers
	[System.NonSerialized]
	public List<Power> powers = new List<Power>();
	public GameObject punchPowerObject;
	
	[System.NonSerializedAttribute]
	public int numDoubleJumps;
	
	private int score;
	
	//general status things that items may need to affect
	[System.NonSerializedAttribute]
	public bool isPlayerControlled;
	[System.NonSerialized]
	public float fallingGrav;
	[System.NonSerialized]
	public float speed;
	
	//general movement
	[System.NonSerialized]
	public Vector3 curVel;
	
	//showing stuff in HUD
	public float hudShakeTime;
	float hudShakeTimer;
	
	// Use this for initialization
	void Start () {
		avatar.renderer.material.color = myColor;
		startPos = transform.position;
		
		customStart();
		
		reset();
		
		facingDir = 1;
	}
	
	public virtual void customStart(){}
	
	void reset(){
		health = baseHealth;
		
		invincibilityTimer = invincibilityTime;
		
		clearPowers();
		
		//give them a punch
		GameObject powerObject = Instantiate(punchPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
		Power thisPower = powerObject.GetComponent<Power>();
		thisPower.assignToPlayer(this);
		
		customReset();
		
	}
	public virtual void customReset(){}
	
	// Update is called once per frame
	void Update () {
		
		customUpdate();
		
		
		//check on powers
		for (int i=0; i<powers.Count; i++){
			powers[i].update();
		}
		
		
		
		//showing damage taken
		if (invincibilityTimer >0 ){
			invincibilityTimer-=Time.deltaTime;
			bool showPlayer = Time.frameCount%10 < 5;
			avatar.renderer.enabled = showPlayer;
		}else if (avatar.renderer.enabled == false){
			avatar.renderer.enabled = true;
		}
		
		hudShakeTimer -= Time.deltaTime;
		
	}
	public virtual void customUpdate(){}
	
	
	
	public void changeHealth(int amount, Player source){
		
		//don't take damage whil invicible
		if (amount < 0 && invincibilityTimer > 0){
			return;
		}
		
		health += amount;
		hudShakeTimer = hudShakeTime;
		
		//if they took damage make them invicible
		if (amount < 0){
			invincibilityTimer = invincibilityTime;
		}
		
		//is this fucker dead?
		if (health == 0){
			//give the killer a point if they are real
			if (source != null && source != this){
				source.addScore(1);
			}
			
			//reset the player
			reset();
		}
	}
	
	public void addScore(int val){
		score += val;
	}
	
	public virtual void push(Vector3 power){} //ghosts cannot be pushed
	
	public bool getPower(Power newPower){
		//do not add this if the player already has it
		for (int i=0; i<powers.Count; i++){
			if (newPower.powerName == powers[i].powerName && !newPower.canStack){
				return false;
			}
		}
		
		//add it!
		powers.Add(newPower);
		newPower.gameObject.transform.parent = transform;
		//Debug.Log("player "+controllerNum+" just got "+newPower.powerName);
		
		//if there was only one power and it was the punch, get rid of it
		if (newPower.isAnAttack && powers[0].powerName == "Punch"){
			Destroy(powers[0].gameObject);
			powers.RemoveAt(0);
		}
		
		return true;
	}
	
	public void clearPowers(){
		for (int i=0; i<powers.Count; i++){
			powers[i].cleanUp();
		}
		
		powers.Clear();
	}
		
	//setters and getters
	
	public List<Power> Powers {
		get {
			return this.powers;
		}
		set {
			powers = value;
		}
	}
	
	public int Health {
		get {
			return this.health;
		}
		set {
			health = value;
		}
	}
	
	public float HudShakeTimer {
		get {
			return this.hudShakeTimer;
		}
		set {
			hudShakeTimer = value;
		}
	}
	
	public int Score {
		get {
			return this.score;
		}
		set {
			score = value;
		}
	}
	
	public Vector3 CurVel {
		get {
			return this.curVel;
		}
		set {
			curVel = value;
		}
	}
	
	public int NumDoubleJumps {
		get {
			return this.numDoubleJumps;
		}
		set {
			numDoubleJumps = value;
		}
	}
	
	public float Speed {
		get {
			return this.speed;
		}
		set {
			speed = value;
		}
	}
}
