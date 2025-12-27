using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootPanel : MonoBehaviour
{
    private Animator animLoot;
    [Header("loaded from resource folder")]
    [SerializeField] private List<FactionLoot> FactionLootLists;
    [Header("panel refrences")]
    [SerializeField] private GameObject ReplaceButtons;
    [SerializeField] List<Image> lootImages;
    private UnitManager um;
    private bool areReplacingUnit;
    [SerializeField]private List<GameObject> PickOptions=new List<GameObject>();
    private int playerPickedThis;

    private void OnEnable()
    {
        //sub to flow manager openLoot +=OpenLootPan;
        if(FlowManager.instance ==null)
        { Debug.Log("instance null"); }
        FlowManager.instance.lootPanelSendOpen += OpenLootPan;
        animLoot = GetComponent<Animator>();
        FactionLootLists =new List<FactionLoot>(Resources.LoadAll<FactionLoot>("FactionLootObjs"));// getting FactionLoot as a list refrence, loading multiple times is less effecient
        um = FindObjectOfType<UnitManager>();
    }

    //probably should still have disable? but the plan is to leave everything loaded

    private void OpenLootPan()
    {
        animLoot.SetBool("Open", true);

        GeneratePicks(um.GetPlayerFaction(),um.GetEnemyFactions());
    }

    public void CloseLootPan()//on select button
    {
        animLoot.SetBool("Open", false);
    }


    public bool IsPanOpen()
    {
        return animLoot.GetBool("Open");
    }
    //how to do section again? or should these be animLootPanel & the functions for selecting

    //selected unit gets added
    //generate picks & load them into buttons (image, info' prob from scriptable obj)

    private void GeneratePicks(Faction player, List<Faction> enemy)//needs to take in data or grab it from somewhere which 2 groups are fighting, I am thinking an Enum on their bases
    {
        //look in the loot list for the list with the right 2 factions
        List<GameObject> lootList=new List<GameObject>();
        //find matching loot table
        Debug.Log("EnemyFactions "+enemy.Count);
        for(int fcv=0;fcv<enemy.Count;fcv++)
        {
            for (int lcv = 0; lcv < FactionLootLists.Count; lcv++)
            {
                if (FactionLootLists[lcv].DoesMatchFactions(player, enemy[fcv]))
                {
                    lootList.AddRange(FactionLootLists[lcv].GetLootOptions());
                }
            }
        }
        Debug.Log("loot list count " + lootList.Count);
        if (lootList==null)
        {
            Debug.LogError("loot is Null");
        }

        //make lootLists into one long list

        //now we have the list
        //fill the 3 buttons with 3 options, but can't repeat things the player already has (maybe want or had incase of getting rid of it)
        //unit manager needs a list of unit History, which would get compared against
        List<GameObject> prevUps =um.GetUpgradeHistory();

        //could remove dups from the loot list first? then pull random number
        for(int lcv=0;lcv<prevUps.Count;lcv++)
        {
            for(int lcv2=0;lcv2< lootList.Count;lcv2++)
            {
                if(lootList[lcv2]==prevUps[lcv])
                {
                    lootList.RemoveAt(lcv2);
                    lcv2=0;
                    //do we put a break here? I think it should work
                }
            }
        }
        //if this works we get 3 randoms, and fill the images,
        PickOptions.Clear();

        for (int lcv = 0; lcv < 3; lcv++)
        {
            int rand = Random.Range(0, lootList.Count);
            PickOptions.Add(lootList[rand]);
            //set image
            lootImages[lcv].sprite = lootList[rand].GetComponent<UnitStats>().getIcon();//set icon sof specific one
            lootList.RemoveAt(rand);//remove it so no repeats
        }
        
        //I do need a refrence to the images to set them, realizing they need to grab the stat refrence
    }

    public void PickButton(int num)
    {
        if(um.GetCurrentUnits().Count+1>5 && !areReplacingUnit)//in future 5 should be a variable, so it can change
        {
            areReplacingUnit = true;
            playerPickedThis = num;
            ReplaceButtons.SetActive(true);//currently the replace buttons will only come up if we have 5 or more, doesnt need to enable each button
        }
        else//had replaceThis function in here as a extra if trying to be too clever
        {
            //not replacing and under unit button count
            um.PlayerGotNewUnit(PickOptions[num]);
        }
        if (!areReplacingUnit)
        { CloseLootPan(); }
    }

    public void ReplaceThis(int num)
    {
        um.PlayerReplaceOldUnit(PickOptions[playerPickedThis], num);
        CloseLootPan();
        areReplacingUnit = false;
        ReplaceButtons.SetActive(false);
    }
}
public enum Faction { Penguins, Giraffes, PolarBears, Beavers, Monkeys}