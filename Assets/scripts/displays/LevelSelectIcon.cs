using UnityEngine;
using System.Collections;

public class LevelSelectIcon : MonoBehaviour {
	
	private int levelNum;
	
	public tk2dTextMesh textSprite;
	public tk2dSlicedSprite borderSprite;
	public tk2dSprite iconSprite;
	
	private bool isSelected;
	
	private int borderNormId, borderHighlightedId;
	
	
	
	public void setup(int _levelNum, string levelName, int highScore){
	
		levelNum = _levelNum;
		
		//set the icon
		iconSprite.SetSprite("levelIcon"+levelNum.ToString());
		
		//set the text
		textSprite.text = levelName +"\nBest: "+highScore.ToString();
		textSprite.Commit();
		
		isSelected = false;
		
		borderNormId = borderSprite.GetSpriteIdByName("levelSelectBox");
		borderHighlightedId = borderSprite.GetSpriteIdByName("levelSelectBoxHighlight");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setSelected(bool _isSelected){
		isSelected = _isSelected;
		
		borderSprite.spriteId = isSelected ? borderHighlightedId : borderNormId;
		//make sure the padding is right
		int borderVal = isSelected ? 4 : 2;
		borderSprite.borderLeft = borderVal;
		borderSprite.borderRight = borderVal;
		borderSprite.borderTop = borderVal;
		borderSprite.borderBottom = borderVal;
		
	}
}
