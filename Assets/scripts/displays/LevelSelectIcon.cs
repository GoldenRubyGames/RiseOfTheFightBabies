using UnityEngine;
using System.Collections;

public class LevelSelectIcon : MonoBehaviour {
	
	private int levelNum;
	
	public tk2dTextMesh textSprite;
	public GameObject borderNorm, borderHighlight;
	public tk2dSprite iconSprite;
	
	private bool isSelected;
	
	
	
	
	public void setup(int _levelNum, string levelName, int highScore){
	
		levelNum = _levelNum;
		
		//set the icon
		iconSprite.SetSprite("levelIcon"+levelNum.ToString());
		
		//set the text
		textSprite.text = levelName +"\nBest: "+highScore.ToString();
		textSprite.Commit();
		
		isSelected = false;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setSelected(bool _isSelected){
		isSelected = _isSelected;
		
		borderNorm.SetActive( !isSelected );
		borderHighlight.SetActive( isSelected );
		
		
	}
}
