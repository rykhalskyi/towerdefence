using UnityEngine;
using System.Collections;

public class LaserProjectile : MonoBehaviour {
	public float mySpeed = 25;
	public float myRange = 8;
	public GameObject myExplosion;
	public int myDamageAmount = 34;
	
	private float myDist;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
		myDist += Time.deltaTime * mySpeed;
		if (myDist >= myRange) {
			
			//		Destroy (gameObject);
			Explode (false);
		}
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("on trigger");
		if (other.gameObject.tag == "GroundEnemy" || other.gameObject.tag == "Air Enemy") {
			//Debug.Log("cannon enemy");
			other.gameObject.SendMessage("TakeDamage",myDamageAmount,SendMessageOptions.DontRequireReceiver);
			Explode (true);
		}
	}
	
	void Explode(bool exp)
	{
		//Debug.Log ("Explode");
		if (exp) {
		GameObject explode = (GameObject) Instantiate (myExplosion, transform.position, Quaternion.identity);
		
		Destroy (explode,.5f);
		}
		Destroy (gameObject);
	}
}
