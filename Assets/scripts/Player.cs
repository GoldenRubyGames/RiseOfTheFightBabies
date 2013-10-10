using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public CharacterController controller;
	//getting input;
	public int controllerNum;
	string moveAxis, jumpButton, attack1Button, atack2Button; 
	
	//showing the player
	public GameObject avatar;
	public Color myColor;
	
	//general moving
	Vector3 curVel;
	Vector3 pushVel;
	public float pushFric;
	
	//moving left and right
	public float baseSpeed;
	float speed;
	private int facingDir;
	
	//jumping
	public float jumpGrav, fallingGrav;
	public float jumpPower;
	public float jumpCut;
	bool isJumping;
	
	int numDoubleJumps;
	int numDoubleJumpsUsed;
	
	//health
	public int baseHealth;
	int health;
	
	//invincibility
	public float invincibilityTime;
	float invincibilityTimer;
	
	Vector3 startPos;
	
	//powers
	List<Power> powers = new List<Power>();
	public GameObject punchPowerObject;
	
	private int score;
	
	//showing stuff in HUD
	public float hudShakeTime;
	float hudShakeTimer;
	
	// Use this for initialization
	void Start () {
		avatar.renderer.material.color = myColor;
		startPos = transform.position;
		
		reset();
		curVel = new Vector3(0,0,0);
		
		//default to controller
		moveAxis = "player0Move";
		jumpButton = "player0Jump";    //X
		attack1Button = "player0Fire1"; //square
		atack2Button = "player0Fire2";  //triangle
		
		if (controllerNum == 1){
			moveAxis = "player1Move";
			jumpButton = "player1Jump";    //X
			attack1Button = "player1Fire1"; //square
			atack2Button = "player1Fire2";  //triangle
			/*
			moveAxis = "Horizontal";
			jumpButton = "Jump";    
			attack1Button = "Fire1"; 
			atack2Button = "Fire2";  
			*/
		}
		
		facingDir = 1;
		
		
	}
	
	void reset(){
		speed = baseSpeed;
		health = baseHealth;
		
		invincibilityTimer = invincibilityTime;
		
		clearPowers();
		
		//give them a punch
		GameObject powerObject = Instantiate(punchPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
		Power thisPower = powerObject.GetComponent<Power>();
		thisPower.assignToPlayer(this);
		
		//set the pos
		transform.position = startPos;
		
		//clear velocity
		pushVel = new Vector3(0,0,0);
		curVel = new Vector3(0,0,0);
		
		numDoubleJumpsUsed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		//running
		float curSpeed = speed * Input.GetAxis(moveAxis);
		curVel.x = curSpeed;
		
		if (curSpeed > 0)  facingDir = 1;
		if (curSpeed < 0)  facingDir = -1;
		
		//jumping
		if (Input.GetButtonDown(jumpButton) && (controller.isGrounded || numDoubleJumpsUsed < numDoubleJumps)){
			startJump();
			if (!controller.isGrounded){
				numDoubleJumpsUsed++;
			}
		}
		if (Input.GetButtonUp(jumpButton)){
			endJump();
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
		
		if (controllerNum==0){
			//Debug.Log("ma vel "+curVel);
		}
		
		
		//check on powers
		for (int i=0; i<powers.Count; i++){
			powers[i].update();
		}
		
		//using powers
		if (Input.GetButtonDown(attack1Button)){
			for (int i=0; i<powers.Count; i++){
				powers[i].use();
			}
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
	
	void startJump(){
		isJumping = true;
		curVel.y = jumpPower;
	}
	
	void endJump(){
		if (curVel.y > jumpCut){
			curVel.y = jumpCut;
	    }
	    isJumping = false;
	}
	
	public void changeHealth(int amount, Player source){
		
		//don't take damage whil invicible
		if (amount < 0 && invincibilityTimer > 0){
			return;
		}
		
		health += amount;
		Debug.Log("ma health "+health);
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
	
	public void push(Vector3 power){
		pushVel += power;
	}
	
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
		Debug.Log("player "+controllerNum+" just got "+newPower.powerName);
		
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
		
		
		numDoubleJumps = 0;
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
	
	public int FacingDir {
		get {
			return this.facingDir;
		}
		set {
			facingDir = value;
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
	
	public float Speed {
		get {
			return this.speed;
		}
		set {
			speed = value;
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
	
	public int Score {
		get {
			return this.score;
		}
		set {
			score = value;
		}
	}
}
