using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectScreen : MonoBehaviour {
	
	public GameObject iconPrefab;
	
	public string[] levelNames;
	private LevelSelectIcon[] levelIcons;
	
	//placing the icons
	public int iconRowSize;
	public float iconStartY, iconYPadding, iconXSpacing;
	
	
	public void reset(){
		gameObject.SetActive(true);
		
		//spawn some level icons!
		levelIcons = new LevelSelectIcon[levelNames.Length];
		for (int i=0; i<levelNames.Length; i++){
		
			int row = (int)Mathf.Floor( (float)i/(float)iconRowSize);
			int col = i%iconRowSize;
			
			Debug.Log(i +" - "+levelNames[i]+" -  r:"+row+" c:"+col);
			
			Vector3 newPos = new Vector3(0,0,0);
			newPos.x = (col-1) * iconXSpacing;  //center around 0
			newPos.y = iconStartY + row*iconYPadding;
			
			GameObject newIconObj = Instantiate(iconPrefab, newPos, new Quaternion(0,0,0,0)) as GameObject;
			levelIcons[i] = newIconObj.GetComponent<LevelSelectIcon>();
			
			levelIcons[i].setup(i%3, levelNames[i], 666);
		}
	}
	
	public void finish(){
		//destroy all icons
		for (int i=0; i<levelIcons.Length; i++){
			Destroy(levelIcons[i].gameObject);
		}
		
		//turn this off
		gameObject.SetActive(false);
	}

	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<levelNames.Length; i++){
		
			int row = (int)Mathf.Floor( (float)i/(float)iconRowSize);
			int col = i%iconRowSize;
			
			Vector3 newPos = new Vector3(0,0,0);
			newPos.x = (col-1) * iconXSpacing;  //center around 0
			newPos.y = iconStartY + row*iconYPadding;
			
			levelIcons[i].gameObject.transform.position = newPos;
		}
	}
}
