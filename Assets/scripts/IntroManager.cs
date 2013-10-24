using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {
	
	GameManager gm;
	
	public PickupSpot  pickupSpot;
	
	public GameObject firstPowerObject, secondPowerObject;
	
	private int phase;
	
	// Use this for initialization
	void Start () {
		
		//find the game manager
		gm = GameObject.Find("gameManager").GetComponent<GameManager>();
		
		
		phase = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
