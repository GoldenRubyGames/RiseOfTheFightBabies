using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour {
	
	public Camera cam;
	public GameManager gm;

	private Vector3 startPos;
	private float startZoom;
	
	public float zoomLerpSpeed, zoomLerpSpeedAfterKill;
	public float moveLerpSpeed, moveLerpSpeedAfterKill;
	public float timeLerpSpeed;
	
	private Vector3 targetPos;
	private float targetZoom;
	private float targetTimeScale;
	
	public float killEffectZoom;
	
	public float killEffectTimeScale;
	
	
	
	// Use this for initialization
	void Start () {
		
		startPos = transform.position;
		startZoom = cam.orthographicSize;
	
		hardReset();
	}
	
	public void hardReset(){
		reset();
		transform.position = startPos;
		cam.orthographicSize = startZoom;
		Time.timeScale = 1;
	}
	public void reset(){
		targetZoom = startZoom;
		targetPos = startPos;
		targetTimeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!gm.Paused){
			//lerp this som-bitch into place
			transform.position = Vector3.Lerp(transform.position, targetPos, gm.DoingKillEffect ? moveLerpSpeed : moveLerpSpeedAfterKill);
			
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, gm.DoingKillEffect ? zoomLerpSpeed : zoomLerpSpeedAfterKill);
			Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, timeLerpSpeed);
			
			//Debug.Log("cur time scale: "+Time.timeScale+"  target: "+targetTimeScale);
		}
		
	}
	
	public void setTargetPos(Vector3 newPos){
		setTargetPos(newPos.x, newPos.y);
	}
	public void setTargetPos(float x, float y){
		targetPos = new Vector3(x,y,startPos.z);
	}
	
	public void setTargetZoom(float newZoom){
		targetZoom = newZoom;
	}
	
	public void startKillEffect(Vector3 pos){
		setTargetPos(pos);
		setTargetZoom(killEffectZoom);
		
		targetTimeScale = killEffectTimeScale;
	}
	
}
