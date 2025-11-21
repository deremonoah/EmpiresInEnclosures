using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlowManager : MonoBehaviour
{
    // this manager has a state machine tracking when the player can do what
    // the map can be brought up during the looting phase, 
    public static FlowManager instance;
    string battleLoser;
    public gameState curState;
    public event Action lootPanelSendOpen;
    public event Action MapPanelSendOpen;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        //subscribe to mapSelectionEvent
        //subscribe to end of battle description
        BaseHP[] baseHPs = FindObjectsOfType<BaseHP>();//there is supposed to be a better way to do this,says in another oh his videos-> https://www.youtube.com/watch?v=AGGmnVIhHvc
        foreach (BaseHP bhp in baseHPs)
        {
            bhp.BattleEndedLoserCalls += didPlayerLoseBattle;
        }
        //subscribe to loot selected event
    }

    private void OnDisable()
    {
        //unsubscribe to end of battle description
        BaseHP[] baseHPs = FindObjectsOfType<BaseHP>();//there is supposed to be a better way to do this,says in another oh his videos-> https://www.youtube.com/watch?v=AGGmnVIhHvc
        foreach (BaseHP bhp in baseHPs)
        {
            bhp.BattleEndedLoserCalls -= didPlayerLoseBattle;
        }
    }

    public IEnumerator GameFlowRoutine()
    {
        curState = gameState.movingLocation;
        //start with moving location
        //waiting for player to select a battlefield
        //once picked a battleFieldLoader needs to be subed aswell, so they can prep the arena

        curState = gameState.inBattle;
        while (battleLoser.Length<1)//while string is empty
        {
            yield return null;
        }
        if (battleLoser == "Enemy Base")
        {
            curState = gameState.looting;
            lootPanelSendOpen.Invoke();
            //should be able to see your units, maybe all the time?
            //and able to check map
        }
        else if(battleLoser== "Player Base")
        {
            //skip loot phase
            //move enemies?, or diff punishment, could be just a loss idk
        }
        else
        { Debug.LogWarning("sent from neither base? not player or enemy"); }

        StartCoroutine(GameFlowRoutine());
    }
    //what happens if player loses? is there a hp? do they lose if they lose a single battle, or do the fights get harder?
    //like all enemies get an upgrade, and the boss starts moving to you

    public void didPlayerLoseBattle(string whoLost)
    {
        battleLoser = whoLost;
    }
}
public enum gameState { movingLocation,inBattle,looting}