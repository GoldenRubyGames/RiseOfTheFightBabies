static var instance : Kongregate;

function Start(){
	if(instance == null){
		Application.ExternalEval("if(typeof(kongregateUnitySupport) != 'undefined'){kongregateUnitySupport.initAPI('" + gameObject.name + "', 'OnKongregateAPILoaded');};");
		instance = this;
	}
}

static var isKongregate = false;
static var userId = 0;
static var username = "Guest";
static var gameAuthToken = "";

function OnKongregateAPILoaded(userInfoString){
	// We now know we're on Kongregate
	isKongregate = true;
	// Split the user info up into tokens
	var params = userInfoString.Split("|"[0]);
	userId = parseInt(params[0]);
	username = params[1];
	gameAuthToken = params[2];
}

var debug : boolean;
var debugStyle : GUISkin;
function OnGUI(){

	GUI.skin =  debugStyle;
	
	if(debug){
		GUILayout.BeginArea(Rect(Screen.width/2-256, 10, 256, 256));
			GUILayout.Label(isKongregate.ToString());
			GUILayout.Label(userId.ToString());
			GUILayout.Label(username);
			GUILayout.Label(gameAuthToken);
		GUILayout.EndArea();
	}
}

function toggleDebug(){
	debug = !debug;
}

static function SubmitStatistic(stat : String, val : int){
	if(isKongregate){
		Application.ExternalCall("kongregate.stats.submit",stat,val);
	}
}

static function SubmitHighScore(levelNum : int, score : int){
	Debug.Log("I'm try submit "+score+" in "+levelNum);
	if(isKongregate){
		var statName = "level_"+levelNum.ToString();
		Application.ExternalCall("kongregate.stats.submit",statName,score);
	}
}

