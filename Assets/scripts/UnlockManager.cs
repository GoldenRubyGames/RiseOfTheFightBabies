using UnityEngine;
using System.Collections;

public class UnlockManager : MonoBehaviour {
	
	public GameManager gm;
	
	private int numWeaponUnlocks;
	
	//private bool[] unlockState;
	private int[] unlockVals;
	
	private int weaponsUnlocked;
	private int nextUnlock;
	
	public GameObject popUpPrefab;
	
	private bool doneWithUnlocks;
	
	public void setup(){
		numWeaponUnlocks = gm.powerObjects.Length - gm.powerUnlockCutoff;
		Debug.Log("I got "+numWeaponUnlocks+" to unlock");
		
		unlockVals = new int[numWeaponUnlocks];
		
		for (int i=0; i<numWeaponUnlocks; i++){
			unlockVals[i] = getUnlockVal(i);
			//Debug.Log(i + " : " + getUnlockVal(i));
		}
		
		doneWithUnlocks = false;
		weaponsUnlocked = 0;
		nextUnlock = 0;
	}
	
	public void checkUnlocks(int cloneKills, bool showPopUp){
		
		for (int i=0; i<numWeaponUnlocks; i++){
			if ( cloneKills >= unlockVals[i] && weaponsUnlocked <= i ){
				weaponsUnlocked = i+1;
				nextUnlock = i+1;
				if (showPopUp){
					makePopUp(i);
					gm.PowerJustUnlocked = gm.powerUnlockCutoff + i;
					return;
				}
			}
		}
		
		if (nextUnlock >= numWeaponUnlocks){
			doneWithUnlocks = true;
			nextUnlock = 0;
		}
		
		Debug.Log("weapons unlocked: "+weaponsUnlocked);
		
	}
	
	int getUnlockVal(int num){
		
		float fNum = num+1;
		
		return 2+ 2*num;
		
		//return  (int)( 25.0f*Mathf.Pow( fNum, 1.25f) );
	}
	
	void makePopUp(int num){
		GameObject newPopUpObj = Instantiate(popUpPrefab, new Vector3(0,0,-2), new Quaternion(0,0,0,0)) as GameObject;
		UnlockPopUp newPopUp = newPopUpObj.GetComponent<UnlockPopUp>();
		int nextUnlockVal = 0;
		if (nextUnlock < numWeaponUnlocks){
			nextUnlockVal = unlockVals[nextUnlock];
		}
		string unlockName = gm.powerObjects[gm.powerUnlockCutoff + num].GetComponent<Power>().powerName;
		newPopUp.setup(unlockName, true, nextUnlockVal, num==numWeaponUnlocks-1, gm);
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
}
