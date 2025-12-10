using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTrainFeet : MonoBehaviour
{
    //this script is to deal with it looking like units aren't on terrain, so a seperate collider is needed
    UnitAI myAi;
    private void Start()
    {
        myAi = GetComponentInParent<UnitAI>();
    }

    public void TerrainChange(Terrain ter)
    {
        myAi.UpdateSpeed(ter);//this 3 calls is not great but still seperating things
        //should probably just be a unit move that handles move and attack. and ai just handles which it uses
    }
}