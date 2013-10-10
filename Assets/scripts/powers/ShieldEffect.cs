using UnityEngine;
using System.Collections;

public class ShieldEffect : MonoBehaviour {
	
	Player owner;
	
	public Vector3 offsetFromPlayer;
	

	public void setup(Player _owner){
		owner = _owner;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//keep it in place
		Vector3 thisOffset = offsetFromPlayer;
		thisOffset.x *= owner.FacingDir;
		transform.position = owner.transform.position + thisOffset;
	}
	
	public Player Owner {
		get {
			return this.owner;
		}
		set {
			owner = value;
		}
	}
}
