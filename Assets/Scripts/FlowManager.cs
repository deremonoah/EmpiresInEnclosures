using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlowManager : MonoBehaviour
{
    // this manager has a state machine tracking when the player can do what
    // the map can be brought up during the looting phase, 
    public static FlowManager instance;
    public string battleLoser;
    public gameState curState;
    public event Action lootPanelSendOpen;
    public event Action MapPanelSendOpen;
    public event Action BattleStart;

    [Header("Panels")]
    [SerializeField] GameObject winPan;
    [SerializeField] GameObject lossPan;
    private MapPanel mapPan;
    private LootPanel looPan;

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
        //should subscribe to events from these?
        mapPan = FindObjectOfType<MapPanel>();
        looPan = FindObjectOfType<LootPanel>();
        StartCoroutine(GameFlowRoutine());
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
        mapPan.openMap();
        curState = gameState.movingLocation;//need to tell ai that we are maping or it checks

        while (mapPan.IsPanOpen())
        {
            yield return null;
        }
        //map panel handles input of node to load
        //map loader loads map
        curState = gameState.inBattle;
        BattleStart.Invoke();//this event should tell ai battle has started
        while (battleLoser.Length<1)//while string is empty
        {
            yield return null;
        }

        if (battleLoser == "Enemy Base")
        {
            curState = gameState.looting;
            lootPanelSendOpen.Invoke();
            battleLoser = "";//clear it for next time
            mapPan.PlayerBeatNode();
            //playerWon();
        }
        else if(battleLoser== "Player Base")
        {
            //skip loot phase, could no loot create a negative feedback loop so you don't get stronger and each fight is harder?
            //move enemies?, or diff punishment, could be just a loss idk
            playerLost();// later will change specific panels
        }
        else
        { Debug.LogWarning("sent from neither base? not player or enemy"); }

        
        while(looPan.IsPanOpen())
        {
            yield return null;
        }


        //maybe display pre battle info,
        //then would be another waiting for them to hit the fight? is this too many menus?

        //load next battle

        StartCoroutine(GameFlowRoutine());
    }
    //what happens if player loses? is there a hp? do they lose if they lose a single battle, or do the fights get harder?
    //like all enemies get an upgrade, and the boss starts moving to you

    private void playerLost()
    {
        //will become more complex later tracking losses, maybe only losing
        //against a boss counts
        lossPan.SetActive(true);
    }

    private void playerWon()
    {
        //for now just pop up panel no upgrades yet
        winPan.SetActive(true);
    }

    public void didPlayerLoseBattle(string whoLost)
    {
        Debug.Log("got in battle loser");
        battleLoser = whoLost;
    }

    public gameState getCurrentState()
    { return curState; }
}
public enum gameState { starting,movingLocation,inBattle,looting}