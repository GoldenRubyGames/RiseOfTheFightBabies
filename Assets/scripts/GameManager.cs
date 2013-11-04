using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public Kongregate kongregate;
	
	//camera
	public CamControl camera;
	
	
	//players
	public PlayerController[] players;
	private List<PlayerGhost> ghosts = new List<PlayerGhost>();
	private List<PlayerGoon> goons = new List<PlayerGoon>();
	
	//the level
	public GameObject[] levelObjects;
	private GameObject levelObject;
	private int curLevelNum;
	private bool levelJustUnlocked;
	
	//list of powers
	public GameObject[] powerObjects;
	public GameObject punchPowerObject;
	public int powerUnlockCutoff; //array location of the first unlockable power
	private int powerJustUnlocked;
	
	//pickup object
	private List<PickupSpot> pickupSpots = new List<PickupSpot>();
	public GameObject pickupSpotPrefab;
	//public GameObject[] pickupSpawnPoints;
	
	//pickup timing
	public float nextPickupTimeMin, nextPickupTimeMax;
	float pickupTimer;
	
	//clone kill
	public int minKillsForCloneKill, maxKillsForCloneKill;
	private int cloneKillCountDown;
	//private bool cloneKillHasBeenSpawned;
	
	//goons
	public bool useGoons;
	public GameObject goonPrefab;
	public float minGoonTime, maxGoonTime;
	private float goonTimer;
	
	//the star
	public StarHelm starHelm;
	
	//sound
	public AudioManager audioController;
	public MuteButton muteButton;
	public AudioClip gameStartSound;
	public AudioClip finishRoundSound;
	public AudioClip gameOverSound;
	public AudioClip menuBeep;
	
	//game status
	private bool gameOver;
	private string gameState;
	private int roundNum;
	
	//pausing
	public PauseScreen pauseScreen;
	private bool paused;
	
	//showing text
	public StatusText statusText;
	public GameOverScreen gameOverScreen;
	public HUD hud;
	
	//kill effect
	private bool doingKillEffect;
	public float killEffectTime;
	private float killEffectTimer;
	private Player killEffectFoe;
	private bool killEffectIsCloneKiller;
	
	public RoundFlash roundFlash;
	public AudioClip roundFlashSound;
	
	//rewind effect after kill effect
	private bool doingRewind;
	
	//other screens
	public LevelSelectScreen levelSelectScreen;
	public DataHolder dataHolder;
	public GameObject titleScreen;
	public UnlockManager unlockManager;
	
	private UnlockPopUp unlockScreenPopUp;
	
	//intro
	private bool doingIntro;
	
	//kongregate
	public GameObject kongManager;
	
	// Use this for initialization
	void Start () {
		titleScreen.SetActive(true);
		
		dataHolder.setup();
		unlockManager.setup();
		unlockManager.checkUnlocks(dataHolder.CloneKills, false);
		
		unlockScreenPopUp = null;
		
		powerJustUnlocked = -1;
		
		gameState = "title";
		//levelSelectScreen.reset();
			
		for (int i=0; i<players.Length; i++){
			players[i].Gm = this;
			players[i].AudioController = audioController;
		}
		
	}
	
	void resetGame(){
		gameState = "game";
		
		gameOver = false;
		gameOverScreen.turnOff();
		
		starHelm.gameObject.SetActive(true);
		hud.gameObject.SetActive(true);
		
		//kill all existing pickups, goons and ghosts
		for (int i=0; i<pickupSpots.Count; i++){
			pickupSpots[i].deactivate();
		}
		
		for (int i=0; i<goons.Count; i++){
			goons[i].clearPowers();
			Destroy(goons[i].gameObject);
		}
		goons.Clear();
		for (int i=0; i<ghosts.Count; i++){
			ghosts[i].clearPowers();
			Destroy(ghosts[i].gameObject);
		}
		ghosts.Clear();
		
		//give us a starting pickup
		/*
		if (!doingIntro){
			spawnPickup(true);
		}
		*/
		
		
		pickupTimer = nextPickupTimeMin;
		
		cloneKillCountDown = (int) Random.Range(minKillsForCloneKill, maxKillsForCloneKill);
		
		//reset the players
		for (int i=0; i<players.Length; i++){
			players[i].gameObject.SetActive(true);
			players[i].clearPowers();
			players[i].Score = 0;
			players[i].LivesLeft = players[i].numLives;
			if (doingIntro){
				Debug.Log("set the lives for intro");
				players[i].LivesLeft = 2;
			}
		}
		
		
		//spawn one goon and give it the star helm
		if (!doingIntro){
			spawnGoon();
			goonTimer = minGoonTime;
			starHelm.setChosenOne( goons[0] );
		}
		else{
			starHelm.gameObject.transform.position = new Vector3(0, 1000, 0);
		}
		
		
		roundNum = 1;
		
		doingKillEffect = false;
		
		muteButton.turnOff();
		
		audioController.playMusic(false);
		
		resetRound();
	}
	
	public void resetRound(){
		//reset players
		for (int i=0; i<players.Length; i++){
			players[i].reset();
			//if we're in the intro, put them on the far left
			if (doingIntro){
				players[i].transform.position = new Vector3(players[i].spawnLeft.transform.position.x, players[i].spawnLeft.transform.position.y, 0);
			}
		}
		
		//reset all ghosts
		for (int i=0; i<ghosts.Count; i++){
			ghosts[i].reset();
		}
		
		//reset all goons
		for (int i=0; i<goons.Count; i++){
			goons[i].reset();
		}
		
		//destroy all effect objects
		GameObject[] effects = GameObject.FindGameObjectsWithTag("powerEffect");
		for (int i=0; i<effects.Length; i++){
			Destroy( effects[i] );
		}
		
		//make sure there is a weapon by spawning one
		if (!doingIntro){
			spawnPickup(true);
		}
		
		//reset HUD
		hud.reset();
		
		roundFlash.activate();
		
		if (roundNum != 1){
			audioController.Play(roundFlashSound);
		}else{
			audioController.Play(gameStartSound);
		}
		
	}
	
	
	// Update is called once per frame
	void Update () {
		//kongManager.SendMessage("IsConnected");
		//testing
		if (Input.GetKeyDown(KeyCode.Alpha0)){
			dataHolder.clearData();
			if (gameState == "levelSelect"){
				levelSelectScreen.cleanUp();
			}
			goToLevelSelect();
		}
		if (Input.GetKeyDown(KeyCode.Y)){
			kongregate.toggleDebug();
		}
		
		//do things acording to game state
		if (gameState == "gameOver"){
			if (gameOverScreen.CanKill){
				if (Input.GetKeyDown(KeyCode.R) || Input.GetButton("player0Jump") || Input.GetButton("player0Fire1") || Input.GetButton("pauseButton")){
					if (!levelJustUnlocked && curLevelNum!=0){
						resetGame();
					}else{
						levelJustUnlocked = false;
						goToLevelSelect();
						audioController.Play(menuBeep);
					}
				}
				if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 6")){
					goToLevelSelect();
					audioController.Play(menuBeep);
				}
			}
		}
		else if (gameState == "title"){
			if (Input.GetMouseButtonUp(0) || Input.GetButton("player0Jump") || Input.GetButton("player0Fire1") || Input.GetButton("pauseButton") ){
				titleScreen.SetActive(false);
				goToLevelSelect();
				audioController.Play(menuBeep);
			}
		}
		else if (gameState == "levelSelect"){
			//most level select input is handled in the level select class
			if (Input.GetKeyDown(KeyCode.Q)){
				gameState = "title";
				levelSelectScreen.cleanUp();
				titleScreen.SetActive(true);
				audioController.Play(menuBeep);
			}
		}
		else if (gameState == "unlockPopUp"){
			if (Input.anyKey && unlockScreenPopUp.CanBeKilled){
				gameState = "gameOver";
				Destroy(unlockScreenPopUp.gameObject);
				unlockScreenPopUp = null;
				//see if there are any more
				bool moreUnlocks = unlockManager.checkUnlocks(dataHolder.CloneKills, true);
				//and if not, wake up the game over screen
				if (!moreUnlocks){
					audioController.Play(menuBeep);
					gameOverScreen.turnOn();
					gameOverScreen.setUnlockText(this);
				}
			}
		}
		else if (gameState == "game"){
			//don't allow any input while title screen is up
			if (!pauseScreen.ShowingTitle){
				if (Input.GetKeyDown(KeyCode.V)){
					spawnPickup(false);
				}
				
				//makeshift pause
				if (Input.GetButtonUp("pauseButton")){
					setPause(!paused, false);
				}
				//fire button can be used to unpause
				if (paused && Input.GetButtonUp("player0Fire1")){
					setPause(false, false);
				}
				//h calls up the rules (y on xbox)
				if (Input.GetKeyDown(KeyCode.H) || (paused && Input.GetKeyDown("joystick button 3")) ){
					setPause(true, true);
				}
				//Q ends the game (back on xbox)
				if (paused && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 6")) ){
					setPause(false, false, false);
					endGame(players[0].Score);
				}
				
				/*
				//dbeug stuff
				if (Input.GetKeyDown(KeyCode.G)){
					useGoons = !useGoons;
				}
				if (Input.GetKeyDown(KeyCode.T)){
					spawnGoon();
				}
				if (Input.GetKeyDown(KeyCode.C)){
					spawnCloneKill();
				}
				if (Input.GetKeyDown(KeyCode.R)){
					resetGame();
				}
				*/
			}
			
			
			if (!gameOver && !doingKillEffect && !doingIntro){
				//spawn pickups?
				pickupTimer -= Time.deltaTime;
				if (pickupTimer <= 0){
					spawnPickup(false);
					pickupTimer = Random.Range(nextPickupTimeMin, nextPickupTimeMax);
				}
				
				//spawn goons?
				if (useGoons){
					goonTimer -= Time.deltaTime;
					if (goonTimer <= 0){
						spawnGoon();
						goonTimer = Random.Range(minGoonTime, maxGoonTime);
					}
				}
			}
			
			if (doingKillEffect){
				//don't let this timer be affected by the time scale
				killEffectTimer -= Time.deltaTime*(1/Time.timeScale);
				
				//end the kill effect
				if (killEffectTimer <= 0){
					endKillEffect();
				}
			}
		}
	}
	
	public void setPause(bool pauseGame, bool showHowTo, bool playSound = true){
		paused = pauseGame;
		if (paused){
			pauseScreen.activate( showHowTo );
			muteButton.turnOn();
		}else{
			pauseScreen.deactivate();
			muteButton.turnOff();
		}
		Time.timeScale = paused ? 0 : 1;
		
		if (playSound){
			audioController.Play(menuBeep);
			if (paused){
				audioController.stopMusic();
			}else{
				audioController.playMusic(false);
			}
		}
	}
	
	public void addKill(){
		dataHolder.addCloneKill();
		cloneKillCountDown--;
		Debug.Log("kills left: "+cloneKillCountDown);
		if (cloneKillCountDown <= 0){
			spawnCloneKill();
		}
	}
	
	public void startKillEffect(Player freshlyKilled, Player killer, bool _killEffectIsCloneKiller){
		camera.startKillEffect(freshlyKilled.transform.position);
		
		doingKillEffect = true;
		killEffectTimer = killEffectTime;
		
		killEffectFoe = freshlyKilled;
		killEffectFoe.hideSprite();
		
		killEffectIsCloneKiller = _killEffectIsCloneKiller;
		
		if (starHelm.ChosenOne == freshlyKilled){
			starHelm.startKillEffect(killer);
			audioController.Play(finishRoundSound);
		}
		
		//stun everybody!
		for (int i=0; i<players.Length; i++){
			players[i].freeze(killEffectTime);
			//make sure the players are visible if they made the kill
			if (players[i] != freshlyKilled){
				players[i].avatar.gameObject.SetActive(true);
			}
		}
		for (int i=0; i<ghosts.Count; i++){
			ghosts[i].freeze(killEffectTime);
		}
		for (int i=0; i<goons.Count; i++){
			goons[i].freeze(killEffectTime);
		}
		
		//is the dead player doing any attacks that look bad during kill effect?
		for (int i=0; i<freshlyKilled.Powers.Count; i++){
			if (freshlyKilled.Powers[i].destroyOnDeath){
				freshlyKilled.Powers[i].customCleanUp();
			}
		}
	}
	
	public void endKillEffect(){
		bool reasignStarHelm = starHelm.ChosenOne == killEffectFoe;
					
		//make a ghost unless the player died during the intro
		if (!doingIntro || reasignStarHelm){
			PlayerGhost newGhost = players[0].makeGhost();
			ghosts.Add(newGhost);
			newGhost.gameObject.SetActive(false);
			
			if (reasignStarHelm){
				starHelm.setChosenOne(players[0]);
				//increase the round number
				roundNum++;
			}
		}
		
		//in the intro, make sure the player has a weapon
		if(doingIntro && killEffectFoe == players[0]){
			levelObject.SendMessage("playerDied");
		}
		
		killEffectFoe.killPlayerCustom(null, killEffectIsCloneKiller);
		
		//put the camera back
		camera.reset();
		
		//no effect if the player was killed
		if (!reasignStarHelm){
			resetRound();
		}
		//otherwise rewind them!
		else{
			doingRewind = true;
			players[0].startRewind();
			
			//destroy all effect objects
			GameObject[] effects = GameObject.FindGameObjectsWithTag("powerEffect");
			for (int i=0; i<effects.Length; i++){
				Destroy( effects[i] );
			}
			//turn off ghosts for now
			for (int i=0; i<ghosts.Count; i++){
				ghosts[i].gameObject.SetActive(false);
			}
		}
		
		
		doingKillEffect = false;
	}
	
	public void endRewind(){
		ghosts[ ghosts.Count-1 ].gameObject.SetActive(true);
		starHelm.setChosenOne (ghosts[ ghosts.Count-1 ]);
		
		//turn ghosts back on
		for (int i=0; i<ghosts.Count; i++){
			ghosts[i].gameObject.SetActive(true);
		}
		
		
		doingRewind = false;
		resetRound();
		
		//ghosts[ ghosts.Count-1 ].recorder.PlayHead = ghosts[ ghosts.Count-1 ].recorder.GroundedFrame;
	}
	
	public void setLevel(int num){
		//destroy the current level if there is one
		if (levelObject != null){
			Destroy(levelObject);
		}
		
		doingIntro = false; //assume this is not the intro level
		
		//load the selected one
		levelObject = Instantiate(levelObjects[num], new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		curLevelNum = num;
		
		//find all of the pickup spots and add them
		//levelObject.transform.chil
		GameObject[] pickupSpotObjects = GameObject.FindGameObjectsWithTag("pickupSpotMarker");
		pickupSpots.Clear();
		for (int i=0; i<pickupSpotObjects.Length; i++){
			//the game objects array will also have pickup spots from the level we just removed, so we need ot only take ones we want
			if (pickupSpotObjects[i].transform.parent == levelObject.transform){
				
				GameObject newPickupObj = Instantiate( pickupSpotPrefab, pickupSpotObjects[i].transform.position, new Quaternion(0,0,0,0) ) as GameObject;
				newPickupObj.transform.parent = levelObject.transform;
				Destroy(pickupSpotObjects[i]);
				
				pickupSpots.Add( newPickupObj.GetComponent<PickupSpot>() );
				pickupSpots[ pickupSpots.Count-1 ].Hud = hud;
			}
			
			
		}
		
		if (curLevelNum == 0){
			doingIntro = true;
			//set the pickups
			IntroManager thisIntro = levelObject.GetComponent<IntroManager>();
			thisIntro.pickupSpot = pickupSpots[0];
			thisIntro.pickupSpot2 = pickupSpots[1];
		}
		
		//reset the game
		resetGame();
		
	}
	
	void spawnPickup(bool forceWeapon){
		//select a power
		int powerID = 0;
		//select a point
		int posNum = (int)Random.Range(0, pickupSpots.Count);
		
		int numPowersAvailable = powerUnlockCutoff + unlockManager.WeaponsUnlocked;
		
		//make sure the power is not already being used
		bool alreadyUsed = true;
		while (alreadyUsed){
			alreadyUsed = false;
			powerID = (int)Random.Range(0, numPowersAvailable);
			for (int i=0; i<pickupSpots.Count; i++){
				if (pickupSpots[i].IsActive && pickupSpots[i].PowerObject == powerObjects[powerID]){
					alreadyUsed = true;
				}
			}
			
			//if we demand a weapon, treat it as being used already so we get a new one
			if (forceWeapon){
				if (!powerObjects[powerID].GetComponent<Power>().isAnAttack){
					alreadyUsed = true;
				}
			}
		}
		
		//if the player just unlocked something, show that
		if (powerJustUnlocked != -1){
			powerID = powerJustUnlocked;
			powerJustUnlocked = -1;
		}
		
		if (!pickupSpots[posNum].IsActive){
			pickupSpots[posNum].activate( powerObjects[powerID] );
		}
		
	}
	
	void spawnCloneKill(){
		//select a power
		int powerID = 0;
		//select a point
		int posNum = (int)Random.Range(0, pickupSpots.Count);
		
		//if it's taken, move through the list until we find one that isn't
		int numTries = 0;
		while (pickupSpots[posNum].IsActive && numTries<pickupSpots.Count){
			posNum++;
			if (posNum >= pickupSpots.Count){
				posNum -= pickupSpots.Count;
			}
			numTries++;
		}
		
		pickupSpots[posNum].activateCloneKill();	
		
		//set up the next one
		cloneKillCountDown = (int) Random.Range(minKillsForCloneKill, maxKillsForCloneKill);
		
		Debug.Log("SPAWN IT BUSTER");
	}
	
	public void spawnGoon(){
		GameObject goonSpawnObj = Instantiate(goonPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		PlayerGoon goonSpawn = goonSpawnObj.GetComponent<PlayerGoon>();
		goonSpawn.gm = this;
		goonSpawn.AudioController = audioController;
		goons.Add(goonSpawn);
	}
	
	public void removeGhost(PlayerGhost ghost){
		//Debug.Log("num ghosts before: "+ghosts.Count);
		ghosts.Remove(ghost);
		ghost.clearPowers();
		Destroy(ghost.gameObject);
		//Debug.Log("num ghosts after: "+ghosts.Count);
	}
	
	public void endGame(int score){
		gameOver = true;
		gameState = "gameOver";
		
		hud.reset();
		hud.gameObject.SetActive(false);
		statusText.turnOff();
		bool isHighscore = score > dataHolder.HighScores[curLevelNum];
		
		
		//destroy everything!
		for (int i=0; i<players.Length; i++){
			players[i].clearPowers();
			players[i].gameObject.SetActive(false);
		}
		
		for (int i=0; i<pickupSpots.Count; i++){
			pickupSpots[i].deactivate();
		}
		
		for (int i=0; i<goons.Count; i++){
			goons[i].clearPowers();
			Destroy(goons[i].gameObject);
		}
		goons.Clear();
		
		for (int i=0; i<ghosts.Count; i++){
			ghosts[i].clearPowers();
			Destroy(ghosts[i].gameObject);
		}
		ghosts.Clear();
		
		starHelm.gameObject.SetActive(false);
		
		//is this a new high score?
		if (score > dataHolder.HighScores[curLevelNum]){
			dataHolder.setHighScore(curLevelNum, score);
		}
		
		//turn this thing on
		gameOverScreen.turnOn(players[0].Score, roundNum, isHighscore, this);
		
		//was anything unlocked?
		bool stuffWasUnlocked = unlockManager.checkUnlocks(dataHolder.CloneKills, true);
		
		//try sending the score to Kongregate
		Kongregate.SubmitHighScore(curLevelNum, score);
		
		//play the sound
		if (!stuffWasUnlocked){
			audioController.Play(gameOverSound);
		}
		
		audioController.stopMusic();
		
		muteButton.turnOn();
		
		//save
		dataHolder.save();
	}
	
	public void goToLevelSelect(){
		gameState = "levelSelect";
		
		//make sure everything is off
		gameOverScreen.turnOff();
		
		Destroy(levelObject);
		
		//turn on the level select
		levelSelectScreen.reset();
		
		//make sure the pause screen is gone
		setPause(false, false, false);
		
		muteButton.turnOn();
	}
	
	
	public void setUnlockPopUpShowing(UnlockPopUp _unlockScreenPopUp){
		gameState = "unlockPopUp";
		unlockScreenPopUp = _unlockScreenPopUp;
		
		//turn off game over
		gameOverScreen.turnOff();
	}
	
	
	//setters getters
	
	public List<PlayerGhost> Ghosts {
		get {
			return this.ghosts;
		}
		set {
			ghosts = value;
		}
	}
	
	public List<PlayerGoon> Goons {
		get {
			return this.goons;
		}
		set {
			goons = value;
		}
	}
	
	public bool Paused {
		get {
			return this.paused;
		}
		set {
			paused = value;
		}
	}
	
	public bool DoingKillEffect {
		get {
			return this.doingKillEffect;
		}
		set {
			doingKillEffect = value;
		}
	}
	
	public int RoundNum {
		get {
			return this.roundNum;
		}
	}
	
	public UnlockPopUp UnlockScreenPopUp {
		get {
			return this.unlockScreenPopUp;
		}
	}
	
	public int PowerJustUnlocked {
		get {
			return this.powerJustUnlocked;
		}
		set {
			powerJustUnlocked = value;
		}
	}
	
	public bool LevelJustUnlocked {
		get {
			return this.levelJustUnlocked;
		}
		set {
			levelJustUnlocked = value;
		}
	}
	
	public bool DoingIntro {
		get {
			return this.doingIntro;
		}
		set {
			doingIntro = value;
		}
	}
	
	public int CurLevelNum {
		get {
			return this.curLevelNum;
		}
		set {
			curLevelNum = value;
		}
	}
}
