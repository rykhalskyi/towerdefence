using UnityEngine;
using System.Collections;

public class Projetile_Cannon : MonoBehaviour {
	public float mySpeed = 10;
	public float myRange = 10;
	public GameObject myExplosion;
	public int myDamageAmount = 25;

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
			Explode ();
				}
	
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("on trigger");
		if (other.gameObject.tag == "GroundEnemy") {
			//Debug.Log("cannon enemy");
			other.gameObject.SendMessage("TakeDamage",myDamageAmount,SendMessageOptions.DontRequireReceiver);
			Explode ();
		}
	}

	void Explode()
	{
		//Debug.Log ("Explode");
		GameObject explode = (GameObject) Instantiate (myExplosion, transform.position, Quaternion.identity);
		
		Destroy (explode,.5f);
		Destroy (gameObject);
	}
}
