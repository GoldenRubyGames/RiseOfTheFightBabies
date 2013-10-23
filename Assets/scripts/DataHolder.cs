using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataHolder : MonoBehaviour {
	
	public int numLevels;
	
	private int[] highScores;
	
	private int cloneKills;

	// Use this for initialization
	public void setup () {
		highScores = new int[numLevels];
		
		loadData();
	}
	
	public void save(){
		Debug.Log("pave my save");
		PlayerPrefs.Save();
	}
	
	public void loadData(){
		
		//grab high scores
		for (int i=0; i<highScores.Length; i++){
			highScores[i] = PlayerPrefs.GetInt("level_"+i.ToString(), 0);
		}
		
		//grab kill count
		cloneKills = PlayerPrefs.GetInt("cloneKills", 0);
		
	}
	
	public void setHighScore(int levelNum, int score){
		highScores[levelNum] = score;
		PlayerPrefs.SetInt("level_"+levelNum.ToString(), score);
	}
	
	public void addCloneKill(){
		cloneKills++;
		PlayerPrefs.SetInt("cloneKills", cloneKills);
	}
		
	public void clearData(){
		PlayerPrefs.DeleteAll();
		loadData();
		Debug.Log("data is dead");
	}
	
	
	//setters + getters
	
	public int[] HighScores {
		get {
			return this.highScores;
		}
	}
	
	public int CloneKills {
		get {
			return this.cloneKills;
		}
	}
}
