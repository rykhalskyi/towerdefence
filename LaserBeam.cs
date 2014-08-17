using UnityEngine;
using System.Collections;

public class LaserBeam : BaseTurret  {


	public GameObject myProjectile;
	public float reloadTime = .5f;
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
	private int currentMuzzle = 0;
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
		if (other.gameObject.tag == "Air Enemy" || other.gameObject.tag == "GroundEnemy") {
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
		nextFireTime = Time.time + reloadTime/3;
		nextMoveTime = Time.time + firePauseTime;
		//CalculateAimError ();
		
		
		//foreach (Transform theMuzzlePos in muzzlePositions){
			
			//Debug.Log(theMuzzlePos.rotation);
			GameObject clone1 = (GameObject) Instantiate(myProjectile, muzzlePositions[currentMuzzle].position, muzzlePositions[currentMuzzle].rotation);
			Destroy(clone1,reloadTime*2);
			
		currentMuzzle++;
		if (currentMuzzle > 2) {
						currentMuzzle = 0;
				}
		
		
	//}
}
}