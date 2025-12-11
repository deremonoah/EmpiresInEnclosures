using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanel : MonoBehaviour
{
    private Animator animMap;
    private void OnEnable()
    {
        //sub to flow manager for forced pop up
        animMap = GetComponent<Animator>();
        FlowManager.instance.MapPanelSendOpen += openMap;
    }

    private void openMap()
    {
        animMap.SetBool("OpenMap", true);
    }

    public void closemap()
    {
        animMap.SetBool("OpenMap", false);
    }

    public bool IsPanOpen()
    {
        return animMap.GetBool("Open");
    }

    public void pickMapNode()
    {
        //would it also intake a number? probably or the nodes store inffo about who you would fight
        //can you get data stored on a button while executing a function from the refrence script on the button
    }
}
