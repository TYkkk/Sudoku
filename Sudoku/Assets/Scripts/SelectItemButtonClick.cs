using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectItemButtonClick : MonoBehaviour, IPointerClickHandler
{
    public int Value;

    public Text text;

    private void Awake()
    {
        text.text = Value.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SetItemDataValue(Value);
    }
}
