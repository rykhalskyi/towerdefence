using UnityEngine;
using System.Collections;

public class LevelMaster : MonoBehaviour {

	public static float playerDamage = 0f;



	//HUD labels
	public UILabel waveLabel;
	public UILabel scoreLabel;
	public UILabel healthLabel;
	public UILabel cashLabel;

	public bool waveActive = false;
	public bool spawnEnemies = false;
	public bool upgradePanelOpen = false;

	//ingame counts
	public int scoreCount = 0;
	public int healthCount = 10;
	public int cashCount = 200;

	//wave logic
	public int waveLevel = 0;
	public float difficultyMultiplier = 1f;
	public float waveLenght = 30f;
	public float intermissionTime = 1f;
	private float waveEndTime = 0f;
	public int theLastWave = 20;

	public int[] WeaponPrices;
	public int[] WeaponAvailibility;
	public UILabel[] WeaponPricesLabels;

	//respawn logic
	public GameObject[] EnemyPrefabs;

	public float respawnMinBase = 3f;
	public float respawnMaxBase = 10f;
	private float respawnMin = 3f;
	private float respawnMax = 10f;
	private float respawnInterval = 2.5f;
	public int enemyCount = 0;
	public int hittedEnemyCount = 0;
	private float lastRespawnTime = 0f;

	//public GameObject falconPrefab;
	public Transform falconSpawns;
	public Transform tankSpawns;
	private Transform[] falconSpawnPoint;
	private Transform[] tankSpawnPoint;
	public float falconInterval = 1f;
	private float nextFalconSpawnTime = 0f;

	private UIScript uiscript;

	//in game sprites

	public GameObject waveCompletedSprite;

	//winner logic
	public Animation winnerAnimation;
	public TweenPosition winnerPanel;
	private float freezeWait;
	private bool freezeTime = false;

	// Use this for initialization
	void Start () {

		uiscript = (UIScript)GameObject.FindGameObjectWithTag ("MainCamera").GetComponent ("UIScript");
		falconSpawnPoint = new Transform[falconSpawns.childCount];
		int i = 0;
		foreach (Transform theSpawnPoint in falconSpawns) {
						falconSpawnPoint [i] = theSpawnPoint;
						i++;
				}
		i = 0;
		tankSpawnPoint = new Transform[tankSpawns.childCount];
		foreach (Transform theSpawnPoint in tankSpawns) {
			tankSpawnPoint[i] = theSpawnPoint;
			i++;
				}
		SetNextWave ();
		StartNewWave ();
		
		Time.timeScale = 1;
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
		winnerPanel.Play (false);
		/*Debug.Log("Animation.count="+ waveCompletedSprite.GetClipCount());
		i =1;
		foreach (AnimationState clip in waveCompletedSprite)
		{Debug.Log ("!!!!"+i.ToString()+" "+clip.name);
			i++;
		}*/

		//waveCompletedSprite.Play ();

	}
	
	// Update is called once per frame
	void Update () {
		/*if (Time.time >= nextFalconSpawnTime)
		{
			SpawnNewEnemy();
		}*/
		if (waveActive) 
		{
			if (Time.time >= waveEndTime) //End of the wave
			{
				spawnEnemies = false;
				if (enemyCount <= 0) { 
					StartCoroutine(WaitASecond());
					//Debug.Log ("Play animation");
					waveCompletedSprite.animation.Play ("Sprite_animation");
					waveCompletedSprite.audio.Play();

				StartCoroutine(	FinishWave());
				}
			}

			if (spawnEnemies){
				if (Time.time > (lastRespawnTime+respawnInterval)) { 
					SpawnNewEnemy();
				}

			}
				}

		if (freezeTime && Time.time > freezeWait) {
			Time.timeScale = 0;
				}
	}

	void SpawnNewEnemy() 
	{			
		int enemyChoice = Random.Range (0, EnemyPrefabs.Length);

		int spawnChoice;

		//nextFalconSpawnTime += falconInterval;
		//Debug.Log ("Spawn New Enemy");
				if (EnemyPrefabs [enemyChoice].tag == "Air Enemy") {
						//Debug.Log ("Air Enemy");
						spawnChoice = Random.Range (0, falconSpawnPoint.Length);
						GameObject newFalcon = (GameObject)Instantiate (EnemyPrefabs [enemyChoice], falconSpawnPoint [spawnChoice].position, falconSpawnPoint [spawnChoice].rotation);
				} else {
					//	Debug.Log ("Ground Enemy");
						spawnChoice = Random.Range(0, tankSpawnPoint.Length);
						GameObject newTank = (GameObject)Instantiate(EnemyPrefabs[enemyChoice], tankSpawnPoint[spawnChoice].position,tankSpawnPoint[spawnChoice].rotation);
				}

		enemyCount++;
		lastRespawnTime = Time.time;
		respawnInterval = Random.Range (respawnMin, respawnMax);
		}

	void SetNextWave(){
		waveLevel++;
		difficultyMultiplier = (float)(waveLevel * waveLevel * .005) + 1;
		respawnMax = respawnMaxBase * (1 / difficultyMultiplier);
		respawnMin = respawnMinBase * (1 / difficultyMultiplier);

		if (waveLevel == theLastWave + 1) {				//Winner definition code.!!!
			SaveMaxScore();
			winnerAnimation.Play("Lose_animation");
			winnerPanel.Play(true);
			freezeWait = Time.time + 1.5f;
			freezeTime = true;
			uiscript.upgradePanelOpen = true;
				}
		}

	void StartNewWave()
	{
		HUDUpdate ();

		SpawnNewEnemy ();

		waveEndTime = Time.time + waveLenght;

		waveActive = true;
		spawnEnemies = true;
	}

	IEnumerator WaitASecond()
	{
		yield return new WaitForSeconds(1f);

	}

	IEnumerator FinishWave()
				{
					
					waveActive = false;
					yield return new WaitForSeconds(intermissionTime);
					SetNextWave();
					StartNewWave();
				}
	public void HUDUpdate()
	{
				waveLabel.text = "WAVE: " + waveLevel+"/"+theLastWave;
				scoreLabel.text = "SCORE: " + scoreCount;
				healthLabel.text = "SHIELDS: " + healthCount;
				cashLabel.text = "CASH: " + cashCount;

				uiscript.UpdateGUI ();

		}

	public void OnMenuButtons(GameObject btn)
	{
		if (btn.name == "Button_Next") {
			if (Application.loadedLevel<3)   //remove it later
				{Application.LoadLevel(Application.loadedLevel+1);}
				else 
					{Application.LoadLevel(0);}

				}
		if (btn.name == "Button_Retry") {
			Application.LoadLevel(Application.loadedLevel);
				}
	}

	public void SaveMaxScore()
	{
		int maxscore = 0;
		if (PlayerPrefs.HasKey ("MaxScore")) {
						maxscore = PlayerPrefs.GetInt ("MaxScore");
				}
		if (scoreCount > maxscore) {
						maxscore = scoreCount;
				}
		PlayerPrefs.SetInt ("MaxScore", maxscore);
		}
}
