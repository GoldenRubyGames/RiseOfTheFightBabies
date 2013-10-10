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
			timer += Time.deltaTime;
		}
		
		//advance the playhead until it is current
		while ( playHead<data.Count-1 && data[playHead].time < timer){
			playHead++;
			if (data[playHead].attackPressed){
				attackPressed = true;
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
	
	public bool isFinished(){
		return playHead >= data.Count-1;
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
}
