using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnablerManager : MonoBehaviour
{
    //this holds the different maps and simpley enables them
    //in future it might handle randomization either from multiple hand made versions or randomly generated maps

    //do we need a current map one? probably

    [SerializeField] List<Level> mapList;// could have a map holder that holds the faction refrence on it
    private int currentlyOpenMap;

    private void Start()
    {
        //disables all levels at the start
        for (int lcv = 0; lcv < mapList.Count; lcv++)
        {
            mapList[lcv].gameObject.SetActive(false);
        }
    }

    public void loadLevel(Faction facinLoad)
    {
        mapList[currentlyOpenMap].gameObject.SetActive(false);//disables previous map
        currentlyOpenMap = -1;//can't set it to null, but is for check
        for(int lcv=0;lcv<mapList.Count;lcv++)
        {
            if(mapList[lcv].GetFaction()==facinLoad)//if its the map to load, enable it
            {
                mapList[lcv].gameObject.SetActive(true);
                currentlyOpenMap = lcv;
            }
        }
        if (currentlyOpenMap < 0)
        { Debug.LogError("couldn't find matching map currently open map =-1"); }

        //the map on enable enstaniates it
    }

}
