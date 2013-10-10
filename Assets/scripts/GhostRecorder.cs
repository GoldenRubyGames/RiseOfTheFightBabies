using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct GhostDataPoint{
	public float time;
	public Vector3 vel;
	public int facingDir;
	public Vector3 pos;
}

public class GhostRecorder : MonoBehaviour {
	
	private List<GhostDataPoint> data = new List<GhostDataPoint>();
	
	private float timer;
	
	int playHead;//where are we in the playback
	
	private Vector3 curVel;
	private int curFacingDir;
	private Vector3 curPos;

	// Use this for initialization
	void Start () {
	
	}
	
	public void reset(bool clearData){
		timer = 0;
		
		playHead = 0;
		
		if (clearData){
			data.Clear();
		}
	}
	
	public void record(Vector3 vel, int facingDir, Vector3 pos){
		timer += Time.deltaTime;
		
		//make a data point
		GhostDataPoint datum;
		datum.time = timer;
		datum.vel = vel;
		datum.facingDir = facingDir;
		datum.pos = pos;
		
		//save it!
		data.Add(datum);
	}
	
	public void play(){
		timer += Time.deltaTime;
		
		//advance the playhead until it is current
		while ( playHead<data.Count-1 && data[playHead].time < timer){
			playHead++;
		}
		
		curVel = data[playHead].vel;
		curFacingDir = data[playHead].facingDir;
		curPos = data[playHead].pos;
		
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
}
