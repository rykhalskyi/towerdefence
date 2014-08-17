using UnityEngine;
using System.Collections;

using Pathfinding;

public class Tank : BaseEnemy {


	public Transform tankTurret;
	public Transform tankBody;
	public Transform tankCompass;

	public float turnSpeed = 10f;
	//public float speed = 100f;

	public Vector3 targetPosition;
	public Seeker seeker;
	public CharacterController controller;
	public Path path;
	public float nextWaypointDistance = 3f;

	private int currentWaypoint = 0;


	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();

		targetPosition = GameObject.FindGameObjectWithTag ("GroundTargetObject").transform.position;
			//Start a new path to the targetPosition, return the result to the OnPathComplete function
		FindNewPath ();
		//Debug.Log ("Tank Start");
	
	}

	public void FindNewPath(){
		seeker.StartPath (transform.position,targetPosition, OnPathComplete);
		}

	public void OnPathComplete (Path p) {
		//Debug.Log ("Yay, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
				if (path != null) {
						if (currentWaypoint < path.vectorPath.Count) {
				//Debug.Log ("so we have a path and stil don't reach the end of it");

								Vector3 direction = (path.vectorPath [currentWaypoint] - transform.position).normalized;
								direction *= forwardSpeed * Time.fixedDeltaTime;
								controller.SimpleMove (direction);

								tankCompass.LookAt (path.vectorPath [currentWaypoint]);
								tankBody.rotation = Quaternion.Lerp (tankBody.rotation, tankCompass.rotation, Time.deltaTime * turnSpeed);

								if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
										currentWaypoint++;
			
								}

						}
				}
		}
}
