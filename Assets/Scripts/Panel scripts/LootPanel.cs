using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPanel : MonoBehaviour
{
    private Animator animLoot;
    private void OnEnable()
    {
        //sub to flow manager openLoot +=OpenLootPan;
        if(FlowManager.instance ==null)
        { Debug.Log("instance null"); }
        FlowManager.instance.lootPanelSendOpen += OpenLootPan;
        animLoot = GetComponent<Animator>();
    }

    //probably should still have disable? but the plan is to leave everything loaded

    private void OpenLootPan()
    {
        animLoot.SetBool("Open", true);
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

    public void PickButton(int num)
    {
        //num is for the pick
        CloseLootPan();
    }

}
