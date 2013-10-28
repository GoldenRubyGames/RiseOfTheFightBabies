using UnityEngine;
using System.Collections;

public class KnifePower :  Power {

	public override void customUse(bool isCloneKiller){
		
		//spawn 2 knives 
		for (int i=0; i<2; i++){
			GameObject knifeObj = Instantiate( effectObject, owner.transform.position, new Quaternion(0,0,0,0)) as GameObject;
			knifeObj.GetComponent<KnifeEffect>().setup(owner, i==0);
		}
		
	}
}
