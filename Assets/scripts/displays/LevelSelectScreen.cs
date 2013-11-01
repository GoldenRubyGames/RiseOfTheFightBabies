using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectScreen : MonoBehaviour {
	
	public GameManager gm;
	public DataHolder dataHolder;
	public UnlockManager unlockManager;
	
	public GameObject iconPrefab;
	
	public tk2dTextMesh killText, nextUnlockText;
	
	public string[] levelNames;
	private LevelSelectIcon[] levelIcons;
	
	//placing the icons
	public int iconRowSize;
	public float iconStartY, iconYPadding, iconXSpacing;
	
	//selecting
	private int curSelection;
	public float minTimeOnScreen;
	private float timer;
	
	//chekcing the mouse
	public Camera guiCam;
	public float mouseMoveThreshold;
	private Vector2 prevMousePos;
	private bool usingMouse;
	
	//joystick controls
	public float joystickThreshold;
	private bool canPressUp, canPressDown, canPressLeft, canPressRight;
	
	public void reset(){
		gameObject.SetActive(true);
		
		canPressUp = false;
		canPressDown = false;
		canPressLeft = false;
		canPressRight = false;
		
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
			
			levelIcons[i].setup(i, levelNames[i], dataHolder.HighScores[i]);
			
			//lock them if they are locked
			if (i!=0){
				if (!unlockManager.LevelUnlocks[i-1]){
					levelIcons[i].lockLevel( unlockManager.levelUnlockScores[i-1], levelNames[i-1]);
				}
			}
					
		}
		
		
		//set the bottom text
		killText.text = "Total Ghost Kills: "+dataHolder.CloneKills.ToString();
		killText.Commit();
		if (!unlockManager.DoneWithUnlocks){
			nextUnlockText.text = "Next Unlock: "+unlockManager.UnlockVals[ unlockManager.NextUnlock ];
		}else{
			nextUnlockText.text = "Next Unlock: NEVER";
		}
		nextUnlockText.Commit();
		
		//set the level as the most recently unlocked
		setIconSelected( unlockManager.LevelUnlocks[0] ? 1 : 0 );
		
		prevMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		
		usingMouse = Input.GetJoystickNames().Length == 0;
		
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//clicking or pressing start/jump starts the game
		if (timer>=minTimeOnScreen && (Input.GetMouseButtonUp(0) || Input.GetButton("player0Fire1") || (Input.GetButton("player0Jump") && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)) || Input.GetButton("pauseButton")) && curSelection != -1){
			finish();
		}
		timer += Time.deltaTime;
		
		
		for (int i=0; i<levelNames.Length; i++){
		
			int row = (int)Mathf.Floor( (float)i/(float)iconRowSize);
			int col = i%iconRowSize;
			
			Vector3 newPos = new Vector3(0,0,0);
			newPos.x = (col-1) * iconXSpacing;  //center around 0
			newPos.y = iconStartY + row*iconYPadding;
			
			levelIcons[i].gameObject.transform.position = newPos;
		}
		
		
		//check for joystick input
		float xAxisInput = Input.GetAxis("Horizontal");
		float yAxisInput = Input.GetAxis("Vertical");
		
		if (xAxisInput < joystickThreshold)   canPressRight = true;
		if (xAxisInput > -joystickThreshold)  canPressLeft = true;
		if (yAxisInput < joystickThreshold)   canPressDown = true;
		if (yAxisInput > -joystickThreshold)  canPressUp = true;
		
		if (xAxisInput > joystickThreshold && canPressRight){
			canPressRight = false;
			moveSelection(1, 0);
		}
		if (xAxisInput < -joystickThreshold && canPressLeft){
			canPressLeft = false;
			moveSelection(-1, 0);
		}
		if (yAxisInput > joystickThreshold && canPressDown){
			canPressDown = false;
			moveSelection(0, 1);
		}
		if (yAxisInput < -joystickThreshold && canPressUp){
			canPressUp = false;
			moveSelection(0, -1);
		}
		
		//up/dow is being screwy with the input, so I'm just not using it
		if ( Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ){
			moveSelection(0, -1);
		}
		if ( Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ){
			moveSelection(0, 1);
		}
		
		
		//check the mouse. If it moved, see if it's over anything
		if (usingMouse){
			Vector2 curMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if ( Mathf.Abs(curMousePos.x-prevMousePos.x) > mouseMoveThreshold || Mathf.Abs(curMousePos.y-prevMousePos.y) > mouseMoveThreshold){
				int thisSelection = -1;
				
				Ray ray;
				RaycastHit hit;
				
				ray = guiCam.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray, out hit)) {
					//figure out if it was any of our levels
					for (int i=0; i<levelIcons.Length; i++){
						if (hit.transform == levelIcons[i].transform && !levelIcons[i].IsLocked){
							thisSelection = i;
						}
					}
				}
				if (curSelection != thisSelection){
					setIconSelected(thisSelection);
				}
			}
			//save the mouse position for next frame
			prevMousePos = curMousePos;
		}
		
	}
	
	public void finish(){
		if (!levelIcons[curSelection].IsLocked){
			cleanUp();
			gm.setLevel(curSelection);
		}
	}
	
	public void cleanUp(){
		//destroy all icons
		for (int i=0; i<levelIcons.Length; i++){
			Destroy(levelIcons[i].gameObject);
		}
		
		//turn this off
		gameObject.SetActive(false);
	}
	
	void moveSelection(int xDir, int yDir){
		curSelection += xDir + yDir*iconRowSize;
		
		if (curSelection < 0){
			curSelection += levelIcons.Length;
		}
		
		setIconSelected( curSelection % levelIcons.Length );
		
	}
	
	//turns on the selecte dicona nd turns off all others
	void setIconSelected(int num){
		for (int i=0; i<levelIcons.Length; i++){
			levelIcons[i].setSelected( i==num );
		}
		
		curSelection = num;
		
		gm.audioController.Play(gm.menuBeep);
	}
}
