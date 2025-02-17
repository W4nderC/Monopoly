using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviour : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ToggleUI()
    {
        if(GameManager.Instance.GetLocalPlayerType() == GameManager.Instance.GetCurrentPlayablePlayerType()) {
            Show();
        }
        else if 
        (!GameManager.Instance.CheckGameState(GameManager.GameState.RollDice)
        ||GameManager.Instance.GetLocalPlayerType() != GameManager.Instance.GetCurrentPlayablePlayerType())
        {
            Hide();
        }
    }

}
