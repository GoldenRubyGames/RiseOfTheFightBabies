using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	//character controller
	public CharacterController controller;
	
	//showing the player
	public tk2dSprite avatar;
	public tk2dSpriteAnimator avatarAnimation;
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
	public GameObject startingPowerObject;
	
	//score
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
	//public CamControl camera;
	
	//animating
	private tk2dSpriteAnimationClip animStandingClip, animWalkingClip, animDyingClip, animKickingClip;
	private float velForWalkAnim = 1;
	public GunSprite gunSprite;
	private bool isKicking;
	private bool isGhostMelting;
	
	//sound
	AudioManager audioController;
	public AudioClip deathSound;
	public AudioClip jumpSound;
	
	//dying
	public GameObject deadPlayerPrefab;
	private bool isHidingSprite;
	
	// Use this for initialization
	void Start () {
		//avatar.renderer.material.color = myColor;
		startPos = transform.position;
		
		spawnLeft = GameObject.Find("spawnLeft");
		spawnRight = GameObject.Find("spawnRight");
		
		if (starHelm == null){
			starHelm = GameObject.FindWithTag("starHelm").GetComponent<StarHelm>();
		}
		
		//cache the animation IDs
		animStandingClip = avatarAnimation.GetClipByName("standing");
		animWalkingClip = avatarAnimation.GetClipByName("walking");
		animDyingClip = avatarAnimation.GetClipByName("dying");
		animKickingClip = avatarAnimation.GetClipByName("kicking");
		
		//make some assumptions!
		isPlayerControlled = false;
		isGhost = false;
		
		customStart();
		
		//reset();
		
		facingDir = 1;
	}
	
	public virtual void customStart(){}
	
	public void reset(){
		health = baseHealth;
		
		invincibilityTimer = invincibilityTime;
		
		freezeTimer = 0;
		
		//find the spawns if they haven't been located yet
		if (spawnLeft == null){
			spawnLeft = GameObject.Find("spawnLeft");
			spawnRight = GameObject.Find("spawnRight");
		}
		
		isHidingSprite = false;
		avatar.gameObject.SetActive(true);
		endKickAnimation();
		
		isGhostMelting = false;
		
		//do the custome reset for this type of player
		customReset();
		
		//go througn powers and turn the gun sprite on or off depending on if they have a gun power
		bool gunActiveState = false;
		for (int i=0; i<powers.Count; i++){
			if (powers[i].showGun){
				gunActiveState = true;
			}
		}
		gunSprite.gameObject.SetActive(gunActiveState);
		
	}
	public virtual void customReset(){}
	
	// Update is called once per frame
	void Update () {
		
		if (freezeTimer > 0){
			freezeTimer-=Time.deltaTime;
			controller.Move( new Vector3(0,0,0));//for some reaosn, I need to do this for the player to be able to be hurt
			return;
		}
		
		
		customUpdate();
		
		
		//check on powers
		for (int i=0; i<powers.Count; i++){
			powers[i].update();
		}
		
		//showing damage taken
		if (invincibilityTimer > 0 ){
			invincibilityTimer-=Time.deltaTime;
			bool showPlayer = Time.frameCount%10 < 5;
			avatar.gameObject.SetActive( showPlayer);
		}else if (avatar.gameObject.active == false){
			avatar.gameObject.SetActive(true);
		}
		
		hudShakeTimer -= Time.deltaTime;
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		
		//flip the sprite to face the right way
		avatar.FlipX = facingDir==-1;
		
		
		//set the animation
		if (!isGhostMelting){
			if (Mathf.Abs(curVel.x) > velForWalkAnim){
				if (avatarAnimation.CurrentClip != animWalkingClip){
					avatarAnimation.Play(animWalkingClip);
				}
			}else{
				avatarAnimation.Play(animStandingClip);
			}
			
			if (isKicking){
				avatarAnimation.Play(animKickingClip);
			}
		}
		
	}
	public virtual void customUpdate(){}
	
	
	public void startJump(){
		isJumping = true;
		curVel.y = jumpPower;
		
		if (isPlayerControlled){
			audioController.Play(jumpSound);
		}
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
	
	/*
	public void changeHealth(int amount, Player source){
		//ghosts can't hurt ghosts!
		if (!isPlayerControlled && !source.isPlayerControlled){
			return;
		}
		
		//don't take damage while invicible
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
	*/
	
	public void takeDamage(Player source){
		takeDamage(source, false);
	}
	
	public void takeDamage(Player source, bool cloneKiller){
		//ghosts can't hurt ghosts!
		if (!isPlayerControlled && !source.isPlayerControlled){
			return;
		}
		
		//don't take damage while invicible
		if (invincibilityTimer > 0){
			return;
		}
		
		health--;
		hudShakeTimer = hudShakeTime;
		
		//if they took damage make them invicible
		invincibilityTimer = invincibilityTime;
		
		//is this fucker dead?
		if (health <= 0){
			killPlayer(source, cloneKiller);
			
		}
	}
	
	//by default, just kill the player, but allow child classes to define their own
	public void killPlayer(Player killer, bool cloneKiller){
		if (gm != null){
			if (gm.DoingKillEffect){
				return;
			}
		}
		
		//instantiate a dead player object to take the fall
		GameObject deadPlayerObj = Instantiate(deadPlayerPrefab, transform.position, new Quaternion(0,0,0,0)) as GameObject;
		DeadPlayer deadPlayer = deadPlayerObj.GetComponent<DeadPlayer>();
		deadPlayer.setup( avatarAnimation.Library, killer, gunSprite.active, cloneKiller);
		
		//if this had the star helm, have the player spawn a new one
		if (killer != null && starHelm.ChosenOne == this){
			if (killer.isPlayerControlled){
				killer.starHelmScore();
				
				gm.startKillEffect(this, killer, cloneKiller);
				return;
			}
		}
		//otherwise just play the death sound{
		else{
			audioController.Play(deathSound);
		}
		
		
		killPlayerCustom(killer, cloneKiller);
	}
	
	public void startKickAnimation(float angle){
		isKicking = true;
		avatar.gameObject.transform.localEulerAngles = new Vector3(0,0, angle);
	}
	public void endKickAnimation(){
		isKicking = false;
		avatar.gameObject.transform.localEulerAngles = new Vector3(0,0, 0);
	}
	
	
	public virtual void killPlayerCustom(Player killer, bool cloneKiller){
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
		
		
		//show the text
		GameObject.FindGameObjectWithTag("statusText").SendMessage("showScoreText", score);
	}
	
	public virtual void activateCloneKill(){}
	
	
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
		
		//does this new power want us to show the gun?
		if (!gunSprite.gameObject.active && newPower.showGun){
			gunSprite.gameObject.SetActive(true);
		}
		
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
		newGhost.ghostSetup(myColor, recorder, powers, starHelm, gm, audioController);
		
		
		return newGhost;
	}
	
	public void hideSprite(){
		isHidingSprite = true;
		avatar.gameObject.SetActive(false);
		gunSprite.gameObject.SetActive(false);
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
	
	public float InvincibilityTimer {
		get {
			return this.invincibilityTimer;
		}
		set {
			invincibilityTimer = value;
		}
	}
	
	public bool IsGhostMelting {
		get {
			return this.isGhostMelting;
		}
		set {
			isGhostMelting = value;
		}
	}
	
	public AudioManager AudioController {
		get {
			return this.audioController;
		}
		set {
			audioController = value;
		}
	}
	
	
}
