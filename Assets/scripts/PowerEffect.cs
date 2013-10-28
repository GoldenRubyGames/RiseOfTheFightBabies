using UnityEngine;
using System.Collections;

public class PowerEffect : MonoBehaviour {
	
	Player owner;
	bool isCloneKiller;
	
	public void setup(Player _owner, bool _isCloneKiller){
		owner = _owner;
		isCloneKiller = _isCloneKiller;
		setupCustom();
	}
	
	public void setup(Player _owner, bool _isCloneKiller, bool extraVal){
		owner = _owner;
		isCloneKiller = _isCloneKiller;
		setupCustom(extraVal);
	}
	
	public virtual void setupCustom(){}
	public virtual void setupCustom(bool extraVal){}
	 
	
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
