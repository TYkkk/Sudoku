using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitManager
{
    public MapData[,] Map;

    public GameObject OneSdTarget;

    private Dictionary<int, MapData[,]> parentMapData;

    private Dictionary<int, List<MapData>> rowMapDatas;

    private Dictionary<int, List<MapData>> columnMapDatas;

    private List<MapData> otherMapList;

    private Dictionary<int, int> dataDict = new Dictionary<int, int>();

    private int loopNum = 0;

    #region 单例
    private static GameInitManager _instance;

    public static GameInitManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameInitManager();
            }
            return _instance;
        }
    }

    private GameInitManager()
    {

    }
    #endregion

    public void InitMapData()
    {
        Map = new MapData[9, 9];
        parentMapData = new Dictionary<int, MapData[,]>();
        rowMapDatas = new Dictionary<int, List<MapData>>();
        columnMapDatas = new Dictionary<int, List<MapData>>();

        int total = 0;

        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                MapData mapData = new MapData
                {
                    baseValue = 0,
                    rowIndex = i,
                    columnIndex = j
                };

                Map[i, j] = mapData;

                int parentID = i / 3 + j / 3 + ((total / 27) * 2);

                if (!parentMapData.ContainsKey(parentID))
                {
                    MapData[,] parentData = new MapData[3, 3];
                    parentMapData.Add(parentID, parentData);
                }

                if (!rowMapDatas.ContainsKey(i))
                {
                    List<MapData> maps = new List<MapData>();
                    rowMapDatas.Add(i, maps);
                }

                if (!columnMapDatas.ContainsKey(j))
                {
                    List<MapData> maps = new List<MapData>();
                    columnMapDatas.Add(j, maps);
                }

                rowMapDatas[i].Add(mapData);
                columnMapDatas[j].Add(mapData);

                parentMapData[parentID][i % 3, j % 3] = Map[i, j];
                Map[i, j].mapDataParent = parentMapData[parentID];
                total++;
            }
        }
    }

    public void CreateInitMapData()
    {
        loopNum = 0;

        ClearDataMapBaseValue();

        CreateFreeMapData();
        otherMapList = new List<MapData>();
        foreach (var child in Map)
        {
            if (child.baseValue == 0)
            {
                otherMapList.Add(child);
            }
        }

        CreateOtherMapData(otherMapList);
    }

    private void CreateFreeMapData()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                CreateFreeOneMapData(Map[i, j]);
            }
        }

        for (int i = 3; i < 6; i++)
        {
            for (int j = 3; j < 6; j++)
            {
                CreateFreeOneMapData(Map[i, j]);
            }
        }

        for (int i = 6; i < 9; i++)
        {
            for (int j = 6; j < 9; j++)
            {
                CreateFreeOneMapData(Map[i, j]);
            }
        }
    }

    public void CreateOtherMapData(List<MapData> mapDataList, int Index = 0)
    {
        loopNum++;

        if (loopNum >= 200)
        {
            GameManager.Instance.DestroyUI();
            GameManager.Instance.CreateMapData();
            return;
        }

        if (Index < mapDataList.Count)
        {
            var enableList = GetEnableNumList(mapDataList[Index]);

            if (mapDataList[Index].initCouldSelectNums == null)
            {
                mapDataList[Index].initCouldSelectNums = enableList;
            }

            if (mapDataList[Index].initCouldSelectNums.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, mapDataList[Index].initCouldSelectNums.Count);
                var useNum = mapDataList[Index].initCouldSelectNums[randomIndex];
                mapDataList[Index].initCouldSelectNums.Remove(useNum);
                mapDataList[Index].baseValue = useNum;
                CreateOtherMapData(otherMapList, ++Index);
            }
            else
            {
                mapDataList[Index].initCouldSelectNums = null;
                mapDataList[Index].baseValue = 0;
                CreateOtherMapData(otherMapList, --Index);
            }
        }
        else
        {
            Debug.Log("Create End");
            DebugMap();
            GameManager.Instance.CreateUI();
        }
    }

    private void CreateFreeOneMapData(MapData child)
    {
        var enableList = GetEnableNumList(child);

        int index = UnityEngine.Random.Range(0, enableList.Count);

        var useNum = enableList[index];

        child.baseValue = useNum;
    }

    private void ClearDataMapBaseValue()
    {
        foreach (var child in Map)
        {
            child.baseValue = 0;
        }
    }

    private void DebugMap()
    {
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            Debug.Log(Map[i, 0].baseValue + "," + Map[i, 1].baseValue + "," + Map[i, 2].baseValue + "," + Map[i, 3].baseValue + "," + Map[i, 4].baseValue + "," + Map[i, 5].baseValue + "," + Map[i, 6].baseValue + "," + Map[i, 7].baseValue + "," + Map[i, 8].baseValue);
        }
    }

    private List<int> GetCouldSelectNum(MapData currentData, IEnumerable<MapData> mapDatas, bool isInit = true)
    {
        List<int> couldSelectNums = new List<int>();

        dataDict.Clear();
        for (int i = 0; i < GameConfig.EnableArrayNums.Length; i++)
        {
            dataDict.Add(GameConfig.EnableArrayNums[i], 0);
        }

        if (!isInit)
        {
            if (dataDict.ContainsKey(currentData.baseValue))
            {
                dataDict[currentData.baseValue]++;
            }
        }

        foreach (var child in mapDatas)
        {
            if (dataDict.ContainsKey(child.baseValue))
            {
                dataDict[child.baseValue]++;
            }
        }

        foreach (var child in dataDict.Keys)
        {
            if (dataDict[child] == 0)
            {
                couldSelectNums.Add(child);
            }
        }

        return couldSelectNums;
    }

    private List<int> GetCouldSelectNum(MapData currentData, MapData[,] mapDatas, bool isInit = true)
    {
        List<int> couldSelectNums = new List<int>();

        dataDict.Clear();
        for (int i = 0; i < GameConfig.EnableArrayNums.Length; i++)
        {
            dataDict.Add(GameConfig.EnableArrayNums[i], 0);
        }

        if (!isInit)
        {
            if (dataDict.ContainsKey(currentData.baseValue))
            {
                dataDict[currentData.baseValue]++;
            }
        }

        foreach (var child in mapDatas)
        {
            if (dataDict.ContainsKey(child.baseValue))
            {
                dataDict[child.baseValue]++;
            }
        }

        foreach (var child in dataDict.Keys)
        {
            if (dataDict[child] == 0)
            {
                couldSelectNums.Add(child);
            }
        }

        return couldSelectNums;
    }

    private List<int> GetIntersect(List<int> A, List<int> B)
    {
        List<int> couldSelectNums = new List<int>();
        dataDict.Clear();

        foreach (var child in A)
        {
            if (!dataDict.ContainsKey(child))
            {
                dataDict.Add(child, 0);
            }
        }

        foreach (var child in B)
        {
            if (dataDict.ContainsKey(child))
            {
                dataDict[child]++;
            }
        }

        foreach (var child in dataDict.Keys)
        {
            if (dataDict[child] > 0)
            {
                couldSelectNums.Add(child);
            }
        }

        return couldSelectNums;
    }

    public List<int> GetEnableNumList(MapData child)
    {
        var rcList = GetIntersect(GetCouldSelectNum(child, rowMapDatas[child.rowIndex]), GetCouldSelectNum(child, columnMapDatas[child.columnIndex]));
        var pList = GetCouldSelectNum(child, child.mapDataParent);

        return GetIntersect(pList, rcList);
    }
}

public class MapData
{
    public int baseValue;
    public int currentValue;
    public int rowIndex;
    public int columnIndex;
    public MapData[,] mapDataParent;
    public List<int> initCouldSelectNums;
}
