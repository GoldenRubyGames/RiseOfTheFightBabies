using UnityEngine;
using System.Collections;

public class MuteButton : MonoBehaviour {
	
	public Camera guiCam;
	public AudioManager audioController;
	
	public tk2dSprite sprite;
	
	public GameObject anchor;
	public Vector3 offset;
	
	private int muteOnID, muteOffID;
	
	// Use this for initialization
	void Start () {
		
		//cache the IDs
		muteOnID = sprite.GetSpriteIdByName("muteButtonOn");
		muteOffID = sprite.GetSpriteIdByName("muteButtonOff");
		
		//place it
		transform.position = anchor.transform.position + offset;
		
		turnOff();
	}
	
	// Update is called once per frame
	void Update () {
		
		//check for clicks
		if (Input.GetMouseButtonDown(0)){
			Ray ray;
			RaycastHit hit;
			
			ray = guiCam.ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(ray, out hit)) {
				//did they click us?????
				if (hit.transform == transform){
					toggleMute();
				}
			}
		}
		
		//check for button presses
		if (Input.GetKeyDown(KeyCode.M)){
			toggleMute();
		}
	
		//transform.position = anchor.transform.position + offset;
	}
	
	public void toggleMute(){
	
		audioController.muted = !audioController.muted;
		
		sprite.spriteId = audioController.muted ? muteOnID : muteOffID;
	}
	
	public void turnOn(){
		Debug.Log("turn me on og too sexy");
		renderer.enabled = true;
	}
	public void turnOff(){
		Debug.Log("my lust has gone away again");
		renderer.enabled = false;
	}
}
