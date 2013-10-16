using UnityEngine;
using System.Collections;

public class StarHelm : MonoBehaviour {
	
	private Player chosenOne;
	
	public int scoreValue;
	
	public Vector3 offset;
	public float pulseSpeed;
	public float pulseMin, pulseMax;
	
	public Vector3 rotationSpeed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (chosenOne == null){
			Debug.Log("laugh at god");
			return;
		}
		
		float newScale = pulseMin + Mathf.Abs(Mathf.Sin( Time.time * pulseSpeed )) * (pulseMax-pulseMin);
		
		//transform.localScale = new Vector3(newScale, newScale, newScale);
		transform.position = chosenOne.transform.position + offset;
		
		//transform.localEulerAngles = new Vector3( Time.time*rotationSpeed.x, Time.time*rotationSpeed.y, Time.time*rotationSpeed.z);
	}
	
	public void setChosenOne(Player _chosenOne){
		chosenOne = _chosenOne;
	}
	
	
	//seters getters
	
	public Player ChosenOne {
		get {
			return this.chosenOne;
		}
		set {
			chosenOne = value;
		}
	}
}
