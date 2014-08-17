using UnityEngine;
using System.Collections;

public class LevelMenuMaster : MonoBehaviour {

	
	public UILabel maxScoreLabel;
	
	// Use this for initialization
	void Start () {
		int maxscore = 0;
		if (PlayerPrefs.HasKey ("MaxScore"))
		{
			maxscore = PlayerPrefs.GetInt("MaxScore");
		}
		maxScoreLabel.text = "Max SCORE: " + maxscore.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ButtonPressed(GameObject btn) {
		
		if (btn.name == "Button_level1") {
			Application.LoadLevel(1);
		}
		if (btn.name == "Button_level2") {
			Application.LoadLevel(2); }
		if (btn.name == "Button_level3") {
			Debug.Log("3");
			Application.LoadLevel(3); 
		}

		if (btn.name == "Button_Exit") {
			Application.LoadLevel(0);
		}
		
		
	}
}