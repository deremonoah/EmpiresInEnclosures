using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootPanel : MonoBehaviour
{
    private Animator animLoot;
    [Header("loaded from resource folder")]
    [SerializeField] public List<FactionLoot> FactionLootLists;
    [Header("panel refrences")]
    [SerializeField] private GameObject ReplaceButtons;
    [SerializeField] List<Image> lootImages;
    private bool areReplacingUnit;
    [SerializeField]private List<Reward> PickOptions=new List<Reward>();
    private int playerPickedThis;

    public static LootPanel instance;
    private MapPanel mp;

    private void Awake()
    {
        if(instance!=null && instance !=this)
        {
            Debug.LogError("2 loot panels what you got those for?");
        }
        else 
        { 
            instance = this;
            //DontDestroyOnLoad(this.gameObject); yellow error cause its canvas element
        }
    }

    private void OnEnable()
    {
        //sub to flow manager openLoot +=OpenLootPan;
        if(FlowManager.instance == null)
        { Debug.Log("instance null"); }
        FlowManager.instance.lootPanelSendOpen += OpenLootPan;
        animLoot = GetComponent<Animator>();
        FactionLootLists =new List<FactionLoot>(Resources.LoadAll<FactionLoot>("FactionLootSets"));// getting FactionLoot as a list refrence, loading multiple times is less effecient
        mp = MapPanel.instance;
    }

    //probably should still have disable? but the plan is to leave everything loaded

    private void OpenLootPan()
    {
        animLoot.SetBool("Open", true);
        Debug.Log("panel set to open");
        GeneratePicks();
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

    private void GeneratePicks()//needs to take in data or grab it from somewhere which 2 groups are fighting, I am thinking an Enum on their bases
    {
        List<Reward> lootList = mp.getCurrentNode().GenerateRewardOptions();//this gets the factions or encounter rewards & no repeats
        Debug.Log("got in generatePicks");
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
