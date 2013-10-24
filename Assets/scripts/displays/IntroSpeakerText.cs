using UnityEngine;
using System.Collections;

public class IntroSpeakerText : MonoBehaviour {
	
	public tk2dTextMesh textSprite;
	
	public float letterTime;
	private float timer;
	
	private string targetString;
	private int curLetter;

	// Use this for initialization
	void Start () {
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
		timer = 0;
		char letterToAdd = targetString[curLetter];
		textSprite.text += letterToAdd;
		textSprite.Commit();
		curLetter++;
		
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
