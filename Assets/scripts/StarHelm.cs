using UnityEngine;
using System.Collections;

public class StarHelm : MonoBehaviour {
	
	private Player chosenOne;
	
	public int scoreValue;
	
	public Vector3 offset;
	
	//flying off in kill effect
	private bool doingKillEffect;
	private Vector3 deathEffectVel;
	public float flySpeed, angleSpeedBase;
	private float angleSpeed;
	public float deathEffectFric;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//ussualy just stay with the chosen player
		if (!doingKillEffect){
			if (chosenOne == null){
				//Debug.Log("laugh at god");
				return;
			}
			transform.position = chosenOne.transform.position + offset;	
		}
		//but during kill effect, fly the fuck away
		else{
			transform.position += deathEffectVel * Time.deltaTime;
		
			transform.localEulerAngles += new Vector3(0,0, angleSpeed * Time.deltaTime);
			
			//friction
			deathEffectVel *= Mathf.Pow(deathEffectFric, Time.deltaTime);
			angleSpeed *= Mathf.Pow(deathEffectFric, Time.deltaTime);
		}
	}
	
	public void setChosenOne(Player _chosenOne){
		chosenOne = _chosenOne;
		
		//reset anything that may have been changed during kill effect
		doingKillEffect = false;
		transform.localEulerAngles = new Vector3(0,0,0);
	}
	
	public void startKillEffect(Player killer){
		doingKillEffect = true;
		//set the vel based on the angle to the killer
		float angleToKiller = Mathf.Atan2( transform.position.y-killer.transform.position.y, transform.position.x-killer.transform.position.x);
		deathEffectVel = new Vector3(Mathf.Cos(angleToKiller) * flySpeed, Mathf.Sin(angleToKiller) * flySpeed,0);
		
		angleSpeed = angleSpeedBase;
		
		//cur angle is used to rotate the sprite
		if (deathEffectVel.x > 0 ){
			angleSpeed *= -1;
		}
		
	}
	
	
	//seters getters
	
	public Player ChosenOne {
		get {
			return this.chosenOne;
		}
		set {
			chosenOne = value;
		}
	}
}
