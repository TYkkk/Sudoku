using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SetGameConfigButtonClick : MonoBehaviour, IPointerClickHandler
{
    public int value;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameConfig.GameLevel = value;
        SceneManager.LoadScene("GameStart");
    }
}
