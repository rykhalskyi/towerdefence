using UnityEngine;
using System.Collections;

public class Projectile_Missile : MonoBehaviour {

	public Transform myTarget;
	public GameObject myExplosion;
	public float myRange =10f;
	public float mySpeed = 10f;
	public int myDamageAmount = 15;

	private float myDist;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
		myDist += Time.deltaTime * mySpeed;
		if (myDist >= myRange) {
			//Debug.Log("outrange");
			Explode(false);
				}

		if (myTarget) {
						transform.LookAt (myTarget);
				} else
		{
			//Debug.Log("no target");
			Explode(false);
				}
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == "Air Enemy") {
			//Debug.Log("enemy");
						Explode (true);
			other.gameObject.SendMessage("TakeDamage",myDamageAmount,SendMessageOptions.DontRequireReceiver);
				}
	}

	void Explode(bool explosion)
	{
		//Debug.Log ("Explode");
		if (explosion) {
						GameObject explode = (GameObject)Instantiate (myExplosion, transform.position, Quaternion.identity);

						Destroy (explode, .5f);
				}
		Destroy (gameObject);
		}
}
