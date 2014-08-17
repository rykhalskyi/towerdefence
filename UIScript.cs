using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {

	public LevelMaster levelMaster;
	//Toggle panel logic
	public TweenRotation ToggleButtonTween;
	public TweenPosition BuildPanelTween;

	private bool isBuildPanel = false;



	public bool upgradePanelOpen = false;

	//Placement items
	public Transform placementPlanesGroup;
	public Material hoverMat;
	public LayerMask placementLayer;

	private Material originalMat;
	private GameObject lastHitObj;

	//buton colors
	public Color onColor;
	public Color offColor;
	public GameObject[] allStructures;
	public UISprite[] buildButtons;

	private int structureIndex = 0;
	private PlacementPlain focusedPlain;
//upgrades

	public UILabel upgradeText;
	public UIButton upgradeBtn;
	public TweenPosition upgradePanelTweener;
	public TweenPosition exitPanelTweener;
	private BaseTurret turretToUpgrade;
	private int upgradeCost = 0;

	public AstarPath astar;

	// Use this for initialization
	void Start () {
		structureIndex = 0;
		UpdateGUI ();
		upgradePanelTweener.Play (true);
		upgradePanelTweener.enabled = false;

		exitPanelTweener.Play (true);
		exitPanelTweener.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		///

		///
		if (isBuildPanel && !upgradePanelOpen) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hit;

						if (Physics.Raycast (ray, out hit, 1000, placementLayer)) {
								if (lastHitObj) {
										lastHitObj.renderer.material = originalMat;
								}

								lastHitObj = hit.collider.gameObject;
								originalMat = lastHitObj.renderer.material;
								lastHitObj.renderer.material = hoverMat;
						} else {
								if (lastHitObj) {
										lastHitObj.renderer.material = originalMat;
										lastHitObj = null;
								}
						}
			if (Input.GetMouseButtonDown(0) && lastHitObj ) {
				focusedPlain = (PlacementPlain)lastHitObj.GetComponent("PlacementPlain");
				//Debug.Log("Click!");

					if (lastHitObj.tag == "Placement_Open" && levelMaster.WeaponPrices[structureIndex]<=levelMaster.cashCount &&
				    levelMaster.WeaponAvailibility[structureIndex] <= levelMaster.waveLevel)
				{
					//Debug.Log("Placement Open");
					Vector3 vec3;

					// fixing pivot point mistake
					if (structureIndex == 1) {
						vec3 = new Vector3(lastHitObj.transform.position.x, lastHitObj.transform.position.y+0.30f,lastHitObj.transform.position.z); //if launcher move little bit up
					}
					else {vec3 = new Vector3(lastHitObj.transform.position.x, lastHitObj.transform.position.y,lastHitObj.transform.position.z);}



					GameObject newStructure = (GameObject)Instantiate(allStructures[structureIndex], lastHitObj.transform.position,Quaternion.identity);
					newStructure.transform.position = vec3;

					newStructure.transform.localEulerAngles = new Vector3(newStructure.transform.localEulerAngles.x, Random.Range(0,360), newStructure.transform.localEulerAngles.z);
					newStructure.tag="Obstacles";
					//newStructure.layer = 10;
					lastHitObj.tag = "Placement_Taken";
					focusedPlain.myStructure = newStructure;

					levelMaster.cashCount -= levelMaster.WeaponPrices[structureIndex];
					levelMaster.HUDUpdate();


					astar.Scan();
					Debug.Log("Scan");
					foreach (GameObject tnk in GameObject.FindGameObjectsWithTag("Ground Enemy")) {
						Tank tnkScript = (Tank)tnk.GetComponent("Tank");
						tnkScript.FindNewPath();

					}
					//UpdateGUI();
				}
/*UPGRADE*/				else if (lastHitObj.tag != "Placement_Open") {
					//upgrade turret

					turretToUpgrade = (BaseTurret) focusedPlain.myStructure.GetComponent("BaseTurret");
					if (turretToUpgrade.updateObject) 
					{
							upgradeCost = turretToUpgrade.updateCost;
							upgradeText.text = "Upgrade to\n "+turretToUpgrade.name+" $"+turretToUpgrade.updateCost+"?";
							CheckUpgradeAvailible();
							upgradePanelTweener.enabled = true;
							upgradePanelTweener.Play(true);
							upgradePanelOpen = true;
					}

					//Debug.Log ("upgrade panel");
				}

			}
		} //isBuildPanel 

	}

	private bool CheckUpgradeAvailible()
	{
		bool upgrade = false;
		UILabel lbl = (UILabel)upgradeBtn.transform.Find("Label").gameObject.GetComponent("UILabel");
		if (levelMaster.cashCount >= upgradeCost) {

						lbl.color = Color.green;
						upgrade = true;
						upgradeBtn.collider.enabled = true;
				} else {
						lbl.color = Color.gray;
						upgradeBtn.collider.enabled = false;
				}
		return upgrade;
		}

	public void UpgradeTurret(){

		GameObject newStructure = (GameObject)Instantiate (turretToUpgrade.updateObject, lastHitObj.transform.position, Quaternion.identity);
		levelMaster.cashCount -= turretToUpgrade.updateCost;
		Destroy (turretToUpgrade.gameObject);
		//newStructure.tag = "Obstacles";
		//newStructure.layer = 10;
		focusedPlain.myStructure = newStructure;
		turretToUpgrade = null;
		CloseUpgradePanel ();
		levelMaster.HUDUpdate();
		}

	public void CloseUpgradePanel()
	{
		upgradePanelTweener.Play (false);
		upgradePanelOpen = false;
	//	Debug.Log ("NO clicked");

		}

	public void UpdateGUI()
	{
		//Debug.Log ("UpdateGUI");
		foreach (UISprite sprite in buildButtons) {
			sprite.color = offColor;

		}
		foreach (UILabel lbl in levelMaster.WeaponPricesLabels) {
			lbl.color = offColor;		
		}
		Debug.Log ("UPD"+structureIndex);
		buildButtons [structureIndex].color = onColor;
		levelMaster.WeaponPricesLabels [structureIndex].color = onColor;

		for (int i=0; i<buildButtons.Length; i++) {
			if (levelMaster.cashCount<levelMaster.WeaponPrices[i] || levelMaster.WeaponAvailibility[i]>levelMaster.waveLevel)
			{buildButtons[i].color = Color.gray;
				levelMaster.WeaponPricesLabels[i].color = Color.gray;
				//levelMaster.WeaponPricesLabels[i].text = "$"+levelMaster.WeaponPrices[i];
			}
				}


	}


	void ToggleButtonPanel()
	{
		//Debug.Log ("Toggle panel"+isBuildPanel);
		if (!isBuildPanel) {

			foreach (Transform thePlane in placementPlanesGroup)
			{
				thePlane.gameObject.renderer.enabled = true;
			}

		//	Debug.Log ("open");
			BuildPanelTween.Play(false);
			ToggleButtonTween.Play(false);
				}
		else {

			foreach (Transform thePlane in placementPlanesGroup)
			{
				thePlane.gameObject.renderer.enabled = false;
			}
			//Debug.Log ("close");
			BuildPanelTween.Play(true);
			ToggleButtonTween.Play (true);
				}
		isBuildPanel = !isBuildPanel;

	}

	void SetBuildChoice(GameObject btn) {

		if (btn.name == "Button_cannon") {
			structureIndex = 0; }
		if (btn.name == "Button_launcher") {
			structureIndex = 1; }
		if (btn.name == "Button_laser") {
			structureIndex = 2;
				}

		UpdateGUI ();
	}

	public void PauseGame(GameObject btn)
	{

		upgradePanelOpen = true;
		exitPanelTweener.enabled = true;
		exitPanelTweener.Play (true);
		Time.timeScale = 0;

		}

	public void OnExitPanel(GameObject btn)
	{
		//Debug.Log ("ON EXIT PANEL");
		if (btn.name == "Button_Resume") {
		//	Debug.Log ("Resume Game");
			Time.timeScale =1;
			exitPanelTweener.Play(false);
			upgradePanelOpen = false;
				}
		if (btn.name == "Button_Exit") {
			Time.timeScale = 1;
			Application.LoadLevel(0);
				}
	}


}
