using UnityEngine;
using System.Collections;

public class PowerEffect : MonoBehaviour {
	
	Player owner;
	
	public void setup(Player _owner){
		owner = _owner;
		setupCustom();
	}
	
	public void setup(Player _owner, bool extraVal){
		owner = _owner;
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

}
