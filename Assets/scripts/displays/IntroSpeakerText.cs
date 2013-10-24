using UnityEngine;
using System.Collections;

public class IntroSpeakerText : MonoBehaviour {
	
	public tk2dTextMesh textSprite;
	
	public float letterTime;
	public float lineBreakPause;
	private float timer;
	
	private string targetString;
	private int curLetter;

	// Use this for initialization
	void Start () {
		targetString = "";
		textSprite.text = "";
		textSprite.Commit();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (textSprite.text != targetString){
			
			timer += Time.deltaTime;
			if (timer >= letterTime){
				advanceText();
			}
			
		}
	}
	
	void advanceText(){
		char letterToAdd = targetString[curLetter];
		
		textSprite.text += letterToAdd;
		textSprite.Commit();
		curLetter++;
		
		timer = 0;
		//after a sentence, pause for longer
		if (letterToAdd=='.' || letterToAdd=='?' || letterToAdd=='!'){
			timer =  -lineBreakPause;
		}
		
		//skip spaces
		if (letterToAdd == ' '){
			advanceText();
		}
	}
	
	public void setNewText(string newText){
		timer = 0;
		curLetter = 0;
		
		targetString = newText;
		
		textSprite.text = "";
		textSprite.Commit();
	}
}
