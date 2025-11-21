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

    private void closemap()
    {
        animMap.SetBool("OpenMap", false);
    }

    public void pickMapNode()
    {
        //would it also intake a number? probably or the nodes store inffo about who you would fight
    }
}
