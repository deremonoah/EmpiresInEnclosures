using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    //this handles the display text and Icon of the buttons for them to change
    [Header("Unit Button variables")]
    [SerializeField] List<GameObject> UnitButtons;//this is for enabling and disabling the correct amount of buttons
    public List<TextMeshProUGUI> unitButtonCosts;
    public List<Image> unitButtonIcons;
    

    [Header("item Button variables")]
    [SerializeField] List<GameObject> itemButtons;
    public List<TextMeshProUGUI> itemButtonUses;
    public List<Image> itemButtonIcons;
    
    private PlacingController pc;

    public static ButtonManager instance;

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Debug.LogError("we got 2 Unit Managers in the scene");
            Destroy(this);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
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
        
        var units = EquipManagerPlayer.instance.getPlayerUnits();
        for (int lcv = 0; lcv < units.Count; lcv++)//so do we limit the player to 3 units? for now maybe, choose which to replace? or we need more buttons
        {

            
            UnitButtons[lcv].SetActive(true);
            unitButtonIcons[lcv].sprite = units[lcv].getIcon();
            if(units[lcv] is UnitReward)
            {
               UnitReward un = (UnitReward)units[lcv];
               unitButtonCosts[lcv].text = "" + un.getCost();
            }
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

    public void UpdateItemList()
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
