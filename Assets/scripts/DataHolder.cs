using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataHolder : MonoBehaviour {
	
	public int numLevels;
	
	private int[] highScores;

	// Use this for initialization
	public void setup () {
		highScores = new int[numLevels];
		
		loadData();
	}
	
	public void loadData(){
		
		//grab high scores
		for (int i=0; i<highScores.Length; i++){
			highScores[i] = PlayerPrefs.GetInt("level_"+i.ToString(), 0);
		}
		
	}
	
	public void setHighScore(int levelNum, int score){
		highScores[levelNum] = score;
		PlayerPrefs.SetInt("level_"+levelNum.ToString(), score);
		PlayerPrefs.Save();
	}
		
	public void clearData(){
		PlayerPrefs.DeleteAll();
	}
	
	
	//setters + getters
	
	public int[] HighScores {
		get {
			return this.highScores;
		}
	}
}
