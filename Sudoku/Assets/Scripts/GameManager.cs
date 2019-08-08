using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("StartTime:" + Time.realtimeSinceStartup);
        GameInitManager.Instance.InitMapData();
        GameInitManager.Instance.CreateInitMapData();
        Debug.Log("EndTime:" + Time.realtimeSinceStartup);
    }
}
