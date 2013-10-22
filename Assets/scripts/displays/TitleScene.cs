using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour {
	
	public float blinkSpeed;
	
	public GameObject clickToStartObj;
	
	
	public tk2dSpriteAnimator companyTitleAnim;
	public float companyAnimRestTime;
	private float companyAnimTimer;

	// Use this for initialization
	void Start () {
		companyAnimTimer = companyAnimTimer-0.7f;
	}
	
	// Update is called once per frame
	void Update () {
	
		clickToStartObj.SetActive( Time.time%blinkSpeed < blinkSpeed/2);
		
		companyAnimTimer += Time.deltaTime;
		if (companyAnimTimer > companyAnimRestTime){
			companyAnimTimer = 0;
			companyTitleAnim.Play();
		}
	}
}
