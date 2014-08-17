using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

	public LevelMaster levelMaster;
	public GameObject[] AlarmLight;
	public GameObject looser;
	public TweenPosition menuTweener;
	public AudioSource audioAlarm;
	public AudioSource audioLose;

	public UIScript uiscript;

	bool freezeTime = false;
	float waitTime = 0f;


	// Use this for initialization
	void Start () {
		for (int i=0; i<AlarmLight.Length; i++) {
//			AlarmLight[i].animation.Stop ();
		}
		menuTweener.Play (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (freezeTime && Time.time > waitTime)
		{Time.timeScale = 0;
		}
	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "GroundEnemy" || other.gameObject.tag == "Air Enemy") {
			audioAlarm.Play();
			//Debug.Log("On the base"+other.gameObject.tag);
			for (int i=0; i <AlarmLight.Length; i++) {
				AlarmLight[i].animation.Play("AlarmLight");
				//Debug.Log("Light "+i.ToString()+" .Play");
			}
			levelMaster.healthCount--;
			levelMaster.enemyCount--;
			levelMaster.hittedEnemyCount++;
			levelMaster.HUDUpdate();
			Destroy (other.gameObject);

			if (levelMaster.healthCount < 1)
			{
			//	Debug.Log("Looser!!!");
				looser.animation.Play("Lose_animation");
			//	Debug.Log("where is Looser!!!");
				menuTweener.Play(true);
				uiscript.BuildPanelTween.Play(true);
				audioLose.Play();
					waitTime = Time.time+1.5f;
					freezeTime=true;
				uiscript.upgradePanelOpen = true;

				//Time.timeScale = 0;


			}

				}
		}

	public void OnMenuButtons(GameObject btn){

		if (btn.name == "Button_Retry") {
			Time.timeScale =1;
			Application.LoadLevel(1);

		}
		if (btn.name == "Button_Exit") {
			Application.LoadLevel(0);}
	}

}
