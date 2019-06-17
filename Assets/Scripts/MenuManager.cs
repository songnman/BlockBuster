using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameObject shopPrefab;
	public GameObject shop_ItemPanelPrefab;
	public GameObject shop_SkinPanelPrefab;
	public Image shop_SkinInspectorPreview;
	 List<GameObject> shopItemList = new List<GameObject>();
	 List<GameObject> shopSkinList = new List<GameObject>();
	 List<Toggle> skinToggleList = new List<Toggle>();

	static public int CountItem1, CountItem2, CountItem3, CountItem4, CountItem5;
	static public int skinNum;
	Sprite inpectorPreviewImage;
	
	void Start()
	{
		ShopClose();
		if(skinNum < 1)
			skinNum = 1;

		for (int i = 0; i < 5; i++)
			shopItemList.Add(shop_ItemPanelPrefab.transform.GetChild(0).GetChild(i).gameObject);
		for (int i = 0; i < 25; i++)
		{
			Sprite sprite = Resources.Load<Sprite>("Icon/C_icon_" + (i + 1));
			shopSkinList.Add(shop_SkinPanelPrefab.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject);
			skinToggleList.Add(shopSkinList[i].GetComponent<Toggle>());	//[2019-06-02 15:18:56] 토글리스트 생성
			if (shopSkinList[i].transform.GetChild(1).GetComponent<Text>() != null)
				Destroy(shopSkinList[i].transform.GetChild(1).gameObject);	//[2019-06-02 14:50:51] 라벨을 부숨

			GameObject go = new GameObject();
			go.AddComponent<Image>().sprite = sprite;
			go.name = "Skin" + (i + 1);
			go.transform.SetParent(shopSkinList[i].transform);
			go.transform.transform.localPosition = Vector3.zero;
			go.transform.transform.localScale = new Vector3(2,2,0);
		}
		LoadGame();
		SetItemCountText();

		inpectorPreviewImage = Resources.Load<Sprite>("Icon/C_icon_" + skinNum);

	}
	public void SelectSkin()
	{
		shop_SkinInspectorPreview.sprite = skinToggleList.Find(i => i.isOn).transform.GetChild(1).GetComponent<Image>().sprite;
	}
	public void EquipSkin()
	{
		// Debug.Log(skinToggleList.FindIndex(i => i.isOn));
		skinNum = skinToggleList.FindIndex(i => i.isOn) + 1;
		inpectorPreviewImage = Resources.Load<Sprite>("Icon/C_icon_" + skinNum);
		SetItemCountText();
	}
	public void ResetGame()
	{
		// LoadGame();
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
		SetItemCountText();
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
	}
	public void BuyItem2()
	{
		CountItem2++;
		SetItemCountText();
	}
	public void BuyItem3()
	{
		CountItem3++;
		SetItemCountText();
	}
	public void BuyItem4()
	{
		CountItem4++;
		SetItemCountText();
	}
	public void ButItem5()
	{
		CountItem5++;
		SetItemCountText();
	}
	public void ResetSaveGame()
	{
		CountItem1 = 0;
		CountItem2 = 0;
		CountItem3 = 0;
		CountItem4 = 0;
		CountItem5 = 0;
		skinNum = 1;
		SetItemCountText();
	}
	static public Save CreateSaveGameObject()
	{
		Save save = new Save();

		save.CountItem1 = CountItem1;
		save.CountItem2 = CountItem2;
		save.CountItem3 = CountItem3;
		save.CountItem4 = CountItem4;
		save.CountItem5 = CountItem5;
		save.skinNum = skinNum;
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
			skinNum = save.skinNum;
		}
	}
	public void SetItemCountText()
	{
		shop_SkinInspectorPreview.sprite = inpectorPreviewImage;
/*
		shopItemList[0].GetComponent<ShopItem>().ItemCount = CountItem1;
		shopItemList[1].GetComponent<ShopItem>().ItemCount = CountItem2;
		shopItemList[2].GetComponent<ShopItem>().ItemCount = CountItem3;
		shopItemList[3].GetComponent<ShopItem>().ItemCount = CountItem4;
		shopItemList[4].GetComponent<ShopItem>().ItemCount = CountItem5;
		for (int i = 0; i < shopItemList.Count; i++)
			shopItemList[i].transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = shopItemList[i].GetComponent<ShopItem>().ItemCount.ToString();
*/
shopItemList[0].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem1.ToString();
shopItemList[1].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem2.ToString();
shopItemList[2].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem3.ToString();
shopItemList[3].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem4.ToString();
shopItemList[4].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem5.ToString();
		SaveGame();
	}
	
}
