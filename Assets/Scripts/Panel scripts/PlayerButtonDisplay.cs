using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonDisplay : MonoBehaviour
{
    //this script is to set the player button's canvas to be a higher sort oder as needed
    //like for comparing stats for units in upgrade panel
    private Canvas playerButtons;

    private void Start()
    {
        playerButtons = GetComponent<Canvas>();
    }

    public void showPlayerButtons()//will be called by lootpanel for now
    {
        playerButtons.sortingOrder = 11;
    }

    public void hidePlayerButtons()
    {
        playerButtons.sortingOrder = 0;
    }
}
