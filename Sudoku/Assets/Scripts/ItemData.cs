using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    private int value;
    private Color InitColor;

    public MapData mapData;
    public Text valueText;
    public Image itemBgImg;

    public void SetValueTextData(int value)
    {
        this.value = value;
        valueText.text = this.value.ToString();
    }

    public void ClearTextData()
    {
        value = 0;
        valueText.text = "";
    }

    public void SetInitColor(Color color)
    {
        InitColor = color;
    }

    public void ResetBgColor()
    {
        itemBgImg.color = InitColor;
    }
}
