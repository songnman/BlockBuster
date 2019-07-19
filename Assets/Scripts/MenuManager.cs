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
	public GameObject shop_SkinBuyBtnPrefab , shop_SkinEquipBtnPrefab;
	public Image shop_SkinInspectorPreview;
	public Text currency0Text, currency1Text;
	 List<GameObject> shopItemList = new List<GameObject>();
	 List<GameObject> shopSkinList = new List<GameObject>();
	 List<Toggle> skinToggleList = new List<Toggle>();

	static public int CountItem1, CountItem2, CountItem3, CountItem4, CountItem5;
	static public int currency0, currency1;
	static public List<bool> isHaveSkinList = new List<bool>();
	static public int skinNum;
	Sprite inpectorPreviewImage;
	int item1value = 1000,	item2value = 400,	item3value = 150,	item4value = 160, item5value = 100;
	public Sprite tempSprite, tempSprite2;
	void Start()
	{
		ShopClose();
			
		if(skinNum < 1)
			skinNum = 1;

		for (int i = 0; i < 5; i++)
			shopItemList.Add(shop_ItemPanelPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject);

		for (int i = 0; i < 25; i++)
		{
			shopSkinList.Add(shop_SkinPanelPrefab.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject);
			skinToggleList.Add(shopSkinList[i].GetComponent<Toggle>());	//[2019-06-02 15:18:56] 토글리스트 생성
			isHaveSkinList.Add(false);

			
			// if (shopSkinList[i].transform.GetChild(1).GetComponent<Text>() != null)
			// Destroy(shopSkinList[i].transform.GetChild(1).gameObject);	//[2019-06-02 14:50:51] 라벨을 부숨
			// Debug.Log((i+1) +" "+ isHaveSkinList[i]);
			
			// Sprite sprite = Resources.Load<Sprite>("Icon/Icon_lock");
			// sprite = Resources.Load<Sprite>("Icon/Icon_lock");
			// GameObject go = new GameObject();
			// go.AddComponent<Image>().sprite = sprite;
			// go.name = "Icon_lock";
			// go.transform.SetParent(shopSkinList[i].transform);
			// go.transform.localPosition =  new Vector2(-90,50);
			// go.GetComponent<RectTransform>().sizeDelta = new Vector2(80,100);
			// go.transform.localScale = new Vector3(1,1,1);

			GameObject go = new GameObject();
			go.AddComponent<Image>().sprite = tempSprite;
			go.GetComponent<Image>().color = new Color(0,0,0,0.6f);
			go.name = "Black_Layer";
			go.transform.SetParent(shopSkinList[i].transform);
			go.transform.localPosition = Vector2.zero;
			go.GetComponent<RectTransform>().sizeDelta = new Vector2(189,170);
			go.transform.localScale = new Vector3(1,1,1);
			
			GameObject go2 = new GameObject();
			go2.AddComponent<Image>().sprite = tempSprite2;
			go2.name = "Icon_lock";
			go2.transform.SetParent(go.transform);
			go2.transform.localPosition =  new Vector2(-90,50);
			go2.GetComponent<RectTransform>().sizeDelta = new Vector2(80,100);
			go2.transform.localScale = new Vector3(1,1,1);

		}
		if (File.Exists(Application.persistentDataPath + "/gamesave.save") == false)
			ResetSaveGame();
		LoadGame();
		SetItemCountText();


	}
	public void SelectSkin()
	{
		//[2019-06-20 01:10:07] 선택한 스킨을 프리뷰에 뿌려줌
		shop_SkinInspectorPreview.sprite = Resources.Load<Sprite>("Icon/C_icon_" + (skinToggleList.FindIndex(i => i.isOn) + 1));

		// shop_SkinInspectorPreview.sprite = skinToggleList.Find(i => i.isOn).transform.GetChild(1).GetComponent<Image>().sprite;
		// GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Balls/Ball_" + (skinToggleList.FindIndex(i => i.isOn) + 1)));
		// go.transform.SetParent(shop_SkinInspectorPreview.transform);
		// go.transform.localPosition = Vector2.zero;
		
		
		//[2019-06-20 01:09:52] 선택한 스킨의 가격을 측정.
		shop_SkinInspectorPreview.transform.parent.GetChild(2).GetComponent<Text>().text = CalcSkinCost().ToString("N0");
		
		//[2019-06-20 01:08:42] 선택한 스킨을 갖고있는지 판단해서 구입 버튼과 가격을 숨김
		if(isHaveSkinList[skinToggleList.FindIndex(i => i.isOn)])
		{
			shop_SkinBuyBtnPrefab.SetActive(false);
			shop_SkinInspectorPreview.transform.parent.GetChild(2).gameObject.SetActive(false);
		}
		else
		{
			shop_SkinBuyBtnPrefab.SetActive(true);
			shop_SkinInspectorPreview.transform.parent.GetChild(2).gameObject.SetActive(true);
		}
		
		//[2019-06-20 01:09:28] 돈이 충분하지 않으면 버튼을 비활성화 시킴
		if(currency0 < CalcSkinCost())
			shop_SkinBuyBtnPrefab.GetComponent<Button>().interactable = false;
		else
			shop_SkinBuyBtnPrefab.GetComponent<Button>().interactable = true;
		
		//[2019-06-20 01:11:09] 현재 착용한 스킨을 선택 할 시 버튼을 비활성화 시키고 버튼 TEXT를 바꿈
		if(skinNum == skinToggleList.FindIndex(i => i.isOn) + 1)
		{
			shop_SkinEquipBtnPrefab.transform.GetChild(0).GetComponent<Text>().text = "EQUIPED";
			shop_SkinEquipBtnPrefab.GetComponent<Button>().interactable = false;
		}
		else
		{
			shop_SkinEquipBtnPrefab.transform.GetChild(0).GetComponent<Text>().text = "EQUIP";
			shop_SkinEquipBtnPrefab.GetComponent<Button>().interactable = true;
		}
	}
	public void BuySkin()
	{
		currency0 -= CalcSkinCost();
		isHaveSkinList[skinToggleList.FindIndex(i => i.isOn)] = true;
		/*
		for (int i = 0; i < 25; i++)
		{
			Debug.Log((i+1) +" "+ isHaveSkinList[i]);
		}
		//*/
		SetItemCountText();
		SelectSkin();
	}
	int CalcSkinCost()
	{
		int indexNum = skinToggleList.FindIndex(i => i.isOn);
		if		(indexNum < 3)
			return 300;
		else if	(indexNum < 5)
			return 500;
		else if	(indexNum < 9)
			return 1000;
		else if	(indexNum < 13)
			return 1500;
		else if	(indexNum < 17)
			return 2500;
		else if	(indexNum < 21)
			return 5000;
		else
			return 10000;
	}
	public void EquipSkin()
	{
		// Debug.Log(skinToggleList.FindIndex(i => i.isOn));
		if(skinToggleList.FindIndex(i => i.isOn) > -1)
		{
			skinNum = skinToggleList.FindIndex(i => i.isOn) + 1;
			inpectorPreviewImage = Resources.Load<Sprite>("Icon/C_icon_" + skinNum);
			SetItemCountText();
			SelectSkin();
		}
	}
	public void ResetGame()
	{
		// LoadGame();
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
	}
	public void ShopOpen()
	{
		shopPrefab.SetActive(true);
		inpectorPreviewImage = Resources.Load<Sprite>("Icon/C_icon_" + skinNum);
		skinToggleList[skinNum - 1].isOn = true;
		SelectSkin();
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
		currency0 -= item1value;
		SetItemCountText();
	}
	public void BuyItem2()
	{
		CountItem2++;
		currency0 -= item2value;
		SetItemCountText();
	}
	public void BuyItem3()
	{
		CountItem3++;
		currency0 -= item3value;
		SetItemCountText();
	}
	public void BuyItem4()
	{
		CountItem4++;
		currency0 -= item4value;
		SetItemCountText();
	}
	public void BuyItem5()
	{
		CountItem5++;
		currency0 -= item5value;
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
		currency0 = 0;
		currency1 = 0;
		
		isHaveSkinList = new List<bool>();
		for (int i = 0; i < 25; i++)
		{
			if(i == 0)
				isHaveSkinList.Add(true);
			else
				isHaveSkinList.Add(false);
			// Debug.Log((i+1) +" "+ isHaveSkinList[i]);
		}
		Debug.Log("isHaveSkinList.Count = " + isHaveSkinList.Count);
		SaveGame();
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
		save.currency0 = currency0;
		save.currency1 = currency1;
		save.isHaveSkinList = isHaveSkinList;
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
			
			CountItem1		= save.CountItem1;
			CountItem2		= save.CountItem2;
			CountItem3		= save.CountItem3;
			CountItem4		= save.CountItem4;
			CountItem5		= save.CountItem5;
			skinNum			= save.skinNum;
			currency0		= save.currency0;
			currency1		= save.currency1;
			isHaveSkinList	= save.isHaveSkinList;
		}
	}
	public void SetItemCountText()
	{
		shop_SkinInspectorPreview.sprite = inpectorPreviewImage;
		shopItemList[0].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem1.ToString();
		shopItemList[1].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem2.ToString();
		shopItemList[2].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem3.ToString();
		shopItemList[3].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem4.ToString();
		shopItemList[4].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = CountItem5.ToString();
		currency0Text.text = currency0.ToString();
		currency1Text.text = currency1.ToString();
		
		//[2019-06-19 00:20:41] 아이템 구입 가능여부 판단
		if(currency0 < item1value)
			shopItemList[0].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency0 < item2value)
			shopItemList[1].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency0 < item3value)
			shopItemList[2].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency0 < item4value)
			shopItemList[3].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency0 < item5value)
			shopItemList[4].transform.GetChild(2).GetComponent<Button>().interactable = false;

		for (int i = 0; i < 25; i++)
		{
			GameObject Balck_Layer = shopSkinList[i].transform.GetChild(3).gameObject;
			if(isHaveSkinList[i] == true)
				Balck_Layer.SetActive(false);
		}

		SaveGame();
	}
	
}
