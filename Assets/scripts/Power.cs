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
	
	public bool destroyOnDeath;  //things like the uppercut should not stick around during kill effect
	
	//sounds!
	AudioManager audioController;
	public AudioClip activationSoundPlayer;
	public AudioClip activationSoundGhost;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public bool assignToPlayer(Player player){
		owner = player;
		if (owner.getPower(this)){
			reset();
			audioController = player.AudioController;
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
	
	public bool use(){
		if (canUse){
			customUse();
			playSound();
			coolDownTimer = coolDownTime;
			return true;
		}else{
			return false;
		}
	}
	
	public void playSound(){
		if (owner.isPlayerControlled && activationSoundPlayer != null){
			audioController.Play(activationSoundPlayer);
		}
		else if (!owner.isPlayerControlled && activationSoundGhost != null){
			audioController.Play(activationSoundGhost);
		}
	}
	
	public virtual void customUse(){}
	
	public void cleanUp(){
		customCleanUp();
		Destroy(gameObject);
	}
	public virtual void customCleanUp(){}
	
}
