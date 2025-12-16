using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonManager : MonoBehaviour
{
    //this handles the display text and Icon of the buttons for them to change
    private UnitManager um;
    [SerializeField] List<GameObject> Buttons;//this is for enabling and disabling the correct amount of buttons
    public List<Text> buttonCosts;
    public List<Image> buttonIcons;

    void Start()
    {
        um=FindObjectOfType<UnitManager>();
        DisplayUnits();
    }

    private void DisplayUnits()
    {
        foreach(GameObject but in Buttons)//so it doesn't have unused ones there
        {
            but.SetActive(false);
        }
        
        var units = um.PlayerUnitPrefabs;
        for (int lcv = 0; lcv < units.Count; lcv++)//so do we limit the player to 3 units? for now maybe, choose which to replace? or we need more buttons
        {
            UnitStats us = units[lcv].GetComponent<UnitStats>();
            if (us != null)
            {
                Buttons[lcv].SetActive(true);
                buttonCosts[lcv].text = "" + us.getCost();
                buttonIcons[lcv].sprite = us.getIcon();
            }
            else { Debug.Log("missing units for buttons at " + lcv); }
        }
    }

    public void UnitListChanged()//this is so when the unit manager changes its list it will be able to tell it to display correctly
    {
        DisplayUnits();
    }
}
