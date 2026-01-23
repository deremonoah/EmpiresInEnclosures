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
    private bool areReplacingUnit;
    [SerializeField]private List<Reward> PickOptions=new List<Reward>();
    private int playerPickedThis;

    private void OnEnable()
    {
        //sub to flow manager openLoot +=OpenLootPan;
        if(FlowManager.instance == null)
        { Debug.Log("instance null"); }
        FlowManager.instance.lootPanelSendOpen += OpenLootPan;
        animLoot = GetComponent<Animator>();
        FactionLootLists =new List<FactionLoot>(Resources.LoadAll<FactionLoot>("FactionLootSets"));// getting FactionLoot as a list refrence, loading multiple times is less effecient
    }

    //probably should still have disable? but the plan is to leave everything loaded

    private void OpenLootPan()
    {
        animLoot.SetBool("Open", true);

        GeneratePicks(UnitManager.instance.GetPlayerFaction(), UnitManager.instance.GetEnemyFactions());
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
        List<Reward> lootList=new List<Reward>();
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
        List<GameObject> prevUps =UnitManager.instance.GetUpgradeHistory();

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
            lootImages[lcv].sprite = lootList[rand].getIcon();//set icon sof specific one
            lootList.RemoveAt(rand);//remove it so no repeats
        }
        
        //I do need a refrence to the images to set them, realizing they need to grab the stat refrence
    }

    public void PickButton(int num)
    {
        //check if PickOptions[num] is a unit or not
        PickOptions[num].SelectReward();

        CloseLootPan();//but we don't always want to close it, or we have a whole new panel pop up that takes over the screen
    }

    //might remove this too
    public void ReplaceThis(int num)
    {
        UnitReward u = (UnitReward)PickOptions[playerPickedThis];
        GameObject unit = u.GetPrefab();
        UnitManager.instance.PlayerReplaceOldUnit(unit, num);
        CloseLootPan();
        areReplacingUnit = false;
        ReplaceButtons.SetActive(false);
    }
}
