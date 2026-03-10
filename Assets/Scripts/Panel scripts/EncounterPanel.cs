using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EncounterPanel : MonoBehaviour
{
    private Animator animEnc;
    [Header("panel refrences")]
    [SerializeField] List<Image> lootImages;
    [SerializeField] private List<Reward> PickOptions = new List<Reward>();
    private int playerPickedThis;
    [Header("Display things")]
    [SerializeField] Image displayImage;//maybe background instead?
    [SerializeField] TextMeshProUGUI displayText;

    private MapPanel mp;

    private void OnEnable()
    {
        //sub to flow manager openLoot +=OpenLootPan;
        if (FlowManager.instance == null)
        { Debug.Log("instance null"); }
        animEnc = GetComponent<Animator>();
        
        mp = MapPanel.instance;
    }

   public void OpenEncounterPan()
    {
        animEnc.SetBool("Open", true);
        Debug.Log("panel set to open");
        DisplayFlavor();
        GeneratePicks();
    }

    public void CloseEncounterPan()//on select button
    {
        animEnc.SetBool("Open", false);
    }

    public bool IsPanOpen()
    {
        Debug.Log(animEnc.GetBool("Open"));
        return animEnc.GetBool("Open");
    }

    private void GeneratePicks()//needs to take in data or grab it from somewhere which 2 groups are fighting, I am thinking an Enum on their bases
    {
        List<Reward> lootList = mp.getCurrentNode().GenerateRewardOptions();//this gets the factions or encounter rewards & no repeats
        Debug.Log("got in generatePicks");
        //if this works we get 3 randoms, and fill the images,
        PickOptions.Clear();

        for (int lcv = 0; lcv < 3; lcv++)//might make picks variable so you can have less maybe more? prob not more than 3
        {
            int rand = Random.Range(0, lootList.Count);
            PickOptions.Add(lootList[rand]);
            //set image
            lootImages[lcv].sprite = lootList[rand].getIcon();//set icon sof specific one
            lootList.RemoveAt(rand);//remove it so no repeats
        }
        
    }

    public void PickButton(int num)
    {
        //check if PickOptions[num] is a unit or not
        PickOptions[num].SelectReward();

        CloseEncounterPan();//but we don't always want to close it, or we have a whole new panel pop up that takes over the screen
    }

    private void DisplayFlavor()
    {
        if(mp.getCurrentNode() is EncounterNode)//we know it is
        {
            EncounterNode en = (EncounterNode)mp.getCurrentNode();
            if(en.getPic()!=null)
            {
                displayImage.sprite = en.getPic();
            }
            displayText.text = en.getText();
        }
    }
}
