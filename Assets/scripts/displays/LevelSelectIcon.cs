using UnityEngine;
using System.Collections;

public class LevelSelectIcon : MonoBehaviour {
	
	private int levelNum;
	
	public tk2dTextMesh textSprite, unlockText;
	public GameObject borderNorm, borderHighlight;
	public tk2dSprite iconSprite;
	
	private bool isSelected;
	
	private string levelName;
	
	//unlocking levels
	private bool isLocked;
	public float lockedFadeAlpha;
	public float lockedTextAlpha;
	
	public void setup(int _levelNum, string _levelName, int highScore){
	
		levelNum = _levelNum;
		isLocked = false;
		
		levelName = _levelName;
		
		//set the icon
		iconSprite.SetSprite("levelIcon"+levelNum.ToString());
		
		//set the text
		textSprite.text = levelName +"\nBest: "+highScore.ToString();
		textSprite.Commit();
		
		unlockText.gameObject.SetActive(false);
		
		isSelected = false;
		
	}
	
	public void lockLevel(int scoreToUnlock, string prevLevelName){
		isLocked = true;
		
		//fade the image
		iconSprite.color = new Color(1,1,1, lockedFadeAlpha);
		
		//kill the score text
		textSprite.text = levelName;
		textSprite.color = new Color(0,0,0, lockedTextAlpha);
		textSprite.Commit();
		
		//show the unlock text
		unlockText.gameObject.SetActive(true);
		unlockText.text = "Score "+scoreToUnlock.ToString()+"\nIn "+prevLevelName+"\nto unlock";
		unlockText.Commit();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setSelected(bool _isSelected){
		isSelected = _isSelected;
		
		borderNorm.SetActive( !isSelected );
		borderHighlight.SetActive( isSelected );
	}
	
	
	
	
	public bool IsLocked {
		get {
			return this.isLocked;
		}
		set {
			isLocked = value;
		}
	}
}
