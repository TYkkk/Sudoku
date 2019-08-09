using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public static readonly int[] EnableArrayNums = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public static readonly int LevelRate = 15;

    public static readonly Color OddLineColor = new Color(0.98f, 0.94f, 0.94f);

    public static readonly Color EvenLineColor = new Color(0.98f, 0.98f, 0.87f);

    public static readonly Color SelectColor = Color.green;

    public static readonly int CreateLoopMax = 10;

    public static int GameLevel = 1;
}
