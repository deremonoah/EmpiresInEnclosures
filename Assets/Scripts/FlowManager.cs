using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
    public event Action BattleEnd;

    [Header("Panels")]
    [SerializeField] GameObject winPan;
    [SerializeField] GameObject lossPan;
    private MapPanel mapPan;
    private LootPanel looPan;
    private EncounterPanel encPan;

    private int turnCount;

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            //this becomes expected behavior with reloading or loading other scenes
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        //subscribe to mapSelectionEvent
        //subscribe to end of battle description
        /*BaseHP[] baseHPs = FindObjectsOfType<BaseHP>();//there is supposed to be a better way to do this,says in another oh his videos-> https://www.youtube.com/watch?v=AGGmnVIhHvc
        foreach (BaseHP bhp in baseHPs)
        {
            bhp.BattleEndedLoserCalls += didPlayerLoseBattle;
        }*/
        //should subscribe to events from these?
        mapPan = FindObjectOfType<MapPanel>();
        looPan = FindObjectOfType<LootPanel>();
        encPan = FindObjectOfType<EncounterPanel>();
        StartCoroutine(GameFlowRoutine());
        //subscribe to loot selected event
    }

    private void OnDisable()
    {
        //unsubscribe to end of battle description
        /*BaseHP[] baseHPs = FindObjectsOfType<BaseHP>();//there is supposed to be a better way to do this,says in another oh his videos-> https://www.youtube.com/watch?v=AGGmnVIhHvc
        foreach (BaseHP bhp in baseHPs)
        {
            bhp.BattleEndedLoserCalls -= didPlayerLoseBattle;
        }*/
    }

    public IEnumerator GameFlowRoutine()
    {
        Debug.Log("map panel is null? "+(mapPan == null));
        mapPan.openMap();
        curState = gameState.movingLocation;//player decides what fight to have first

        while (mapPan.isStillDecidingNode())
        {
            yield return null;
        }
        
        if(battleLoser.Length<1)//this is to tell if its a combat encounter or not
        {
            curState = gameState.inBattle;
            BattleStart.Invoke();//this event should tell ai battle has started
        }
        while (battleLoser.Length<1)//while string is empty
        {
            yield return null;
        }
        BattleEnd.Invoke();//battle ended if battle loser ended

        if (battleLoser == "Enemy Base")//this is hit if we have combat or a noncombat encounter
        {
            curState = gameState.looting;
            lootPanelSendOpen.Invoke();
            battleLoser = "";//clear it for next time
            mapPan.PlayerBeatNode();
            Debug.Log("got to where it should open panel");
            //playerWon();
        }
        else if(battleLoser== "Player Base")
        {
            //skip loot phase, could no loot create a negative feedback loop so you don't get stronger and each fight is harder?
            //move enemies?, or diff punishment, could be just a loss idk
            playerLost();// later will change specific panels
        }
        else if(battleLoser=="Encounter")
        {
            mapPan.PlayerBeatNode();
            encPan.OpenEncounterPan();
        }//could have a bossBase for it being a boss, then it counts up to 3 or a set limit to pop up win panel
        else
        { Debug.LogWarning("sent from neither base? not player or enemy"); }
        battleLoser = "";
        
        while(looPan.IsPanOpen())
        {
            yield return null;
        }

        while(encPan.IsPanOpen())
        {
            yield return null;
        }

        //maybe display pre battle info,
        //then would be another waiting for them to hit the fight? is this too many menus?

        //load next battle
        turnCount++;
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

    public void playerWon()//for the playtest setting it up because the way player beat node works
    {
        //for now just pop up panel no upgrades yet
        winPan.SetActive(true);
    }

    public void didPlayerLoseBattle(string whoLost)
    {
        Debug.Log("got in battle loser "+whoLost);
        battleLoser = whoLost;
    }

    public gameState getCurrentState()
    { return curState; }

    public void ReloadGame()
    {
        SceneManager.LoadScene("Game");
        //was considering restart application, but then it would take them to main menu right?
    }
}
public enum gameState { starting,movingLocation,inBattle,looting}