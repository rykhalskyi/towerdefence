using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour {
	
	public int myCashValue = 50;
	
	//public Vector2 heightRange = new Vector2(10f, 18f);
	public Vector2 speedRange = new Vector2(7f, 10f);
	public float forwardSpeed = 10f;
	public float health = 100f;
	//public ParticleEmitter smokeTrail;
	public ParticleSystem smokeTrail;
	public GameObject explosion;
	public GameObject bonusExplosion;
	
	public LevelMaster levelMaster;
	
	private float maxHealth = 100f;
	private bool isDestroyed = false;
	
	// Use this for initialization
	void Awake () {

		smokeTrail.enableEmission = false;
		maxHealth = health;
		forwardSpeed = Random.Range (speedRange.x, speedRange.y);
		//transform.position = new Vector3(transform.position.x, Random.Range (heightRange.x, heightRange.y), transform.position.z);
		
		levelMaster = (LevelMaster)GameObject.FindGameObjectWithTag ("LevelMaster").GetComponent ("LevelMaster");
		
		forwardSpeed *= levelMaster.difficultyMultiplier;
		health *= levelMaster.difficultyMultiplier;
		maxHealth *= levelMaster.difficultyMultiplier;
		Debug.Log ("Parent Awake");
	}
	
	
	/*void Update () {
		transform.Translate(Vector3.forward* (forwardSpeed*Time.deltaTime));
	}*/
	
	void TakeDamage(float damage)
	{
		health -= damage;
		float healthPercent = health / maxHealth;
		if (health <= 0)
		{
			Explode ();
			return;
		} else if (healthPercent <= .75f)
		{
			smokeTrail.enableEmission = true;
		}
		
		
	}
	
	void Explode ()
	{
		if (!isDestroyed) {
			
			isDestroyed = true;

			int bonus = Random.Range(0,10);

			if (bonus == 5) {
				GameObject Explosion = (GameObject)Instantiate (bonusExplosion, transform.position, Quaternion.identity);
				levelMaster.healthCount ++;
			}
			else {
			GameObject Explosion = (GameObject)Instantiate (explosion, transform.position, Quaternion.identity);
			}
			//Debug.Log ("Falcon Explosion");
			levelMaster.enemyCount--;
			levelMaster.hittedEnemyCount++;
			levelMaster.cashCount += myCashValue;
			levelMaster.scoreCount += (int)(maxHealth + forwardSpeed * levelMaster.difficultyMultiplier);
			levelMaster.HUDUpdate ();
			//Explosion.transform.parent = gameObject.transform;
			//Destroy (Explosion, .25f);
			Destroy (gameObject);
		}
	}
	
	
}
