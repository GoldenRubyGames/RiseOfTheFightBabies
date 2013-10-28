using UnityEngine;
using System.Collections;

public class PowerEffect : MonoBehaviour {
	
	Player owner;
	bool isCloneKiller;
	
	public void setup(Player _owner, bool _isCloneKiller){
		owner = _owner;
		isCloneKiller = _isCloneKiller;
	}
	
	public virtual void setupCustom(){}

	 
	
	public Player Owner {
		get {
			return this.owner;
		}
		set {
			owner = value;
		}
	}

	public bool IsCloneKiller {
		get {
			return this.isCloneKiller;
		}
		set {
			isCloneKiller = value;
		}
	}
}
