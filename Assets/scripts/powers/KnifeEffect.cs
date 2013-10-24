using UnityEngine;
using System.Collections;

public class KnifeEffect : MonoBehaviour {
	
	Player owner;
	
	public Vector3 moveSpeed;
	
	public float killTime;
	private float timer;
	
	public tk2dSprite sprite;
	
	
	public void setup(Player _owner, bool goLeft){
		owner = _owner;
		timer = 0;
		
		if (goLeft){
			moveSpeed.x *= -1;
		}
		
		sprite.FlipX = goLeft;
		
	}
	
	
	// Update is called once per frame
	void Update () {
		timer+=Time.deltaTime;
		if (timer >= killTime){
			Destroy(gameObject);
		}
		
		//move it!
		transform.position += moveSpeed * Time.deltaTime;
	}
	
	
	void OnCollisionEnter(Collision collision) {
        
		//ignore player shells
		if (collision.gameObject.layer == LayerMask.NameToLayer("player")){
			return;
		}
		
		//did we touch a player?
		if (collision.gameObject.layer == LayerMask.NameToLayer("playerHitBox")){
			Player thisPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Player>();
			if (thisPlayer != owner){
				thisPlayer.changeHealth(-1, owner);
			}else{
				return;
			}
		}
		
		//destroy after hitting something
		Destroy(gameObject);
		
    }
}
