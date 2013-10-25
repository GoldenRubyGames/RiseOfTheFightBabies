using UnityEngine;
using System.Collections;

public class UnlockManager : MonoBehaviour {
	
	public bool debugUnlockAll;
	public bool debugUnlockFirstLevel;
	
	public GameManager gm;
	public DataHolder dataHolder;
	
	private int numWeaponUnlocks;
	
	private int[] unlockVals;
	
	private int weaponsUnlocked;
	private int nextUnlock;
	
	public int[] levelUnlockScores;
	private bool[] levelUnlocks;
	
	public GameObject popUpPrefab;
	
	private bool doneWithUnlocks;
	
	public void setup(){
		numWeaponUnlocks = gm.powerObjects.Length - gm.powerUnlockCutoff;
		
		unlockVals = new int[numWeaponUnlocks];
		
		for (int i=0; i<numWeaponUnlocks; i++){
			Debug.Log("its "+getUnlockVal(i));
			unlockVals[i] = getUnlockVal(i);
		}
		
		levelUnlocks = new bool[levelUnlockScores.Length];
		for (int i=0; i<levelUnlocks.Length; i++){
			levelUnlocks[i] = false;
		}
		
		if (debugUnlockFirstLevel){
			levelUnlockScores[0] = 0;
			levelUnlocks[0] = true;
		}
		
		doneWithUnlocks = false;
		weaponsUnlocked = 0;
		nextUnlock = 0;
		
		if (debugUnlockAll){
			unlockAll();
		}
	}
	
	public void checkUnlocks(int cloneKills, bool showPopUp){
		
		for (int i=0; i<numWeaponUnlocks; i++){
			if ( cloneKills >= unlockVals[i] && weaponsUnlocked <= i ){
				weaponsUnlocked = i+1;
				nextUnlock = i+1;
				if (showPopUp){
					makeWeaponPopUp(i);
					gm.PowerJustUnlocked = gm.powerUnlockCutoff + i;
					return;
				}
			}
		}
		
		if (nextUnlock >= numWeaponUnlocks){
			doneWithUnlocks = true;
			nextUnlock = 0;
		}
		
		//check for new levels too
		for (int i=0; i<levelUnlocks.Length; i++){
			if ( dataHolder.HighScores[i] >= levelUnlockScores[i]){
				if (levelUnlocks[i] == false){
					levelUnlocks[i] = true;
					if (showPopUp){
						makeLevelPopUp(i);
						gm.PowerJustUnlocked = gm.powerUnlockCutoff + i;
						gm.LevelJustUnlocked = true;
						return;
					}
				}
			}
		}
		
		
	}
	
	int getUnlockVal(int num){
		
		float fNum = num+1;
		
		//return 2+ 2*num;
		
		return  (int)( 20.0f*Mathf.Pow( fNum, 1.25f) );
	}
	
	void makeWeaponPopUp(int num){
		GameObject newPopUpObj = Instantiate(popUpPrefab, new Vector3(0,0,-2), new Quaternion(0,0,0,0)) as GameObject;
		UnlockPopUp newPopUp = newPopUpObj.GetComponent<UnlockPopUp>();
		int nextUnlockVal = 0;
		if (nextUnlock < numWeaponUnlocks){
			nextUnlockVal = unlockVals[nextUnlock];
		}
		string unlockName = gm.powerObjects[gm.powerUnlockCutoff + num].GetComponent<Power>().powerName;
		newPopUp.setup(unlockName, true, nextUnlockVal, num==numWeaponUnlocks-1, gm);
	}
	
	void makeLevelPopUp(int num){
		GameObject newPopUpObj = Instantiate(popUpPrefab, new Vector3(0,0,-2), new Quaternion(0,0,0,0)) as GameObject;
		UnlockPopUp newPopUp = newPopUpObj.GetComponent<UnlockPopUp>();
		
		string unlockName = gm.levelSelectScreen.levelNames[num+1];
		newPopUp.setup(unlockName, false, 0, num==numWeaponUnlocks-1, gm);
	}
	
	
	public void unlockAll(){
		doneWithUnlocks = true;
		nextUnlock = 0;
		
		weaponsUnlocked = numWeaponUnlocks;
		
		for (int i=0; i<levelUnlocks.Length; i++){
			levelUnlocks[i] = true;
		}
	}
	
	//setters getters
	

	public int[] UnlockVals {
		get {
			return this.unlockVals;
		}
		set {
			unlockVals = value;
		}
	}

	public int NextUnlock {
		get {
			return this.nextUnlock;
		}
		set {
			nextUnlock = value;
		}
	}
	
	public bool DoneWithUnlocks {
		get {
			return this.doneWithUnlocks;
		}
		set {
			doneWithUnlocks = value;
		}
	}
	
	public int WeaponsUnlocked {
		get {
			return this.weaponsUnlocked;
		}
		set {
			weaponsUnlocked = value;
		}
	}
	
	public bool[] LevelUnlocks {
		get {
			return this.levelUnlocks;
		}
	}
}
