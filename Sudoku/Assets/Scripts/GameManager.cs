using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform ItemParent;
    public GameObject ItemPrefab;

    public GameObject[] SelectItems;
    public Transform SelectPanel;
    public CanvasGroup SelectPanelCanvasGroup;

    public GameObject CannotPlayMask;

    public Button ReturnGameConfig;

    private List<ItemData> itemDatas;
    private List<ItemData> PlayerUseItemDatas;

    private ItemData currentItemData;

    private Dictionary<int, int> updateSelectItemDict = new Dictionary<int, int>();

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CannotPlayMask.SetActive(true);

        StartGame();

        ReturnGameConfig.onClick.AddListener(ReturnGameConfigEvent);
    }

    private void OnDestroy()
    {
        ReturnGameConfig.onClick.RemoveListener(ReturnGameConfigEvent);
    }

    private void ReturnGameConfigEvent()
    {
        SceneManager.LoadScene("GameConfig");
    }

    private void StartGame()
    {
        GameInitManager.Instance.InitMapData();

        CreateMapData();
    }

    public void CreateMapData()
    {
        GameInitManager.Instance.CreateInitMapData();
    }

    public void CreateUI()
    {
        itemDatas = new List<ItemData>();
        foreach (var child in GameInitManager.Instance.Map)
        {
            GameObject item = Instantiate(ItemPrefab, ItemParent);
            item.SetActive(false);
            ItemData itemData = item.GetComponent<ItemData>();
            itemData.SetValueTextData(child.baseValue);
            itemData.mapData = child;
            itemDatas.Add(itemData);

            itemData.itemBgImg.color = GameConfig.OddLineColor;
            itemData.SetInitColor(GameConfig.OddLineColor);
            if ((child.rowIndex >= 3 && child.rowIndex <= 5 && ((child.columnIndex >= 0 && child.columnIndex <= 2) || (child.columnIndex >= 6 && child.columnIndex <= 8))) || (((child.rowIndex >= 0 && child.rowIndex <= 2) || (child.rowIndex >= 6 && child.rowIndex <= 8)) && (child.columnIndex >= 3 && child.columnIndex <= 5)))
            {
                itemData.itemBgImg.color = GameConfig.EvenLineColor;
                itemData.SetInitColor(GameConfig.EvenLineColor);
            }
        }

        CreatePlayerUseUI();
    }

    private void CreatePlayerUseUI()
    {
        if (GameConfig.GameLevel == 0)
        {
            return;
        }

        PlayerUseItemDatas = new List<ItemData>();

        int clearNum = GameConfig.GameLevel * GameConfig.LevelRate;

        for (int i = 0; i < clearNum; i++)
        {
            int index = UnityEngine.Random.Range(0, itemDatas.Count);
            itemDatas[index].ClearTextData();
            itemDatas[index].mapData.currentValue = itemDatas[index].mapData.baseValue;
            itemDatas[index].mapData.baseValue = 0;
            itemDatas[index].gameObject.AddComponent<Button>();
            itemDatas[index].gameObject.AddComponent<ItemButtonClick>().itemData = itemDatas[index];
            itemDatas[index].itemBgImg.color = GameConfig.PlayUseColor;
            itemDatas[index].SetInitColor(GameConfig.PlayUseColor);
            PlayerUseItemDatas.Add(itemDatas[index]);
            itemDatas.RemoveAt(index);
        }

        for (int i = 0; i < itemDatas.Count; i++)
        {
            itemDatas[i].transform.localRotation = Quaternion.Euler(0, 90, 0);
            itemDatas[i].gameObject.SetActive(true);
            itemDatas[i].transform.DORotate(Vector3.zero, 0.5f).SetDelay(0.2f);
        }

        for (int i = 0; i < PlayerUseItemDatas.Count; i++)
        {
            PlayerUseItemDatas[i].transform.localRotation = Quaternion.Euler(0, 90, 0);
            PlayerUseItemDatas[i].gameObject.SetActive(true);
            PlayerUseItemDatas[i].transform.DORotate(Vector3.zero, 0.5f).SetDelay(0.2f);
        }

        SelectPanelCanvasGroup.alpha = 0;
        SelectPanel.gameObject.SetActive(true);
        SelectPanelCanvasGroup.DOFade(1, 0.5f).SetDelay(0.4f).OnComplete(() =>
        {
            CannotPlayMask.SetActive(false);
        });
    }

    public void DestroyUI()
    {
        foreach (var child in GameInitManager.Instance.Map)
        {
            child.currentValue = 0;
            child.baseValue = 0;
            child.initCouldSelectNums = null;
        }

        if (itemDatas != null)
        {
            for (int i = 0; i < itemDatas.Count; i++)
            {
                Destroy(itemDatas[i].gameObject);
            }
        }

        if (PlayerUseItemDatas != null)
        {
            for (int i = 0; i < PlayerUseItemDatas.Count; i++)
            {
                Destroy(PlayerUseItemDatas[i].gameObject);
            }
        }
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    public void SelectItem(ItemData itemData)
    {
        if (currentItemData == itemData)
        {
            currentItemData.ResetBgColor();
            currentItemData = null;
            return;
        }

        if (currentItemData != null)
        {
            currentItemData.ResetBgColor();
        }

        currentItemData = itemData;

        itemData.itemBgImg.color = GameConfig.SelectColor;
    }

    public void SetItemDataValue(int value)
    {
        if (currentItemData == null)
        {
            return;
        }

        if (value == 0)
        {
            currentItemData.ClearTextData();
            currentItemData.mapData.baseValue = 0;
        }
        else
        {
            currentItemData.SetValueTextData(value);
            currentItemData.mapData.baseValue = value;
            CheckGameComplete();
        }
    }

    private void CheckGameComplete()
    {
        bool isComplete = true;

        foreach (var child in PlayerUseItemDatas)
        {
            if (child.mapData.baseValue != child.mapData.currentValue)
            {
                isComplete = false;
            }
        }

        if (isComplete)
        {
            foreach (var child in itemDatas)
            {
                child.itemBgImg.color = Color.blue;
            }

            foreach (var child in PlayerUseItemDatas)
            {
                child.itemBgImg.color = Color.blue;
            }

            CannotPlayMask.SetActive(true);
        }
    }

    public void UpdateSelectItem()
    {
        updateSelectItemDict.Clear();
    }
}
