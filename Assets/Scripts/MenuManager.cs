using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class MenuManager : MonoBehaviour
{
	public GameObject shopPrefab;
	public GameObject shop_ItemPanelPrefab;
	public GameObject shop_SkinPanelPrefab;
	static List<GameObject> shopItemList = new List<GameObject>();
	static public int CountItem1, CountItem2, CountItem3, CountItem4, CountItem5;
	void Start()
	{
		ShopClose();
		
		for (int i = 0; i < 5; i++)
			shopItemList.Add(shop_ItemPanelPrefab.transform.GetChild(0).GetChild(i).gameObject);
		
		LoadGame();
		SetItemCountText();
	}

	public void ResetGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
	}
	public void ShopOpen()
	{
		shopPrefab.SetActive(true);
		SetItemCountText();
	}
	public void ShopClose()
	{
		shopPrefab.SetActive(false);
	}
	public void ToggleItem()
	{
		shop_ItemPanelPrefab.SetActive(true);
		shop_SkinPanelPrefab.SetActive(false);
	}
	public void ToggleSkin()
	{
		shop_ItemPanelPrefab.SetActive(false);
		shop_SkinPanelPrefab.SetActive(true);
	}	
	public void BuyItem1()
	{
		CountItem1++;
		SetItemCountText();
		SaveGame();
	}
	public void BuyItem2()
	{
		CountItem2++;
		SetItemCountText();
		SaveGame();
	}
	public void BuyItem3()
	{
		CountItem3++;
		SetItemCountText();
		SaveGame();
	}
	public void BuyItem4()
	{
		CountItem4++;
		SetItemCountText();
		SaveGame();
	}
	public void ButItem5()
	{
		CountItem5++;
		SetItemCountText();
		SaveGame();
	}
	public void ResetSaveGame()
	{
		CountItem1 = 0;
		CountItem2 = 0;
		CountItem3 = 0;
		CountItem4 = 0;
		CountItem5 = 0;
		SetItemCountText();
		SaveGame();
	}
	static public Save CreateSaveGameObject()
	{
		Save save = new Save();

		save.CountItem1 = CountItem1;
		save.CountItem2 = CountItem2;
		save.CountItem3 = CountItem3;
		save.CountItem4 = CountItem4;
		save.CountItem5 = CountItem5;

		return save;
	}
	static public void SaveGame()
	{
		Save save = CreateSaveGameObject();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
		bf.Serialize(file, save);
		file.Close();
	}
	static public void LoadGame()
	{
		if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			Save save = (Save)bf.Deserialize(file);
			file.Close();
			
			CountItem1 = save.CountItem1;
			CountItem2 = save.CountItem2;
			CountItem3 = save.CountItem3;
			CountItem4 = save.CountItem4;
			CountItem5 = save.CountItem5;
		}
	}
	static public void SetItemCountText()
	{
		shopItemList[0].GetComponent<ShopItem>().ItemCount = CountItem1;
		shopItemList[1].GetComponent<ShopItem>().ItemCount = CountItem2;
		shopItemList[2].GetComponent<ShopItem>().ItemCount = CountItem3;
		shopItemList[3].GetComponent<ShopItem>().ItemCount = CountItem4;
		shopItemList[4].GetComponent<ShopItem>().ItemCount = CountItem5;
		for (int i = 0; i < shopItemList.Count; i++)
			shopItemList[i].transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = shopItemList[i].GetComponent<ShopItem>().ItemCount.ToString();
	}
	
}
