using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanel : MonoBehaviour
{
    //rpob make a singleton

    private FlowManager fm;
    private Animator anim;
    private EquipManagerPlayer eqm;

    [Header("Text elements")]
    [SerializeField] Text nameBox;
    [SerializeField] Text description;
    [SerializeField] Text attackBox;
    [SerializeField] Text hpBox;
    [SerializeField] Text SpeedBox;
    [SerializeField] Text costBox;

    [Header("Images & bars")]
    [SerializeField] Image portrait;
    [SerializeField] Image HPBar;
    [SerializeField] Image AttackBar;
    [SerializeField] Image nSpeedBar;
    [SerializeField] Image wSpeedBar;
    [SerializeField] Image mSpeedBar;

    [Header("Button to set")]
    [SerializeField] GameObject selectButton;
    private int rewardToSelect;

    //max numbers for stats
    private float MaxHP=30;
    private float MaxAtk = 15;
    private float MaxSpd = 5;

    private void Start()
    {
        fm = FlowManager.instance;
        anim = GetComponent<Animator>();
        eqm = EquipManagerPlayer.instance;
    }

    public void lookAtLoot(int index)
    {
        if(fm.curState!=gameState.inBattle)
        {
            OpenDescPanel();//only need open with current set up, as it covers looking at the other options
            selectButton.SetActive(true);
            rewardToSelect = index;
            DisplayData(LootPanel.instance.PeakAtReward(index));
        }
    }

    public void lookAtMyUnits(int index)
    {
        if (fm.curState != gameState.inBattle)
        {
            //wait if we already have it open, maybe we just change the data, with an animation? rather than it just being different all of a suddon
            //fill data with object, from equiped manager
            //still need to change over to holding onto the reward refrence, or using it to set the images once equiped it can be dropped, but handles same data type as item equiped
            selectButton.SetActive(false);
            if (anim.GetBool("Open"))//if panel is already open
            {
                ShiftDataPanel();
                //load new data
            }
            else
            {
                OpenDescPanel();
                DisplayData(eqm.getUnitAtIndex(index));
            }
        }
    }

    public void lookAtMyItems(int slot)
    {
        if (fm.curState != gameState.inBattle)
        {
            //wait if we already have it open, maybe we just change the data, with an animation? rather than it just being different all of a suddon
            //fill data with object, from equiped manager
            //still need to change over to holding onto the reward refrence, or using it to set the images once equiped it can be dropped, but handles same data type as item equiped
            selectButton.SetActive(false);
            if (anim.GetBool("Open"))//if panel is already open
            {
                ShiftDataPanel();
                //load new data in slot
            }
            else
            {
                OpenDescPanel();
                //fill data with object, from lootPanel
                //load the select button for the right data
            }
        }
    }

    private void DisplayData(Reward re)
    {
        //shift over to holding onto rewards for equiped so
        //we remove image from unit stat & just have it on reward

        nameBox.text = re.getName();
        portrait.sprite = re.getIcon();
        description.text = re.getDescription();

        if(re is UnitReward)
        {
            DisplayUnitData((UnitReward)re);
        }
        else if(re is ItemReward)
        {
            DisplayItemData((ItemReward)re);
        }
    }

    private void DisplayUnitData(UnitReward re)
    {
        UnitStats stats = re.getStats();
        //max of stats for now to dived them by HP 30, atk 15?, speed 5?
        costBox.text = "Cost: " + stats.getCost();

        HPBar.fillAmount = stats.getMaxHp() / MaxHP;
        AttackBar.fillAmount = stats.getAttack() / MaxAtk;

        nSpeedBar.fillAmount = stats.getMoveSpeed(Terrain.normal)/ MaxSpd;
        wSpeedBar.fillAmount = stats.getMoveSpeed(Terrain.water)/ MaxSpd;
        mSpeedBar.fillAmount = stats.getMoveSpeed(Terrain.mountain)/ MaxSpd;

        SortSpeeds(stats);

        //hpBox.text = "HP:"+stats.getMaxHp();
        //Im going to leave the actual numbers out of it & see
    }

    private void SortSpeeds(UnitStats stats)//orders the bars so the biggest is in the back
    {
        float n = stats.getMoveSpeed(Terrain.normal) / MaxSpd;//will this include buffs for both kinds of units?
        float w = stats.getMoveSpeed(Terrain.water) / MaxSpd;
        float m = stats.getMoveSpeed(Terrain.mountain) / MaxSpd;

        float[] speeds = new float[] { n, w, m };

        for (int lcv = 0; lcv < speeds.Length - 1; lcv++)
        {
            if(speeds[lcv]<speeds[lcv+1])
            {
                //swap them, moving the largest further along
                float s = speeds[lcv];
                speeds[lcv] = speeds[lcv + 1];
                speeds[lcv + 1] = s;
            }
        }

        //sorted largest to smallest
        //match them to the right stat & move that one acordingly
        
        Image[] bars = new Image[] { nSpeedBar, wSpeedBar, mSpeedBar };

        for(int lcv=0;lcv<speeds.Length;lcv++)
        {
            for (int bcv = 0; bcv < bars.Length; bcv++)
            {
                if(bars[bcv].fillAmount==speeds[lcv])
                {
                    bars[bcv].gameObject.GetComponent<RectTransform>().SetAsLastSibling();
                }
            }
        }
        //this ends with the smallest number in the front as it is the last in the list

    }

    private void DisplayItemData(ItemReward re)
    {
        costBox.text = "Item uses: ";
    }

    private void OpenDescPanel()//private as its called by other methods
    {
        anim.SetFloat("Open", 1f);
    }

    public void CloseDescriptionPanel()
    {
        anim.SetFloat("Open", 0f);
    }

    private void ShiftDataPanel()
    {
        anim.SetFloat("Open", 2f);
    }

    public void DynamicSelect()
    {
        LootPanel.instance.PickButton(rewardToSelect);
    }
}
