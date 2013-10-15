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
	public List<Power> powers;
	public GameObject punchPowerObject;
	
	private int score;
	
	//general status things that items may need to affect
	[System.NonSerializedAttribute]
	public bool isPlayerControlled;
	[System.NonSerializedAttribute]
	public bool isGhost;
	[System.NonSerialized]
	public float fallingGrav;
	[System.NonSerialized]
	public float speed;
	
	public bool canPickupPowers;
	
	//general movement
	[System.NonSerialized]
	public Vector3 curVel;
	
	//moving left and right
	public float baseSpeed;
	
	//jumping
	public float jumpGrav, fallingGravBase;
	public float jumpPower;
	public float jumpCut;
	[System.NonSerializedAttribute]
	public bool isJumping;
	[System.NonSerializedAttribute]
	public int numDoubleJumpsUsed;
	[System.NonSerializedAttribute]
	public int numDoubleJumps;
	
	
	//general moving
	[System.NonSerializedAttribute]
	public Vector3 pushVel;
	public float pushFric;
	
	//being frozen
	private float freezeTimer;
	
	//recording movement
	[System.NonSerialized]
	public GhostRecorder recorder;
	
	public GameObject ghostPrefab;
	
	//showing stuff in HUD
	public float hudShakeTime;
	float hudShakeTimer;
	
	//all player characters must know where the star helm is
	[System.NonSerialized]
	public StarHelm starHelm;
	
	//spawn zone
	[System.NonSerialized]
	public GameObject spawnLeft, spawnRight;
	
	//link to GameManager and camera
	[System.NonSerialized]
	public GameManager gm;
	public CamControl camera;
	
	// Use this for initialization
	void Start () {
		avatar.renderer.material.color = myColor;
		startPos = transform.position;
		
		spawnLeft = GameObject.Find("spawnLeft");
		spawnRight = GameObject.Find("spawnRight");
		
		if (starHelm == null){
			starHelm = GameObject.FindWithTag("starHelm").GetComponent<StarHelm>();
		}
		
		//make some assumptions!
		isPlayerControlled = false;
		isGhost = true;
		
		customStart();
		
		reset();
		
		facingDir = 1;
	}
	
	public virtual void customStart(){}
	
	public void reset(){
		health = baseHealth;
		
		invincibilityTimer = invincibilityTime;
		
		freezeTimer = 0;
		
		customReset();
		
	}
	public virtual void customReset(){}
	
	// Update is called once per frame
	void Update () {
		
		if (freezeTimer > 0){
			freezeTimer-=Time.deltaTime;
			return;
		}
		
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
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		
	}
	public virtual void customUpdate(){}
	
	
	public void startJump(){
		isJumping = true;
		curVel.y = jumpPower;
	}
	
	public void endJump(){
		if (curVel.y > jumpCut){
			curVel.y = jumpCut;
	    }
	    isJumping = false;
	}
	
	public void push(Vector3 power){
		pushVel += power;
	}
	
	
	public void changeHealth(int amount, Player source){
		
		//ghosts can't hurt ghosts!
		if (!isPlayerControlled && !source.isPlayerControlled){
			return;
		}
		
		
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
			killPlayer(source);
		}
	}
	
	//by default, just kill the player, but allow child classes to define their own
	public void killPlayer(Player killer){
		killPlayerCustom(killer);
		
		//if this had the star helm, have the player spawn a new one
		if (killer != null && starHelm.ChosenOne == this){
			if (killer.isPlayerControlled){
				killer.starHelmScore();
			}
		}
	}
	public virtual void killPlayerCustom(Player killer){
		clearPowers();
		Destroy(gameObject);
	}
	
	public void freeze(float freezeTime){
		freezeTimer = freezeTime;
	}
	
		
	public void addScore(int val){
		score += val;
	}
	
	public void starHelmScore(){
		score += starHelm.scoreValue;
		
		PlayerGhost newGhost = makeGhost();
		starHelm.setChosenOne(newGhost);
		
		reset();
		
		//reset all ghosts
		GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ghost");
		for (int i=0; i<ghosts.Length; i++){
			ghosts[i].SendMessage("reset");
		}
		
		//destroy all effect objects
		GameObject[] effects = GameObject.FindGameObjectsWithTag("powerEffect");
		for (int i=0; i<effects.Length; i++){
			Destroy( effects[i] );
		}
		
		//show the text
		GameObject.FindGameObjectWithTag("statusText").SendMessage("showScoreText", score);
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
		
		//reset movement info
		speed = baseSpeed;
		fallingGrav = fallingGravBase;
		numDoubleJumpsUsed = 0;
		numDoubleJumps = 0;
	}
	
	public PlayerGhost makeGhost(){
		GameObject ghostObject = Instantiate(ghostPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		PlayerGhost newGhost = ghostObject.GetComponent<PlayerGhost>();
		newGhost.ghostSetup(myColor, recorder, powers, starHelm);
		
		gm.Ghosts.Add(newGhost);
		
		return newGhost;
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
	
	public GameManager Gm {
		get {
			return this.gm;
		}
		set {
			gm = value;
		}
	}
}
