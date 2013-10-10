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

	// Use this for initialization
	void Start () {
		
		//give both players a punch
		/*
		for (int i=0; i<players.Length; i++){
			GameObject powerObject = Instantiate(punchPowerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0) ) as GameObject;
			Power thisPower = powerObject.GetComponent<Power>();
			thisPower.assignToPlayer(players[i]);
		}
		*/
		
		spawnPickup();
		
		pickupTimer = nextPickupTimeMin;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Space)){
			spawnPickup();
		}
		
		pickupTimer -= Time.deltaTime;
		if (pickupTimer <= 0){
			spawnPickup();
			pickupTimer = Random.Range(nextPickupTimeMin, nextPickupTimeMax);
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
}
