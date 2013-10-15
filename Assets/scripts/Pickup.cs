using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	
	GameObject powerObject;
	
	public TextMesh textObject;
	
	GameObject spawnPosObject;
	
	public float time;
	private float timer;
	
	public float flashTime;
	public float flashSpeed;
	
	public void setup(GameObject _powerObject, GameObject _spawnPosObject){
		spawnPosObject = _spawnPosObject;
		
		transform.position = spawnPosObject.transform.position;
		powerObject = _powerObject;
		
		textObject.text = powerObject.GetComponent<Power>().powerName;
		
		//if a power up already exists here, get rid of this thing
		GameObject[] otherPickups = GameObject.FindGameObjectsWithTag("pickup");
		for (int i=0; i<otherPickups.Length; i++){
			if (otherPickups[i] != gameObject && otherPickups[i].transform.position == transform.position){
				Destroy(gameObject);
				break;
			}
		}
		
		timer = time;
		
	}
	
	// Update is called once per frame
	void Update () {
		//lock z
		transform.position = new Vector3( transform.position.x, transform.position.y, 0);
		
		timer-=Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		
		//should we be flahsing?
		if (timer <= flashTime){
			renderer.enabled = (timer%flashSpeed) < flashSpeed/2;
		}
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			
			if (thisPlayer.canPickupPowers){
				//give them a power up!
				GameObject thisPower = Instantiate(powerObject, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
				thisPower.GetComponent<Power>().assignToPlayer(thisPlayer);
				
				//get rid of this
				Destroy(gameObject);
			}
		}
	}
}
