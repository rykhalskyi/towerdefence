using UnityEngine;
using System.Collections;

public class Turret_cannon : BaseTurret {

	public GameObject myProjectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = 0.25f;
	public GameObject muzzleEffect;
	public float errorAmount = 0.001f;
	public Transform myTarget;
	public Transform[] muzzlePositions;
	public Transform turretBall;
	public Transform barrels;
	public Transform aimTilt;
	public Transform aimPan;

	private float nextFireTime;
	private float nextMoveTime;
	private float aimError;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (myTarget) {
						if (Time.time >= nextMoveTime) {
								
								aimPan.LookAt(myTarget);
								aimPan.eulerAngles = new Vector3(0,aimPan.eulerAngles.y,0);
								aimTilt.LookAt(myTarget);
								aimTilt.eulerAngles = new Vector3(aimTilt.eulerAngles.x,aimPan.eulerAngles.y, 0);
								turretBall.rotation = Quaternion.Lerp (turretBall.rotation, aimPan.rotation, Time.deltaTime * turnSpeed);
								barrels.rotation = Quaternion.Lerp(barrels.rotation, aimTilt.rotation, Time.deltaTime*turnSpeed);

								
						}
						if (Time.time >= nextFireTime) {
								FireProjectile();
						}
				}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "GroundEnemy") {
			nextFireTime = (float) (Time.time+(reloadTime*.5));
			myTarget = other.gameObject.transform;
				}
	}

	void OmTriggerStay (Collider other)
	{
				if (other.gameObject.tag == "GroundEnemy") {
			nextFireTime = (float)(Time.time+(reloadTime*.5));
			myTarget = other.gameObject.transform;
		}
		}

	void OnTriggerExit(Collider other){
		if (other.gameObject.transform == myTarget) {
			myTarget = null;
				}
		}


	void CalculateAimError()
	{
		aimError = Random.Range (-errorAmount, errorAmount);
		}

	void FireProjectile()
	{
		audio.Play ();
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + firePauseTime;
		CalculateAimError ();


		foreach (Transform theMuzzlePos in muzzlePositions){
			 
			//Debug.Log(theMuzzlePos.rotation);
			GameObject clone1 = (GameObject) Instantiate(myProjectile, theMuzzlePos.position, theMuzzlePos.rotation);
			GameObject clone2 = (GameObject) Instantiate(muzzleEffect, theMuzzlePos.position, theMuzzlePos.rotation);
			Destroy(clone1,reloadTime);
			Destroy(clone2,reloadTime);
		
		}

	}


}
