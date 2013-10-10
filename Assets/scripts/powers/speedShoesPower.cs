using UnityEngine;
using System.Collections;

public class speedShoesPower : Power {
	
	public float increase;
	
	public override void customReset(){
		owner.Speed *= increase;
	}
}
