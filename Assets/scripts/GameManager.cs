using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	//camera
	public CamControl camera;
	
	//players
	public Player[] players;
	private List<PlayerGhost> ghosts = new List<PlayerGhost>();
	private List<PlayerGoon> goons = new List<PlayerGoon>();
	
	//list of powers
	public GameObject[] powerObjects;
	public GameObject punchPowerObject;
	
	//pickup object
	public GameObject pickupPrefab;
	public GameObject[] pickupSpawnPoints;
	
	//pickup timing
	public float nextPickupTimeMin, nextPickupTimeMax;
	float pickupTimer;
	
	//goons
	public bool useGoons;
	public GameObject goonLabelText;
	public GameObject goonPrefab;
	public float minGoonTime, maxGoonTime;
	private float goonTimer;
	
	//the star
	public StarHelm starHelm;
	
	//game status
	private bool gameOver;
	
	private bool paused;
	
	//showing text
	public StatusText statusText;
	
	//kill effect
	private bool doingKillEffect;
	public float killEffectTime;
	private float killEffectTimer;
	private Player killEffectFoe;

	// Use this for initialization
	void Start () {
		paused = false;
		
		reset();
		
		goonLabelText.SetActive(!useGoons);
		
		for (int i=0; i<players.Length; i++){
			players[i].Gm = this;
		}
		
	}
	
	void reset(){
		gameOver = false;
		
		//kill all existing pickups and goons
		GameObject[] pickups = GameObject.FindGameObjectsWithTag("pickup");
		for (int i=0; i<pickups.Length; i++){
			Destroy(pickups[i]);
		}
		GameObject[] goons = GameObject.FindGameObjectsWithTag("goon");
		for (int i=0; i<goons.Length; i++){
			Destroy(goons[i]);
		}
		
		spawnPickup();
		
		pickupTimer = nextPickupTimeMin;
		
		if (Time.frameCount > 2){
			for (int i=0; i<players.Length; i++){
				players[i].reset();
			}
		}
		
		goonTimer = minGoonTime;
		
		//spawn one goon and give it the star helm
		spawnGoon();
		starHelm.setChosenOne( GameObject.FindGameObjectWithTag("goon").GetComponent<PlayerGoon>() );
		
		doingKillEffect = false;
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
		
		//put the camera back
		camera.reset();
		
	}
	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.V)){
			spawnPickup();
		}
		
		if (Input.GetKeyDown(KeyCode.R)){
			reset();
		}
		
		if (Input.GetKeyDown(KeyCode.G)){
			useGoons = !useGoons;
			goonLabelText.SetActive(!useGoons);
		}
		if (Input.GetKeyDown(KeyCode.T)){
			spawnGoon();
		}
		
		//makeshift pause
		if (Input.GetButtonUp("pauseButton")){
			paused = !paused;
			Time.timeScale = paused ? 0 : 1;
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
	
	public void startKillEffect(Player freshlyKilled){
		camera.startKillEffect(freshlyKilled.transform.position);
		
		doingKillEffect = true;
		killEffectTimer = killEffectTime;
		
		killEffectFoe = freshlyKilled;
		
		//stun everybody!
		for (int i=0; i<players.Length; i++){
			players[i].freeze(killEffectTime);
		}
		for (int i=0; i<ghosts.Count; i++){
			ghosts[i].freeze(killEffectTime);
		}
		for (int i=0; i<goons.Count; i++){
			goons[i].freeze(killEffectTime);
		}
	}
	
	void spawnPickup(){
		//select a power
		int powerID = (int)Random.Range(0, powerObjects.Length);
		//powerID = powerObjects.Length-1;   //testing
		//select a point
		int posNum = (int)Random.Range(0,pickupSpawnPoints.Length);
		
		GameObject newPickupObj = Instantiate(pickupPrefab, pickupSpawnPoints[posNum].transform.position, new Quaternion(0,0,0,0)) as GameObject;
		newPickupObj.GetComponent<Pickup>().setup( powerObjects[powerID], pickupSpawnPoints[posNum]);
	}
	
	void spawnGoon(){
		GameObject goonSpawnObj = Instantiate(goonPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		PlayerGoon goonSpawn = goonSpawnObj.GetComponent<PlayerGoon>();
		goonSpawn.gm = this;
		goons.Add(goonSpawn);
	}
	
	public void endGame(int score){
		gameOver = true;
		
		statusText.showEndGame(score);
		
		//destroy everything!
		for (int i=0; i<players.Length; i++){
			players[i].clearPowers();
			Destroy(players[i].gameObject);
		}
		
		GameObject[] pickups = GameObject.FindGameObjectsWithTag("pickup");
		for (int i=0; i<pickups.Length; i++){
			Destroy(pickups[i]);
		}
		GameObject[] goons = GameObject.FindGameObjectsWithTag("goon");
		for (int i=0; i<goons.Length; i++){
			goons[i].SendMessage("clearPowers");
			Destroy(goons[i]);
		}
		GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ghost");
		for (int i=0; i<ghosts.Length; i++){
			ghosts[i].SendMessage("clearPowers");
			Destroy(ghosts[i]);
		}
		
		GameObject.Find("HUD").SetActive(false);
		
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
