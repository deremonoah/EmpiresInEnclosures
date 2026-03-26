using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonDisplay : MonoBehaviour
{
    //this script is to set the player button's canvas to be a higher sort oder as needed
    //like for comparing stats for units in upgrade panel
    private Canvas playerButtons;
    private PlacingController pc;

    [SerializeField] List<GameObject> UnitMagnifingGlassIcons;
    [SerializeField] List<GameObject> ItemMagnifingGlassIcons;

    private void Start()
    {
        playerButtons = GetComponent<Canvas>();
        pc = PlacingController.instance;
        HideGlasses();
    }

    public void showPlayerButtons()//will be called by lootpanel for now
    {
        playerButtons.sortingOrder = 11;
        DisplayGlasses();
    }

    public void hidePlayerButtons()
    {
        playerButtons.sortingOrder = 0;
        HideGlasses();
    }

    private void DisplayGlasses()
    {
        var units = EquipManagerPlayer.instance.getPlayerUnits();
        int unitc = units.Count;

        for (int lcv = 0; lcv < unitc; lcv++)
        {
            UnitMagnifingGlassIcons[lcv].SetActive(true);
        }

        int itemCount = pc.GetItemCount();
        
        for (int lcv = 0; lcv < itemCount; lcv++)
        {
            ItemMagnifingGlassIcons[lcv].SetActive(true);
        }
    }

    private void HideGlasses()
    {
        foreach (GameObject mg in UnitMagnifingGlassIcons)
        {
            mg.SetActive(false);
        }

        foreach (GameObject mg in ItemMagnifingGlassIcons)
        {
            mg.SetActive(false);
        }
    }
}
