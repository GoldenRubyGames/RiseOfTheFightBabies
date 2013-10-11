using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct GhostDataPoint{
	public float time;
	public Vector3 vel;
	public int facingDir;
	public Vector3 pos;
	public bool attackPressed;
}

public class GhostRecorder {
	
	private List<GhostDataPoint> data;
	
	private float timer;
	
	int playHead;//where are we in the playback
	
	private Vector3 curVel;
	private int curFacingDir;
	private Vector3 curPos;
	
	private bool attackPressed;
	
	private int playbackDir;
	private float playbackSpeed;

	public GhostRecorder(){
		data = new List<GhostDataPoint>();
		reset(true);
	}
	
	public GhostRecorder(GhostRecorder orig){
		data = new List<GhostDataPoint>(orig.Data);
		reset(false);
	}
	
	public void reset(bool clearData){
		timer = 0;
		
		playHead = 0;
		
		playbackDir = 1;
		playbackSpeed = 1;
		
		if (clearData){
			data.Clear();
		}
	}
	
	public void record(Vector3 vel, int facingDir, Vector3 pos, bool attackPressed){
		timer += Time.deltaTime;
		
		//make a data point
		GhostDataPoint datum;
		datum.time = timer;
		datum.vel = vel;
		datum.facingDir = facingDir;
		datum.pos = pos;
		datum.attackPressed = attackPressed;
		
		//save it!
		data.Add(datum);
	}
	
	public void play(bool advanceTime){
		if (advanceTime){
			timer += Time.deltaTime * playbackSpeed * playbackDir;
		}
		
		//advance the playhead until it is current
		if (playbackDir == 1){
			while ( playHead<data.Count-1 && data[playHead].time < timer){
				playHead++;
				if (data[playHead].attackPressed){
					attackPressed = true;
				}
			}
		}else{
			while ( playHead>0 && data[playHead].time > timer){
				playHead--;
				if (data[playHead].attackPressed){
					attackPressed = true;
				}
			}
		}
		
		curVel = data[playHead].vel;
		curFacingDir = data[playHead].facingDir;
		curPos = data[playHead].pos;
	}
	
	public bool checkAttack(){
		bool returnVal = attackPressed;
		attackPressed = false;  //always turn this flag off after checking
	
		return returnVal;
	}
	
	public bool isAtEnd(){
		return playHead >= data.Count-1;
	}
	public bool isAtStart(){
		return playHead <= 0;
	}
	
	public void setPlaybackDir(int newDir){
		//make sure the value is 1 or -1
		if (newDir != 1 && newDir != -1){
			Debug.Log("SETPLAYBACKDIR MUST RECIEVE A VALUE OF 1 OR -1");
			return;
		}
		
		playbackDir = newDir;
	}
	
	public void setPlaybackSpeed(float newSpeed){
		//this must be a possitive value
		if (newSpeed < 0){
			Debug.Log("SETPLAYBACKSPEED MUST RECIEVE A POSSITIVE VALUE");
			return;
		}
		playbackSpeed = newSpeed;
	}
	
	//getters
	
	public Vector3 CurVel {
		get {
			return this.curVel;
		}
	}

	public int CurFacingDir {
		get {
			return this.curFacingDir;
		}
	}

	public Vector3 CurPos {
		get {
			return this.curPos;
		}
	}
		
	public List<GhostDataPoint> Data {
		get {
			return this.data;
		}
	}
	
	public int PlaybackDir {
		get {
			return this.playbackDir;
		}
	}
}
