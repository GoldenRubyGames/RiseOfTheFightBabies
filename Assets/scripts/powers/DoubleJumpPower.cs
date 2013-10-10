using UnityEngine;
using System.Collections;

public class DoubleJumpPower : Power {

	public override void customReset(){
		owner.NumDoubleJumps++;
	}
}
