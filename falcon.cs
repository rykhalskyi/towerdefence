using UnityEngine;
using System.Collections;

public class falcon : BaseEnemy {

//	public int myCashValue = 50;

	public Vector2 heightRange = new Vector2(10f, 18f);


	// Use this for initialization
	 void Start () {
	

		transform.position = new Vector3(transform.position.x, Random.Range (heightRange.x, heightRange.y), transform.position.z);
		Debug.Log ("Child Start");
		}


	void Update () {
		transform.Translate(Vector3.forward* (forwardSpeed*Time.deltaTime));
	}


}
