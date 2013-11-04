// /////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audio Manager.
//
// This code is release under the MIT licence. It is provided as-is and without any warranty.
//
// Developed by Daniel Rodríguez (Seth Illgard) in April 2010
// http://www.silentkraken.com
//
// /////////////////////////////////////////////////////////////////////////////////////////////////////////
 
using UnityEngine;
using System.Collections;
 
public class AudioManager : MonoBehaviour
{
	
	public float soundEffectVolume;
	public bool muted;
	
	public AudioSource music;
	public AudioSource menuMusic;
	private bool playingGameMusic;
	
	void Start(){
		playingGameMusic = false;
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.O)){
			muted = !muted;
		}
		
		music.pitch = Time.timeScale;
		
		//Debug.Log("tim "+music.time);
	}
	
	
	public void playMusic(bool randomPos){
		playingGameMusic = true;
		if (muted){
			return;
		}
		
		music.Play();
		
		if(randomPos){
			float startPos = Random.Range(0, music.clip.length);
			music.time = startPos;
		}else{
			music.time = menuMusic.time;
		}
		
		menuMusic.Stop();
	}
	
	public void stopMusic(){
		playingGameMusic = false;
		if (muted){
			return;
		}
		
		menuMusic.Play();
		menuMusic.time = music.time;
		
		music.Stop();
	}
	
	public void toggleMute(){
		muted = !muted;
		
		if (muted){
			music.Stop();
			menuMusic.Stop();
		}
		
		if (!muted){
			if (playingGameMusic){
				playMusic(true);
			}
			else{
				stopMusic();
			}
		}
	}
	
	
	public AudioSource Play(AudioClip clip){
		if (muted){
			return null;
		}
		
		if (clip != null){
			return Play(clip, transform, 1f * soundEffectVolume, 1f);
		}else{
			Debug.Log("you tried to play a null clip");
			return null;
		}
	}
	
	public AudioSource Play(AudioClip clip, float volume){
		if (muted){
			return null;
		}
		
		if (clip != null){
			return Play(clip, transform, volume * soundEffectVolume, 1f);
		}else{
			Debug.Log("you tried to play a null clip");
			return null;
		}
	}
	
	
    public AudioSource Play(AudioClip clip, Transform emitter)
    {
        return Play(clip, emitter, 1f, 1f);
    }
 
    public AudioSource Play(AudioClip clip, Transform emitter, float volume)
    {
        return Play(clip, emitter, volume, 1f);
    }
 
    /// <summary>
    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="emitter"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject ("Audio: " +  clip.name);
		go.transform.parent = transform;
        go.transform.position = emitter.position;
        go.transform.parent = emitter;
 
        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play ();
        Destroy (go, clip.length);
        return source;
    }
 
    public AudioSource Play(AudioClip clip, Vector3 point)
    {
        return Play(clip, point, 1f, 1f);
    }
 
    public AudioSource Play(AudioClip clip, Vector3 point, float volume)
    {
        return Play(clip, point, volume, 1f);
    }
 
    /// <summary>
    /// Plays a sound at the given point in space by creating an empty game object with an AudioSource
    /// in that place and destroys it after it finished playing.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="point"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.parent = transform;
        go.transform.position = point;
 
        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(go, clip.length);
        return source;
    }
	
	
	//ANDY EDIT
	//play a sound and have it loop until it is destroyed by something else
	//the game object is returned so that it can be destroyed down the road
	public GameObject PlayLooping(AudioClip clip, Vector3 point, float volume, bool fadeIn){
		//Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.parent = transform;
        go.transform.position = point;
 
        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
		source.loop = true;
        source.Play();
		
		if (fadeIn){
			AudioFadeInScript fadeScript = go.AddComponent<AudioFadeInScript>();
			fadeScript.Source = source;
		}
		
        return go;
	
	}
}