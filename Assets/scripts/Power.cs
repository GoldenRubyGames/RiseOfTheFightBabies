using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour {
	
	public GameObject effectObject;
	
	public string powerName;
	
	public bool isAnAttack;
	public bool canStack;
	
	public bool showGun;
	
	[System.NonSerialized]
	public Player owner;
	
	public float coolDownTime;
	[System.NonSerialized]
	public float coolDownTimer;
	
	[System.NonSerialized]
	public bool canUse;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public bool assignToPlayer(Player player){
		owner = player;
		if (owner.getPower(this)){
			reset();
			return true;
		}else{
			Destroy(gameObject);
		}
		
		return false;
	}
	
	public void reset(){
		coolDownTimer = 0;
		customReset();
	}
	public virtual void customReset(){}
	
	public void update (){
		coolDownTimer -= Time.deltaTime;
		canUse = coolDownTimer <= 0;
	}
	
	public void use(){
		if (canUse){
			customUse();
			coolDownTimer = coolDownTime;
		}else{
			//Debug.Log("fuckin wait "+owner.controllerNum);
		}
	}
	
	public virtual void customUse(){}
	
	public void cleanUp(){
		customCleanUp();
		Destroy(gameObject);
	}
	public virtual void customCleanUp(){}
	
}
