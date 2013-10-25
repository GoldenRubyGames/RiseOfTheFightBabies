#pragma strict

using UnityEngine;
using System.Collections;

public enum WormSound { WORM_APROACH_ID, WORM_SNARL_A_ID, WORM_SNARL_B_ID, WORM_SNARL_C_ID, WORM_SNARL_D_ID, WORM_SNARL_E_ID, WORM_SPIT_ID, WORM_SWALLOW_ID, WORM_HURT_ID, WORM_CAHRGE_A_ID, WORM_CAHRGE_B_ID, WORM_CHEW_LOOP_ID, NUM_WORM_SOUNDS};
public enum ClickSounds{ CLICK_BIG_TACTILE, CLICK_OPEN_TACTILE, CLICK_CLOSE_TACTILE, CLICK_NEGATIVE_FAIL, CLICK_NEGATIVE_CANCEL, CLICK_PLAY_BUTTON, CLICK_POSITIVE, NUM_CLICK_SOUNDS};
    

public class SoundManagerOLD : MonoBehaviour {
	
	/*
	
	public AudioManager AudioManager;
	
	bool muteEverything = false;
	
	float baseVolume;
	
    //music
    public const int NUM_MUSIC = 6;
    int curMusic;
    int prevMusic;
    public AudioSource[] music = new AudioSource[NUM_MUSIC];
    float[] musicStopTime = new float[NUM_MUSIC];
    //fading in and out
    float musicFadeInSpeed;
    float musicFadeOutSpeed;
    float musicCrossFadeSpeed;
    bool usingFastFade;     //toggles crossfad speed or normal, fast fade in/out
    int[] musicFadeDir = new int[NUM_MUSIC];
    bool setMusicPosFromPrev;
    //volume
	[System.NonSerializedAttribute]
    public float musicMaxVol;
	[System.NonSerializedAttribute]
    public float musicGameMaxVol;
    float[] musicVol = new float[NUM_MUSIC];
    //music speed
    float targetMusicSpeed;
    float musicXeno;
    bool musicAtTargetSpeed;
    
    //player
    public const int NUM_JUMP_SOUNDS = 3;
    public AudioClip[] jumpSounds = new AudioClip[NUM_JUMP_SOUNDS];
    public const int NUM_SPRING_SOUNDS = 4;
    public AudioClip[] springSounds = new AudioClip[NUM_SPRING_SOUNDS];
    
    public AudioClip jumpLand;
    
    public const int NUM_JETPACK_SOUNDS = 3;
    public AudioClip[] jetpackSounds = new AudioClip[NUM_JETPACK_SOUNDS];
    public AudioClip jetpackEmptySound;
    
    //footsteps
    public const int NUM_FOOTSTEP_MATERIALS = 3;
    public const int NUM_FOOTSTEP_SOUNDS = 5;
    public AudioClip[,,] footsteps = new AudioClip[NUM_FOOTSTEP_MATERIALS,NUM_FOOTSTEP_SOUNDS,2];
	//1d arrays so I cna add the sounds in the unity editor
	public AudioClip[] footstepsDirtLeft = new AudioClip[NUM_FOOTSTEP_SOUNDS];
	public AudioClip[] footstepsDirtRight = new AudioClip[NUM_FOOTSTEP_SOUNDS];
	public AudioClip[] footstepsMetalLeft = new AudioClip[NUM_FOOTSTEP_SOUNDS];
	public AudioClip[] footstepsMetalRight = new AudioClip[NUM_FOOTSTEP_SOUNDS];
	public AudioClip[] footstepsIceLeft = new AudioClip[NUM_FOOTSTEP_SOUNDS];
	public AudioClip[] footstepsIceRight = new AudioClip[NUM_FOOTSTEP_SOUNDS];
	
    [System.NonSerializedAttribute]
    public bool playerIsWalking;
    bool leftFoot;
    [System.NonSerializedAttribute]
    public int curFootstep;
    [System.NonSerializedAttribute]
    public int curFootstepMaterial;
	[System.NonSerializedAttribute]
    public float timeBetweenFootsteps;
    float footstepTimer;
    
    //worm
    public AudioClip rockSlideLoop;
	public AudioClip[] wormSounds = new AudioClip[(int)WormSound.NUM_WORM_SOUNDS];
    //belch
    public AudioClip wormBelch;
	int curWormSound;
	[System.NonSerializedAttribute]
    public bool wormIsChewing;
    AudioSource wormSoundObject = null;
	public AudioSource rockSlideLoopObject; 
    
    //coins
    public AudioClip coinSound;
    public AudioClip coinBlueSound;
    public AudioClip giantCoinSound;
    public AudioClip magnetSound;
	GameObject magnetLoopObject;
    public const int MAX_COIN_GET_SOUNDS = 5;
    float[] coinTimers = new float[MAX_COIN_GET_SOUNDS];
    
    //misc
    public AudioClip confetti;
    public AudioClip warpSound;
    public AudioClip hologramSound;
    
    //special spaces
    public AudioClip[] darkAreaSoud = new AudioClip[2];
    public AudioClip[] gravFlipSoud = new AudioClip[2];
    
    //deaths
    public AudioClip lavaDeath;
    public AudioClip spikeDeath;
    public AudioClip textFlySound;
    
    //items
    public AudioClip itemGetSound;
    public AudioClip[] drillSounds = new AudioClip[3];    //0-start 1-loop 2-end
    GameObject drillLoopObject = null;
	public AudioClip[] bombSounds = new AudioClip[3];    //0-start 1-loop 2-end
	GameObject bombLoopObject = null;
    
    //unlock sound
    public AudioClip unlockSound;
    
    //store/UI
	public AudioClip[] clickSounds = new AudioClip[(int)ClickSounds.NUM_CLICK_SOUNDS];
    public AudioClip purchaseSound;
    
    //drop down menu
    public const int NUM_MENU_BOUNCE_SOUNDS = 2;
    public AudioClip[] menuBounceSounds = new AudioClip[NUM_MENU_BOUNCE_SOUNDS];
    public AudioClip menuStartDropSound;
    public AudioClip menuEndDropSound;
    public AudioClip menuLoopSound;
    
    //mute toggles
    public bool muteSoundEffects;
    public bool muteMusic;
	
	
	//************************
	//functions!
	
	public void Awake(){
		if (GameObject.FindGameObjectsWithTag("audioPlayer").Length > 1){
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad(transform.gameObject);
	}
	
	
	public void setup(){
	    
		
	    if (!muteEverything){
	        //setup the specific sounds
	        baseVolume = 0.7f;
	        
	        //game music
	        musicGameMaxVol = 1;//0.95f;
	        musicMaxVol = musicGameMaxVol;
	        musicFadeInSpeed = 0.1f;
	        musicFadeOutSpeed = 0.02f;
	        musicCrossFadeSpeed = 0.01f;
			for (int i=0; i<(int)NUM_MUSIC; i++){
	            musicVol[i] =0;
	            musicStopTime[i]=0;
	        }
	        curMusic = 0;
	        //music speed
	        musicXeno = 0.93f;
	        targetMusicSpeed = 1;
	        musicAtTargetSpeed = true;
			
	        //footsteps
	        curFootstep = 0;
	        curFootstepMaterial=0;
	        leftFoot = true;
	        playerIsWalking = true; //testing
	        footstepTimer = 0;
			
			//add the footsteps to the larger 3d array
			for (int i=0; i<NUM_FOOTSTEP_SOUNDS; i++){
				footsteps[0,i,0] = footstepsDirtLeft[i];
				footsteps[0,i,1] = footstepsDirtRight[i];
				footsteps[1,i,0] = footstepsMetalLeft[i];
				footsteps[1,i,1] = footstepsMetalRight[i];
				footsteps[2,i,0] = footstepsIceLeft[i];
				footsteps[2,i,1] = footstepsIceRight[i];
			}
	        
	        //worm
	        curWormSound = (int) WormSound.WORM_APROACH_ID;
	        //wormSounds[curWormSound].play();
	        
	        //ruble
			rockSlideLoopObject.clip = rockSlideLoop;
			rockSlideLoopObject.loop = true;
			rockSlideLoopObject.volume = 0;
			rockSlideLoopObject.Play();
			
	        
	        muteSoundEffects = true;
	        muteMusic = true;
	        
	    }
	    
	    setMusicPosFromPrev = false;
	    toggleSoundEffectMute();
	}
	
	public void update(bool playMagnetSound, float deltaTime){
	    if (muteEverything){
	        muteMusic=true;
	        muteSoundEffects=true;
	        return;
	    }
		
		if (muteSoundEffects){
			playMagnetSound = false;
		}
	    
	    //reduce the counters for the coin sound
	    for (int i=0; i<MAX_COIN_GET_SOUNDS; i++){
	        coinTimers[i]-=deltaTime;
	    }
	    
	    footstepTimer += Time.deltaTime;
	    
	    if (playerIsWalking && !muteSoundEffects){
	        if (footstepTimer >= timeBetweenFootsteps){
	            footstepTimer = 0;
	            //chose another one at random and switch feet
	            curFootstep = (int)Random.Range(0,NUM_FOOTSTEP_MATERIALS);
	            leftFoot = !leftFoot;
				int footNum = leftFoot ? 1 : 0;
	            
				AudioManager.Play(footsteps[curFootstepMaterial , curFootstep , footNum], transform.position, baseVolume);
	        }
	    }
	    
	    //make sure magnet is doing what it should
		if (playMagnetSound && magnetLoopObject == null){
			magnetLoopObject = AudioManager.PlayLooping(magnetSound, transform.position, 0.9f, true);
		}
		if (!playMagnetSound && magnetLoopObject != null){
			//Destroy(magnetLoopObject);
			AudioFadeOutScript fadeOutScript = magnetLoopObject.AddComponent<AudioFadeOutScript>();
			fadeOutScript.Source = magnetLoopObject.GetComponent<AudioSource>();
			magnetLoopObject = null;
		}
	    
	    //keep the warm snarling
		float wormSoundChance = 1.5f * Time.deltaTime;//0.03f; 
		if (wormSoundObject==null && !muteSoundEffects  && (Random.value<wormSoundChance || wormIsChewing)) {
	        //ussualy play the aproaching loop, sometimes a snarl
	        int soundID = (int) WormSound.WORM_APROACH_ID;
	        if (Random.value<0.6f){
	            soundID = (int) WormSound.WORM_SNARL_A_ID + (int)Random.Range(0,5);
	        }
	        //WHEN WE HAVE A CHEWING LOOP, IT SHOULD PLAY CONTINUOUSLY UNTIL THE WORM IS DONE
	        if (wormIsChewing){
	            soundID = (int) WormSound.WORM_CHEW_LOOP_ID;
	        }
	        playWormSound( soundID );
	    }
	    
	    
	    //fade the music in or out if that's what's up
		for (int i=0; i<NUM_MUSIC; i++){
	        if ( music[i].isPlaying ){
	            //cout<<"this music: "<<i<<"   vol "<<music[i].getVolume()<<"   fade dir: "<<musicFadeDir[i]<<endl;
	            
	            if (musicFadeDir[i]==1 && musicVol[i]<musicMaxVol){
	                //cout<<"fade in"<<endl;
	                musicVol[i]+= (usingFastFade) ? musicFadeInSpeed : musicCrossFadeSpeed;
	                musicVol[i] = Mathf.Min(musicVol[i], musicMaxVol);
	                music[i].volume=(musicVol[i]);
	                
	                if (i==curMusic && music[prevMusic].isPlaying && setMusicPosFromPrev){
						music[i].time = music[prevMusic].time;
	                    //music[i].currentTime = music[prevMusic].currentTime;
	                    setMusicPosFromPrev = false;
	                }
	            }
	            
	            if (musicFadeDir[i]==-1){
	                //cout<<"fade out"<<endl;
	                musicVol[i]-= (usingFastFade) ? musicFadeOutSpeed : musicCrossFadeSpeed;
	                musicStopTime[i] = music[i].time;
	                if (musicVol[i]<=0){
	                    music[i].Pause();
	                }else{
						music[i].volume = musicVol[i];
	                }
	            }
	        }
	    }
	    
	    if (!musicAtTargetSpeed){
			float curSpeed = music[curMusic].pitch;
	        curSpeed = musicXeno*curSpeed + (1-musicXeno)*targetMusicSpeed;
	        if (Mathf.Abs(curSpeed-targetMusicSpeed)<0.001f){
	            curSpeed = targetMusicSpeed;
	            musicAtTargetSpeed = true;
	        }
	        for (int i=0; i<NUM_MUSIC; i++){
	            music[i].pitch=curSpeed;
	        }
	    }
	}
	
	//mutes or unmutes the game
	public void toggleSoundEffectMute(){
	    muteSoundEffects = !muteSoundEffects;
	    if (muteSoundEffects){
	        if (wormSoundObject != null){
				Destroy(wormSoundObject);
				wormSoundObject = null;
			}
	        rockSlideLoopObject.Pause();
	        playMenuLoop(false);
	    }else{
	        
			//wormSounds[curWormSound].play();
	        rockSlideLoopObject.Play();
	    }
	}
	
	public void toggleMusicMute(){
		muteMusic = !muteMusic;
	    
	    if (muteMusic){
			music[curMusic].Pause();
	    }
	}
	
	public void setMusicPlaying(bool play, bool restartSong=false){
		if (muteMusic)  return;
	    
	    if (restartSong)    musicStopTime[curMusic] = 0;
	    
	    if (play){
	        if (music[curMusic].isPlaying == false){
	            music[curMusic].Play();
	        }
			music[curMusic].time = musicStopTime[curMusic];
	        musicFadeDir[curMusic]=1; //bring it in
	    }else{
	        musicFadeDir[curMusic]=-1;    //fade it out
	    }
	    
	    usingFastFade = true;
	}
	
	public void setMusicArea(int area){
	    area = Mathf.Clamp(area, 0, NUM_MUSIC-1); //don't let it go out of range
	    
	    prevMusic = curMusic;
	    curMusic = area;
	    
	    if (music[prevMusic].isPlaying){
			
			Debug.Log("god is here, but he is uncaring");
	        musicFadeDir[prevMusic]=-1;
	        
	        setMusicPlaying(true, false);
	    }
	    
	    setMusicPosFromPrev = true;
	    usingFastFade = false;
	}
	
	public void setTargetMusicSpeed(float val){
		targetMusicSpeed = val;
	    musicAtTargetSpeed = false;
	}
	
	//play a random jump sound
	public void playJump(bool springShoes){
	    if (muteSoundEffects)   return;
	    
	    if (!springShoes){
	        int num=(int)Random.Range(0,NUM_JUMP_SOUNDS);
	        AudioManager.Play(jumpSounds[num], transform.position, baseVolume);
	    }else{
	        int num=(int)Random.Range(0,NUM_SPRING_SOUNDS);
			AudioManager.Play(springSounds[num], transform.position, 0.5f);
	    }
	    
	}
	
	public void playJumpLand(){
		if (muteSoundEffects)   return;
		AudioManager.Play(jumpLand, transform.position, baseVolume);
	}
	
	public void playJetpack(float power){
	    if (muteSoundEffects)   return;
	    
	    //select the sound to play based on the power of the jump
	    bool playEmpty = power<=0.3f;
	    
	    if (playEmpty){
			AudioManager.Play(jetpackEmptySound, transform.position, baseVolume);
	    }
	    else{
	    
	        int num = (int)Random.Range(0,NUM_JETPACK_SOUNDS);
	        //set the volume based on power
	        float volume = BasicInfo.map(power, 0, 1, 0.1f, 0.9f, false);
	        
			AudioManager.Play(jetpackSounds[num], transform.position, volume);
	    }
	}
	
	public void playCoinGet(){
	    if (muteSoundEffects)   return;
	    
	    //make sure there aren't too many playing
	    float maxTimeLeft = 1.2f;
	    int openID = -1;
	    for (int i=0; i<MAX_COIN_GET_SOUNDS; i++){
	        if (coinTimers[i]<maxTimeLeft){
	            openID = i;
	            break;
	        }
	    }
	    
	    //get out if there were not spots
	    if (openID == -1){
	        return;
	    }
	    
	    //and if there was, play the sound and start the timer
	    AudioManager.Play(coinSound, transform.position, baseVolume-0.1f);
	    coinTimers[openID] = 1.80f;   //aproximate length of coin get sound
	}
	
	public void playBlueCoinGet(){
	    if (muteSoundEffects)   return;
	    AudioManager.Play(coinBlueSound, transform.position, baseVolume-0.1f);
	}
	
	public void playGiantCoinGet(){
	    if (muteSoundEffects)   return;
	    //check that there is an open spot in the sounds
	    AudioManager.Play(coinBlueSound, transform.position, baseVolume);
	}
	
	public void playConfetti(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(confetti, transform.position, baseVolume);
	}
	
	public void playWarp(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(warpSound, transform.position, baseVolume);
	}
	
	public void playHologram(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(hologramSound, transform.position, baseVolume);
	}
	
	public void playDarkArea(bool enter){
	    if (muteSoundEffects)   return;
		int enterNum = enter ? 1 : 0;
	    AudioManager.Play(darkAreaSoud[enterNum], transform.position, baseVolume);
	}
	public void playGravFlip(bool enter){
	    if (muteSoundEffects)   return;
		int enterNum = enter ? 1 : 0;
	    AudioManager.Play(gravFlipSoud[enterNum], transform.position, baseVolume);
	}
	
	public void playDeath(string deathType){
	    if (muteSoundEffects)   return;
	    
	    if (deathType=="lava"){
			AudioManager.Play(lavaDeath, transform.position, baseVolume);
	        return;
	    }
	    
	    if (deathType=="spikes"){
			AudioManager.Play(spikeDeath, transform.position, baseVolume);
	        return;
	    }
	    
	    Debug.Log("PROBLEM: no death sound for "+deathType);
	}
	
	public void playTextFlySound(){
	    if (muteSoundEffects)   return;
	    AudioManager.Play(textFlySound, transform.position, baseVolume);
	}
	
	public void playItemGet(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(itemGetSound, transform.position, baseVolume);
	}
	
	public void playDrill(bool isStarting){
		if (isStarting){
	        if (!muteSoundEffects){
				AudioManager.Play(drillSounds[0], transform.position, baseVolume);
				if (drillLoopObject == null){
					drillLoopObject = AudioManager.PlayLooping(drillSounds[1], transform.position, baseVolume, false);
				}
	        }
	    }else{
			if (drillLoopObject != null){
				Destroy(drillLoopObject);
				drillLoopObject = null;
			}
	        if (!muteSoundEffects){
	            AudioManager.Play(drillSounds[2], transform.position, baseVolume);
	        }
	    }
	}
	
	public void playBomb(bool isStarting, int numBombs){
		
	    if (isStarting){
	        if (!muteSoundEffects){
				AudioManager.Play(bombSounds[0], transform.position, baseVolume);
				//start the fuse loop if there isn't alreayd one playing
				if (bombLoopObject == null){
					bombLoopObject = AudioManager.PlayLooping(bombSounds[1], transform.position, baseVolume, false);
				}
	        }
	    }else{
	        if (numBombs==0){
				stopBombSound();
			}
	        if (!muteSoundEffects){
				AudioManager.Play(bombSounds[2], transform.position, baseVolume);
	        }
	    }
	}
	
	public void stopBombSound(){
		if(bombLoopObject!=null) {
			Destroy(bombLoopObject);
			bombLoopObject = null;
		}
	}
	
	public void playUnlock(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(unlockSound, transform.position, baseVolume);
	}
	
	public void playClick(int num){
	    if (muteSoundEffects)   return;
	    
	    if (num >= (int)ClickSounds.NUM_CLICK_SOUNDS || num<0){
	        Debug.Log("not a valid click sound");
	        return;
	    }
	    
		AudioManager.Play(clickSounds[num], transform.position, baseVolume);
	}
	
	public void playPurchase(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(purchaseSound, transform.position, baseVolume);
	}
	
	public void playMenuStartDrop(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(menuStartDropSound, transform.position, baseVolume);
	}
	
	public void playMenuEndDrop(){
	    if (muteSoundEffects)   return;
		AudioManager.Play(menuEndDropSound, transform.position, baseVolume);
	}
	
	public void playMenuBounce(){
	    if (muteSoundEffects)   return;
	    
	    int soundNum = (int)Random.Range(0, NUM_MENU_BOUNCE_SOUNDS);
		AudioManager.Play(menuBounceSounds[soundNum], transform.position, baseVolume);
	}
	
	
	public void playWormSound(int soundID){
	    if (muteSoundEffects)   return;
	    if (wormSoundObject != null){
	        Destroy(wormSoundObject);
			wormSoundObject = null;
	    }
	    
	    curWormSound = soundID;
		wormSoundObject = AudioManager.Play(wormSounds[curWormSound], transform.position, baseVolume);
	}
	
	public void playWormBelch(){
	    if (muteSoundEffects)   return;
	    
		AudioManager.Play(wormBelch, transform.position, baseVolume*0.7f);
	}
	
	public void setWormVolume(float distance, bool muteRocks, float retinaAdjust){
		
	    if (muteSoundEffects)   return;
	    
	    float minDistForRockslide = 1500*retinaAdjust;
	    float minDistanceForGrowl = 900*retinaAdjust;
	    float fullGrowlDist = 300*retinaAdjust;      //volume hits 1 at this distance
	    
	    //put it on a curve to get much louder when closer
	    float growlPrc = Mathf.Clamp((distance-fullGrowlDist)/minDistanceForGrowl, 0, 1);
	    growlPrc = Mathf.Pow(growlPrc, 0.2f);
	    float growlVol = 1-growlPrc;
	    
	    
	    float rockslidePrc = Mathf.Clamp((distance-fullGrowlDist)/minDistForRockslide, 0, 1);
	    rockslidePrc = Mathf.Pow(rockslidePrc, 0.35f);
	    float rockslideVol = 1-rockslidePrc;
	    
	    //wormSounds[curWormSound].setVolume(growlVol);
		if (wormSoundObject != null){
			wormSoundObject.volume = growlVol;
		}
		
	    if (!muteRocks){
	        rockSlideLoopObject.volume = (rockslideVol);
	    }else{
	        rockSlideLoopObject.volume = 0;
	    }
	    
	    
	}
	
*/
}
