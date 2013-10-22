using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectScreen : MonoBehaviour {
	
	public GameManager gm;
	
	public GameObject iconPrefab;
	
	public string[] levelNames;
	private LevelSelectIcon[] levelIcons;
	
	//placing the icons
	public int iconRowSize;
	public float iconStartY, iconYPadding, iconXSpacing;
	
	//selecting
	private int curSelection;
	
	//chekcing the mouse
	public Camera guiCam;
	public float mouseMoveThreshold;
	private Vector2 prevMousePos;
	
	public void reset(){
		gameObject.SetActive(true);
		
		//spawn some level icons!
		levelIcons = new LevelSelectIcon[levelNames.Length];
		for (int i=0; i<levelNames.Length; i++){
		
			int row = (int)Mathf.Floor( (float)i/(float)iconRowSize);
			int col = i%iconRowSize;
			
			Vector3 newPos = new Vector3(0,0,0);
			newPos.x = (col-1) * iconXSpacing;  //center around 0
			newPos.y = iconStartY + row*iconYPadding;
			
			GameObject newIconObj = Instantiate(iconPrefab, newPos, new Quaternion(0,0,0,0)) as GameObject;
			levelIcons[i] = newIconObj.GetComponent<LevelSelectIcon>();
			
			levelIcons[i].setup(i, levelNames[i], 666);
		}
		
		//set the first one as selected
		curSelection = -1;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		for (int i=0; i<levelNames.Length; i++){
		
			int row = (int)Mathf.Floor( (float)i/(float)iconRowSize);
			int col = i%iconRowSize;
			
			Vector3 newPos = new Vector3(0,0,0);
			newPos.x = (col-1) * iconXSpacing;  //center around 0
			newPos.y = iconStartY + row*iconYPadding;
			
			levelIcons[i].gameObject.transform.position = newPos;
		}
		*/
		
		
		//check the mouse. If it moved, see if it's over anything
		Vector2 curMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		if ( Mathf.Abs(curMousePos.x-prevMousePos.x) > mouseMoveThreshold || Mathf.Abs(curMousePos.y-prevMousePos.y) > mouseMoveThreshold){
			Ray ray;
			RaycastHit hit;
			
			ray = guiCam.ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(ray, out hit)) {
				
				//figure out if it was any of our levels
				for (int i=0; i<levelIcons.Length; i++){
					if (hit.transform == levelIcons[i].transform){
						setIconSelected(i);
					}
				}
				
			}
		}
		
		//clicking starts the game
		if (Input.GetMouseButtonUp(0) && curSelection != -1){
			finish();
		}
		
		
		//save the mouse position for next frame
		prevMousePos = curMousePos;
	}
	
	
	public void finish(){
		//destroy all icons
		for (int i=0; i<levelIcons.Length; i++){
			Destroy(levelIcons[i].gameObject);
		}
		
		//turn this off
		gameObject.SetActive(false);
		
		gm.setLevel(curSelection);
	}
	
	
	//turns on the selecte dicona nd turns off all others
	void setIconSelected(int num){
		for (int i=0; i<levelIcons.Length; i++){
			levelIcons[i].setSelected( i==num );
		}
		curSelection = num;
	}
}
