using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	
	public float explosionTime;
	public float explosionScale;
	//public GameObject trigger;
	
	private float timer;
	
	private Player owner;
	public bool isCloneKiller;
	
	public float rotationSpeed;
	
	//sound
	public AudioClip soundEffect;
	
	//tetsing clone kill
	private int dir;
	public float moveSpeed;

	// Use this for initialization
	void Start () {
		timer = explosionTime;
		
	}
	
	public void setOwner(Player _owner, bool _isCloneKiller){
		owner = _owner;
		owner.AudioController.Play(soundEffect);
		
		//isCloneKiller = _isCloneKiller;
	}
	
	public void setOwner(Player _owner){
		owner = _owner;
		owner.AudioController.Play(soundEffect);
		
		dir = owner.facingDir;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0){
			Destroy(gameObject);
		}
		
		float thisScale = (1-(timer/explosionTime)) * explosionScale;
		transform.localScale = new Vector3(thisScale, thisScale, thisScale);
		
		transform.localEulerAngles += new Vector3(0,0,rotationSpeed)*Time.deltaTime;
		
		if (isCloneKiller){
			transform.position += new Vector3(moveSpeed*Time.deltaTime*dir, 0,0);
		}
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("playerHitBox") ){
			//get the player
			//THIS CAN HIT THE OWNER BECAUSE IT'S A GODDAMN EXPLOSION
			//Ok, but the clone kill explosion cannot harm the player
			
			Player thisPlayer = other.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if ( !isCloneKiller || !thisPlayer.isPlayerControlled){
				thisPlayer.takeDamage(owner, isCloneKiller);
			}
		}
	}
}
