using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    //this handles the display text and Icon of the buttons for them to change
    [Header("Unit Button variables")]
    [SerializeField] List<GameObject> UnitButtons;//this is for enabling and disabling the correct amount of buttons
    public List<Text> unitButtonCosts;
    public List<Image> unitButtonIcons;

    [Header("item Button variables")]
    [SerializeField] List<GameObject> itemButtons;
    public List<Text> itemButtonUses;
    public List<Image> itemButtonIcons;
    private PlacingController pc;

    public static ButtonManager instance;

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Debug.LogError("we got 2 Unit Managers in the scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        pc = PlacingController.instance;
        DisplayUnits();
        DisplayItems();
    }

    private void DisplayUnits()
    {
        foreach(GameObject but in UnitButtons)//so it doesn't have unused ones there
        {
            but.SetActive(false);
        }
        
        var units = UnitManager.instance.PlayerUnitPrefabs;
        for (int lcv = 0; lcv < units.Count; lcv++)//so do we limit the player to 3 units? for now maybe, choose which to replace? or we need more buttons
        {
            UnitStats us = units[lcv].GetComponent<UnitStats>();
            if (us != null)
            {
                UnitButtons[lcv].SetActive(true);
                unitButtonCosts[lcv].text = "" + us.getCost();
                unitButtonIcons[lcv].sprite = us.getIcon();
            }
            else { Debug.Log("missing units for buttons at " + lcv); }
        }
    }

    private void DisplayItems()
    {
        foreach (GameObject but in itemButtons)//so it doesn't have unused ones there
        {
            but.SetActive(false);
        }

        int itemCount = pc.GetItemCount();
        for (int lcv = 0; lcv < itemCount; lcv++)//so do we limit the player to 3 units? for now maybe, choose which to replace? or we need more buttons
        {
            itemButtons[lcv].SetActive(true);
            itemButtonUses[lcv].text = "" + pc.GetItemsCurrentUses(lcv);
            itemButtonIcons[lcv].sprite = pc.GetItemsIcon(lcv);
        }
    }

    public void UnitListChanged()//this is so when the unit manager changes its list it will be able to tell it to display correctly
    {
        DisplayUnits();
    }

    public void ItemListChanged()
    {
        DisplayItems();
    }

    public void UpdateItemUses()
    {
        int itemCount = pc.GetItemCount();
        for (int lcv = 0; lcv < itemCount; lcv++)//so do we limit the player to 3 units? for now maybe, choose which to replace? or we need more buttons
        {
            itemButtonUses[lcv].text = "" + pc.GetItemsCurrentUses(lcv);
        }
    }
}
