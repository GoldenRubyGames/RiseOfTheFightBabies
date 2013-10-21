using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	//camera
	public CamControl camera;
	
	//players
	public PlayerController[] players;
	private List<PlayerGhost> ghosts = new List<PlayerGhost>();
	private List<PlayerGoon> goons = new List<PlayerGoon>();
	
	//the level
	public int startingLevel;
	public GameObject[] levelObjects;
	private GameObject levelObject;
	
	//list of powers
	public GameObject[] powerObjects;
	public GameObject punchPowerObject;
	
	//pickup object
	private PickupSpot[] pickupSpots;
	//public GameObject pickupPrefab;
	//public GameObject[] pickupSpawnPoints;
	
	//pickup timing
	public float nextPickupTimeMin, nextPickupTimeMax;
	float pickupTimer;
	
	//goons
	public bool useGoons;
	public GameObject goonPrefab;
	public float minGoonTime, maxGoonTime;
	private float goonTimer;
	
	//the star
	public StarHelm starHelm;
	
	//game status
	private bool gameOver;
	
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

	// Use this for initialization
	void Start () {
		paused = false;
		
		setLevel(startingLevel);
		
		for (int i=0; i<players.Length; i++){
			players[i].Gm = this;
		}
		
		resetGame();
		
	}
	
	void resetGame(){
		gameOver = false;
		gameOverScreen.turnOff();
		
		starHelm.gameObject.SetActive(true);
		hud.gameObject.SetActive(true);
		
		//kill all existing pickups, goons and ghosts
		for (int i=0; i<pickupSpots.Length; i++){
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
		spawnPickup();
		pickupTimer = nextPickupTimeMin;
		
		//reset the players
		if (Time.frameCount > 2){
			for (int i=0; i<players.Length; i++){
				players[i].gameObject.SetActive(true);
				players[i].clearPowers();
				//players[i].reset();
				players[i].Score = 0;
				players[i].LivesLeft = players[i].numLives;
			}
		}
		
		
		//spawn one goon and give it the star helm
		spawnGoon();
		goonTimer = minGoonTime;
		starHelm.setChosenOne( goons[0] );
		
		doingKillEffect = false;
		
		resetRound();
	}
	
	void resetRound(){
		//reset players
		for (int i=0; i<players.Length; i++){
			players[i].reset();
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
		
		//reset HUD
		hud.reset();
		
		//put the camera back
		camera.reset();
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		//don't allow any input while title screen is up
		if (!pauseScreen.ShowingTitle){
			if (Input.GetKeyDown(KeyCode.V)){
				spawnPickup();
			}
			
			if (Input.GetKeyDown(KeyCode.R)){
				resetGame();
			}
			
			if (Input.GetKeyDown(KeyCode.G)){
				useGoons = !useGoons;
			}
			if (Input.GetKeyDown(KeyCode.T)){
				spawnGoon();
			}
			
			//switching levels
			if (Input.GetKeyDown(KeyCode.Alpha1)){
				setLevel(0);
				resetGame();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)){
				setLevel(1);
				resetGame();
			}
			
			//makeshift pause
			if (Input.GetButtonUp("pauseButton")){
				setPause(!paused, false);
			}
			//fire button can be used to unpause
			if (paused && Input.GetButtonUp("player0Fire1")){
				setPause(false, false);
			}
			//h calls up the rules
			if (Input.GetKeyDown(KeyCode.H)){
				setPause(true, true);
			}
		}
		
		//pause the game when it starts for now
		if (Time.frameCount == 2 && !pauseScreen.debugSkipTitle){
			setPause(true, true);
			pauseScreen.showTitle();
		}
		
		if (!gameOver && !doingKillEffect){
			//spawn pickups?
			pickupTimer -= Time.deltaTime;
			if (pickupTimer <= 0){
				spawnPickup();
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
				bool reasignStarHelm = starHelm.ChosenOne == killEffectFoe;
				
				PlayerGhost newGhost = players[0].makeGhost();
				ghosts.Add(newGhost);
				resetRound();
				
				
				killEffectFoe.killPlayerCustom(null);
				
				if (reasignStarHelm){
					starHelm.setChosenOne(newGhost);
				}
				
				doingKillEffect = false;
			}
		}
		
	}
	
	public void setPause(bool pauseGame, bool showHowTo){
		paused = pauseGame;
		if (paused){
			pauseScreen.activate( showHowTo );
		}else{
			pauseScreen.deactivate();
		}
		Time.timeScale = paused ? 0 : 1;
	}
	
	public void startKillEffect(Player freshlyKilled, Player killer){
		camera.startKillEffect(freshlyKilled.transform.position);
		
		doingKillEffect = true;
		killEffectTimer = killEffectTime;
		
		killEffectFoe = freshlyKilled;
		killEffectFoe.hideSprite();
		
		if (starHelm.ChosenOne == freshlyKilled){
			starHelm.startKillEffect(killer);
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
	
	void setLevel(int num){
		//destroy the current level if there is one
		if (levelObject != null){
			Destroy(levelObject);
		}
		
		//load the selected one
		levelObject = Instantiate(levelObjects[num], new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		
		//find all of the pickup spots
		GameObject[] pickupSpotObjects = GameObject.FindGameObjectsWithTag("pickupSpot");
		pickupSpots = new  PickupSpot[pickupSpotObjects.Length];
		for (int i=0; i<pickupSpotObjects.Length; i++){
			pickupSpots[i] = pickupSpotObjects[i].GetComponent<PickupSpot>();
		}
		
		Debug.Log("we got "+pickupSpots.Length+" spots");
		
	}
	
	void spawnPickup(){
		//select a power
		int powerID = (int)Random.Range(0, powerObjects.Length);
		//select a point
		int posNum = (int)Random.Range(0, pickupSpots.Length);
		
		if (!pickupSpots[posNum].IsActive){
			pickupSpots[posNum].activate( powerObjects[powerID] );
		}
		
	}
	
	void spawnGoon(){
		GameObject goonSpawnObj = Instantiate(goonPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		PlayerGoon goonSpawn = goonSpawnObj.GetComponent<PlayerGoon>();
		goonSpawn.gm = this;
		goons.Add(goonSpawn);
	}
	
	public void endGame(int score){
		gameOver = true;
		
		statusText.turnOff();
		gameOverScreen.turnOn(players[0].Score);
		//statusText.showEndGame(score);
		
		//destroy everything!
		for (int i=0; i<players.Length; i++){
			players[i].clearPowers();
			players[i].gameObject.SetActive(false);
		}
		
		for (int i=0; i<pickupSpots.Length; i++){
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
		
		hud.gameObject.SetActive(false);
		
		starHelm.gameObject.SetActive(false);
		
		
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
}
