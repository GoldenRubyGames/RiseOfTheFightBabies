using UnityEngine;
using System.Collections;

public class GunSprite : MonoBehaviour {
	
	public Player owner;
	
	public Vector3 offset;
	
	public tk2dSprite sprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.localPosition = new Vector3( offset.x*owner.facingDir, offset.y, offset.z);
		
		sprite.FlipX = owner.facingDir == -1;
	
	}
}
