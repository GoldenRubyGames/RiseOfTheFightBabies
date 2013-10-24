using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {
	
	GameManager gm;
	
	public PickupSpot  pickupSpot;
	
	public GameObject firstPowerObject, secondPowerObject;
	
	public IntroSpeakerText speakerText;
	
	private int phase;
	
	private float timer;
	
	// Use this for initialization
	void Start () {
		//find the game manager
		gm = GameObject.Find("gameManager").GetComponent<GameManager>();
		
		phase = -1;
		advancePhase();
		
		//speakerText.setNewText("Hello. How are you today?\nI'm good!");
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}
	
	
	void checkPhase(){
	
		if (phase == 0){
			if (timer > 3){
				advancePhase();
				return;
			}
		}
		
		
	}
	
	void advancePhase(){
		phase++;
		timer = 0;
		
		if (phase == 1){
			speakerText.setNewText("Thanks for coming in.\nWe've got a bit of an issue with the clones.");
		}
	}
}
