using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionPanel : MonoBehaviour
{
    //rpob make a singleton

    private FlowManager fm;
    private Animator anim;

    private void Start()
    {
        fm = FlowManager.instance;
        anim = GetComponent<Animator>();
    }

    public void lookAtLoot(int slot)
    {
        if(fm.curState!=gameState.inBattle)
        {
            OpenDescPanel();//only need open with current set up, as it covers looking at the other options
            //fill data with object, from lootPanel
        }
    }

    public void lookAtMyUnits(int slot)
    {
        if (fm.curState != gameState.inBattle)
        {
            //wait if we already have it open, maybe we just change the data, with an animation? rather than it just being different all of a suddon
            //fill data with object, from equiped manager
            //still need to change over to holding onto the reward refrence, or using it to set the images once equiped it can be dropped, but handles same data type as item equiped
            if (anim.GetBool("Open"))//if panel is already open
            {
                ShiftDataPanel();
                //load new data
            }
            else
            {
                OpenDescPanel();
                //fill data with object, from lootPanel
                //load the select button for the right data
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

        //this will need to be able to tell the difference between rewards
        //then display their info, maybe a sub display item vs display unit sub method
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
}
