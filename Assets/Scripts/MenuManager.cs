using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[Header("Shop Object")]
	public GameObject shopPrefab;
	public GameObject settingPrefab;
	public GameObject pushTogglePrefab;
	public GameObject termsOfServicePrefab;
	public GameObject shop_ItemPanelPrefab;
	public GameObject shop_SkinPanelPrefab;
	public GameObject shop_SkinBuyBtnPrefab , shop_SkinEquipBtnPrefab;
	public Image shop_SkinInspectorPreview;
	List<GameObject> shopItemList = new List<GameObject>();
	List<GameObject> shopSkinList = new List<GameObject>();
	List<Toggle> skinToggleList = new List<Toggle>();
	
	[Header("Popup Object")]
	public GameObject popup_DailyReward;
	public GameObject popup_HottimeReward;
	public GameObject popup_AdViewReward;
	public GameObject popup_ResetAsk;
	
	[Header("ETC Object")]
	public Text currency0Text;
	public Text currency1Text;
	public Sprite tempSprite, tempSprite2;
	public Image blackLayer;

	/////////////////////////////////////////////////////////////////////////////
	static public int CountItem1, CountItem2, CountItem3, CountItem4, CountItem5;
	static public int currency0, currency1;
	static public List<bool> isHaveSkinList = new List<bool>();
	static public int skinNum;
	static public int viewAdDay, viewAdMonth, viewAdCount, loginDay, loginMonth;
	static bool isGetDailyReward, isGetHottimeReward;
	static bool isSetAlarm, isAllowTerms;
	Sprite inpectorPreviewImage;
	int item1value = 1000,	item2value = 400,	item3value = 150,	item4value = 160, item5value = 100;
	IEnumerator StartFade()
	{
		for (int i = 0; i < 51; i++)
		{
			blackLayer.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(new Color (1,1,1,0), new Color (1,1,1,1), i * 0.02f);
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 51; i++)
		{
			blackLayer.color = Color.Lerp(new Color (0,0,0,1), new Color (0,0,0,0), i * 0.02f);
			blackLayer.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(new Color (1,1,1,1), new Color (1,1,1,0), i * 0.02f);
			yield return new WaitForFixedUpdate();
		}
		Destroy(blackLayer.gameObject);
	}
	void Awake() 
	{
		blackLayer.gameObject.SetActive(true);
		StartCoroutine(StartFade());
	}
	void Start()
	{
		Debug.Log(DateTime.UtcNow);
		ShopClose();
		// if(!isSetAlarm)
		// {
		// 	Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(System.TimeSpan.FromSeconds(5), "블록버스터", "핫타임 보상을 수령하세요!", new Color(0, 0.6f, 1), Assets.SimpleAndroidNotifications.NotificationIcon.Message);
		// 	isSetAlarm = true;
		// }
		if(skinNum < 1)
			skinNum = 1;

		for (int i = 0; i < 5; i++)
			shopItemList.Add(shop_ItemPanelPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject);

		for (int i = 0; i < 25; i++)
		{
			shopSkinList.Add(shop_SkinPanelPrefab.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject);
			skinToggleList.Add(shopSkinList[i].GetComponent<Toggle>());	//[2019-06-02 15:18:56] 토글리스트 생성
			isHaveSkinList.Add(false);

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
			go2.transform.localScale = new Vector3(0.8f,0.8f,1);

		}
		if (File.Exists(Application.persistentDataPath + "/gamesave.save") == false)
			ResetSaveGame();
		
		LoadGame();
		
		if(DateTime.UtcNow.Day != loginDay || DateTime.UtcNow.Month != loginMonth)
		{
			loginDay			= DateTime.UtcNow.Day;
			loginMonth			= DateTime.UtcNow.Month;
			isGetDailyReward	= false;
			isGetHottimeReward	= false;
		}

		if(!isGetDailyReward)
		{
			isGetDailyReward = true;
			popup_DailyReward.SetActive(true);
			currency1 += 10;
		}
		if(!isGetHottimeReward && DateTime.UtcNow.Hour >= 9 && DateTime.UtcNow.Hour < 13)
		{
			isGetHottimeReward = true;
			popup_HottimeReward.SetActive(true);
			currency0 += 10;
			currency1 += 10;
		}

		if(!isAllowTerms)
			termsOfServicePrefab.SetActive(true);
		else
			termsOfServicePrefab.SetActive(false);

		SetItemCountText();
	}
	public void NewGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
	}
	public void CloseDailyReward()
	{
		popup_DailyReward.SetActive(false);
	}
	public void CloseHottimeReward()
	{
		popup_HottimeReward.SetActive(false);
	}
	public void CloseAdViewReward()
	{
		popup_AdViewReward.SetActive(false);
	}
	public void CloseResetAsk()
	{
		popup_ResetAsk.SetActive(false);
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
		settingPrefab.SetActive(false);
	}
	public void CreditsOpen()
	{
		if(isSetAlarm)
			pushTogglePrefab.GetComponent<Toggle>().isOn = true;
		else
			pushTogglePrefab.GetComponent<Toggle>().isOn = false;
		settingPrefab.SetActive(true);
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
		currency1 -= item1value;
		SetItemCountText();
	}
	public void BuyItem2()
	{
		CountItem2++;
		currency1 -= item2value;
		SetItemCountText();
	}
	public void BuyItem3()
	{
		CountItem3++;
		currency1 -= item3value;
		SetItemCountText();
	}
	public void BuyItem4()
	{
		CountItem4++;
		currency1 -= item4value;
		SetItemCountText();
	}
	public void BuyItem5()
	{
		CountItem5++;
		currency1 -= item5value;
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
		viewAdCount = 0;
		isGetDailyReward = false;
		isGetHottimeReward = false;
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
		Assets.SimpleAndroidNotifications.NotificationManager.CancelAll();
		isAllowTerms = false;
		SaveGame();
		SetItemCountText();
		UnityEngine.SceneManagement.SceneManager.LoadScene("App_Scene");
	}
	public void TogglePush()
	{
		if(termsOfServicePrefab.transform.GetChild(4).GetComponent<Toggle>().isOn == true)
			isSetAlarm = true;
		else
			isSetAlarm = false;
	}
	public void ToggleAllowTerms()
	{
		if(termsOfServicePrefab.transform.GetChild(2).GetComponent<Toggle>().isOn)
		{
			isAllowTerms = true;
			termsOfServicePrefab.transform.GetChild(0).GetComponent<Button>().interactable = true;
		}
		else
		{
			isAllowTerms = false;
			termsOfServicePrefab.transform.GetChild(0).GetComponent<Button>().interactable = false;
		}
	}
	public void OpenTermsOfService()
	{
		Application.OpenURL("https://docs.google.com/document/d/1Dc9a0AlZ8sQqCsV9AHuX9XeyE2w9U_dZbSW1-gMhcBs");
	}
	public void OpenToermsOfPush()
	{
		Application.OpenURL("https://docs.google.com/document/d/1m_ZT_mOWr4TWCTbzPIO4T4VcM5f6JtZFQv198A5pQVA");
	}
	public void ConfirmTerms()
	{
		SaveGame();
		if(isSetAlarm)
		{
			Assets.SimpleAndroidNotifications.NotificationManager.CancelAll();
			Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(System.TimeSpan.FromSeconds(5), "블록버스터", "핫타임 보상을 수령하세요!", new Color(0, 0.6f, 1), Assets.SimpleAndroidNotifications.NotificationIcon.Message);
		}
		else
		{
			Assets.SimpleAndroidNotifications.NotificationManager.CancelAll();
		}
		termsOfServicePrefab.SetActive(false);
	}
	public void OpenResetAsk()
	{
		popup_ResetAsk.SetActive(true);
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
		save.viewAdDay = viewAdDay;
		save.viewAdMonth = viewAdMonth;
		save.viewAdCount = viewAdCount;
		save.loginDay = loginDay;
		save.loginMonth = loginMonth;
		save.isGetDailyReward = isGetDailyReward;
		save.isGetHottimeReward = isGetHottimeReward;
		save.isSetAlarm = isSetAlarm;
		save.isAllowTerms = isAllowTerms;
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
			
			CountItem1			= save.CountItem1;
			CountItem2			= save.CountItem2;
			CountItem3			= save.CountItem3;
			CountItem4			= save.CountItem4;
			CountItem5			= save.CountItem5;
			skinNum				= save.skinNum;
			currency0			= save.currency0;
			currency1			= save.currency1;
			isHaveSkinList		= save.isHaveSkinList;
			viewAdDay			= save.viewAdDay;
			viewAdMonth			= save.viewAdMonth;
			viewAdCount			= save.viewAdCount;
			loginDay			= save.loginDay;
			loginMonth			= save.loginMonth;
			isGetDailyReward	= save.isGetDailyReward;
			isGetHottimeReward	= save.isGetHottimeReward;
			isSetAlarm			= save.isSetAlarm;
			isAllowTerms		= save.isAllowTerms;
		}
		else
		{
			SaveGame();
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
		for (int i = 0; i < shopItemList.Count; i++)
		{
			shopItemList[i].transform.GetChild(2).GetComponent<Button>().interactable = true;
		}
		if(currency1 < item1value)
			shopItemList[0].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency1 < item2value)
			shopItemList[1].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency1 < item3value)
			shopItemList[2].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency1 < item4value)
			shopItemList[3].transform.GetChild(2).GetComponent<Button>().interactable = false;
		if(currency1 < item5value)
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
