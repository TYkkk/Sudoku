using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemButtonClick : MonoBehaviour, IPointerClickHandler
{
    public ItemData itemData;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null)
        {
            return;
        }

        GameManager.Instance.SelectItem(itemData);
    }
}
