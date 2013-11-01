using UnityEngine;
using System.Collections;

public class MuteButton : MonoBehaviour {
	
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
	}
	
	// Update is called once per frame
	void Update () {
		
		//check for clicks
		
		//check for button presses
		if (Input.GetKeyDown(KeyCode.M)){
			toggleMute();
		}
	
		transform.position = anchor.transform.position + offset;
	}
	
	public void toggleMute(){
	
		audioController.muted = !audioController.muted;
		
		sprite.spriteId = audioController.muted ? muteOnID : muteOffID;
		
	}
}
