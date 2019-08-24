using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchGunManager : MonoBehaviour
{
	public GameObject menu;
	public GameObject selectionFocus;
	public Transform gunItemsContainer;

	private bool isPaused;
	private int selectedIdx;
	private GunType[] allGunTypes = new GunType[]{};
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
		{
			menu.SetActive(true);
			populateGunItems();
			pauseGame();
			isPaused = true;
		}

		if (Input.GetKeyDown(KeyCode.Tab) && !isPaused)
		{
			GunStore.SwitchGun();
		}

		if (isPaused)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow) && selectedIdx > 0)
			{
				selectedIdx--;
				moveFocus();
			}else if (Input.GetKeyDown(KeyCode.RightArrow) && selectedIdx < allGunTypes.Length - 1)
			{
				selectedIdx++;
				moveFocus();
			}else if (Input.GetKeyDown(KeyCode.Space))
			{
				GunStore.SwitchGun(allGunTypes[selectedIdx]);
				resetAndHideMenu();
				resumeGame();
			}
		}
	}

	void populateGunItems()
	{
		var gunDict = GunStore.GetGunStoreStatus();
		allGunTypes = new GunType[gunDict.Count];
		var i = 0;
		foreach (KeyValuePair<GunType, int> kvp in gunDict)
		{
			string imgName = "images/collectables/bullet-blocks/" + GunConstants.typeToBlockImgName[kvp.Key];
			setItemIconAndCount(i,imgName,kvp.Value);
			setItemActive(i,true);
			allGunTypes[i] = kvp.Key;
			
			if (kvp.Key == GunStore.currentGunType)
			{
				selectedIdx = i;
				moveFocus();
			}
			
			i++;
		}
	}

	void moveFocus()
	{
		var item = gunItemsContainer.GetChild(selectedIdx);
		var icon = item.GetChild(0);
		selectionFocus.transform.position = icon.position;
	}

	void setItemIconAndCount(int idx, string imgName, int count)
	{
		var item = gunItemsContainer.GetChild(idx);
		var icon = item.GetChild(0);
		icon.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> (imgName);
		var countTxt = item.GetChild(1);
		var t = count > 0 ? count + "" : "";
		countTxt.gameObject.GetComponent<Text>().text = t;
	}

	void setItemActive(int idx, bool active)
	{
		var item = gunItemsContainer.GetChild(idx);
		item.gameObject.SetActive(active);
	}

	void resetAndHideMenu()
	{
		menu.SetActive(false);
		// set all items to inactive
		for (int i = 0; i < gunItemsContainer.childCount; i++)
		{
			setItemActive(i,false);
		}
		// clear variable
		isPaused = false;
		allGunTypes = new GunType[]{};
	}

	void pauseGame()
	{
		Time.timeScale = 0;
	}

	void resumeGame()
	{
		Time.timeScale = 1;
	}
}
