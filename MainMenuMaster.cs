using UnityEngine;
using System.Collections;

public class MainMenuMaster : MonoBehaviour {

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

		if (btn.name == "Button_NewGame") {
			Application.LoadLevel(1);
		}
		if (btn.name == "Button_Resume") {
			Application.LoadLevel(4); }
		if (btn.name == "Button_Exit") {
			Application.Quit();
		}
		

	}
}
