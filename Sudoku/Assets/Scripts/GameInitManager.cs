using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitManager
{
    public int InitNum = 9;

    public GameObject OneSdTarget;

    #region 单例
    private static GameInitManager _instance;

    public static GameInitManager Instance {
        get {
            if( _instance == null ) {
                _instance = new GameInitManager( );
            }
            return _instance;
        }
    }

    private GameInitManager() {

    }
    #endregion


}
