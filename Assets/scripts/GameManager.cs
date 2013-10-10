using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public Player[] players;
	
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
	public GameObject goonPrefab;
	public float minGoonTime, maxGoonTime;
	private float goonTimer;

	// Use this for initialization
	void Start () {
		
		reset();
		
	}
	
	void reset(){
		
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
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Space)){
			spawnPickup();
		}
		
		if (Input.GetKeyDown(KeyCode.R)){
			reset();
		}
		
		//spawn pickups?
		pickupTimer -= Time.deltaTime;
		if (pickupTimer <= 0){
			spawnPickup();
			pickupTimer = Random.Range(nextPickupTimeMin, nextPickupTimeMax);
		}
		
		//spawn goons?
		goonTimer -= Time.deltaTime;
		if (goonTimer <= 0){
			spawnGoon();
			goonTimer = Random.Range(minGoonTime, maxGoonTime);
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
		Instantiate(goonPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0));
	}
}
