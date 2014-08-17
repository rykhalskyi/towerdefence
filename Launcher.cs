using UnityEngine;
using System.Collections;


public class Launcher : BaseTurret  {

	public GameObject myProjectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = 0.25f;
	public float errorAmount = 0.001f;
	public Transform myTarget;
	public Transform[] muzzlePositions;
	public Transform pivot_Tilt;
	public Transform pivot_Pan;
	public Transform aim_Pan;
	public Transform aim_Tilt;

	private float nextFireTime;
	private float nextMoveTime;
/*	private Vector3 desiredRotation;
	private float aimError;
*/
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  if (myTarget) {
						aim_Pan.LookAt (myTarget);

						aim_Pan.eulerAngles = new Vector3 (0, aim_Pan.eulerAngles.y, 0);
						aim_Tilt.LookAt (myTarget);
						aim_Tilt.eulerAngles = new Vector3 (aim_Tilt.eulerAngles.x, aim_Pan.eulerAngles.y, 0);
		
		
						pivot_Pan.rotation = Quaternion.Lerp (pivot_Pan.rotation, aim_Pan.rotation, Time.deltaTime * turnSpeed);
						pivot_Tilt.rotation = Quaternion.Lerp (pivot_Tilt.rotation, aim_Tilt.rotation, Time.deltaTime * turnSpeed);

		

						if (Time.time >= nextFireTime) {

								FireProjectile ();
						}
				}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Air Enemy" ) {
			nextFireTime = (float) (Time.time+(reloadTime*.5));
			myTarget = other.gameObject.transform;
		}
	}

	/*void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Air Enemy") {
			nextFireTime = (float) (Time.time+(reloadTime*.5));
			myTarget = other.gameObject.transform;
		}
	}*/
	
	void OnTriggerExit(Collider other){
		if (other.gameObject.transform == myTarget) {
			myTarget = null;
		}
	}
	

	void FireProjectile()
	{
		audio.Play ();
		nextFireTime = Time.time + reloadTime;
	//	nextMoveTime = Time.time + firePauseTime;
		//CalculateAimError ();
		
		int m = Random.Range (0, 6);
			
		GameObject clone1 = (GameObject) Instantiate(myProjectile, muzzlePositions[m].position, muzzlePositions[m].rotation);
	    clone1.GetComponent <Projectile_Missile>().myTarget = myTarget;
		clone1.GetComponent <Projectile_Missile> ().mySpeed = 14 * (1 / reloadTime);
		clone1.GetComponent <Projectile_Missile> ().myDamageAmount = (int) (20 *(1 / reloadTime));
		//Destroy (clone1, (float)(reloadTime+.5));


	
		
	}
}
