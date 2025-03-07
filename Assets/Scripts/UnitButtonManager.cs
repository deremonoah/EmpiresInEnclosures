using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonManager : MonoBehaviour
{
    //this handles the display text and Icon of the buttons for them to change
    private UnitManager um;
    public List<Text> buttonCosts;
    public List<Image> buttonIcons;

    void Start()
    {
        um=FindObjectOfType<UnitManager>();
        var units = um.PlayerUnitPrefabs;
        for(int lcv=0;lcv<units.Count;lcv++)
        {
            UnitStats us = units[lcv].GetComponent<UnitStats>();
            if(us!=null)
            {
                buttonCosts[lcv].text = ""+us.getCost();
                buttonIcons[lcv].sprite = us.getIcon();
            }
            else { Debug.Log("missing units for buttons at "+lcv); }
        }
    }
}
