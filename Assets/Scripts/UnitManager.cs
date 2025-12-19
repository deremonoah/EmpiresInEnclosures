using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class handles the spawning the player can do with buttons
//and will handle the enemy spawns
public class UnitManager : MonoBehaviour
{
    [Header("player stats")]
    public Transform PlayerBasePos;
    public List<GameObject> PlayerUnitPrefabs;
    private List<GameObject> PlayerUpgradeHistory;
    [SerializeField] private float playerPP,playerStartingPP, playerMaxPP, PPRegenTimer,PPRegenTimerMax;
    public Text playerPPText, playerPPMaxText;

    [Header("Enmey stuff")]
    public List<GameObject> enemyPrefabs;
    public Transform EnemyBasePos;
    [SerializeField] private int enmPP;
    [SerializeField] int enmStartingPP;
    [SerializeField] private int enmMaxPP;
    [SerializeField] private float enmPPRegenTimer;
    [SerializeField] private float enmPPRegenTimerMax;
    public Text enmPPText, enmPPMaxText;

    [Header("Factions Refrence")]
    [SerializeField] Faction playerFaction;
    [SerializeField] Faction EnemyFaction;//this needs to get updated by the map

    [Header("Spawn Varience")]
    public Vector2 xRange;
    public Vector2 yRange;

    private void Start()
    {
        updatePPText();
        playerPPMaxText.text = "" + playerMaxPP;
        //setting enemy pp for testing but maybe keep as public info
        enmPPMaxText.text = "" + enmMaxPP;//might want a set max in future for gaining max pp
        PlayerUpgradeHistory = new List<GameObject>();
        foreach(GameObject go in PlayerUnitPrefabs)
        { PlayerUpgradeHistory.Add(go); }
    }

    private void OnEnable()
    {
        FlowManager.instance.BattleStart+=StartOfBattleSetPP;
    }

    private void StartOfBattleSetPP()
    {
        playerPP = playerStartingPP;
        enmPP = enmStartingPP;
    }

    private void updatePPText()
    {
        playerPPText.text = "" + playerPP;
        enmPPText.text = "" + enmPP;
    }

    private void Update()
    {
        if(FlowManager.instance.curState==gameState.inBattle)//maybe make coroutine? and a set pp to starting pp for both bases
        {
            PPRegenTimer -= Time.deltaTime;
            enmPPRegenTimer -= Time.deltaTime;

            if (PPRegenTimer <= 0)
            {
                //set up this way if you get pp over max with the penguin pandemic ability it can go over. And with the death of stuff that will have to change
                if (playerPP + 1 <= playerMaxPP) { playerPP++; }
                PPRegenTimer = PPRegenTimerMax;
            }
            updatePPText();

            //regen for enemies
            if (enmPPRegenTimer <= 0)
            {
                //set up this way if you get pp over max with the penguin pandemic ability it can go over. And with the death of stuff that will have to change
                if (enmPP + 1 <= enmMaxPP) { enmPP++; }
                enmPPRegenTimer = enmPPRegenTimerMax;
            }
        }
    }

    public void spawnPlayerUnit(int lcv)
    {
        int unitCost = PlayerUnitPrefabs[lcv].GetComponent<UnitStats>().getCost();
        if (playerPP >= unitCost)
        {
            playerPP -= unitCost;
            playerPPText.text = "" + playerPP;
            //instantiate prefab at spawnPos.pos

            var unit = Instantiate(PlayerUnitPrefabs[lcv], RandomizeSpawn(PlayerBasePos.position), PlayerBasePos.rotation);
            PlayerUnitPrefabs[lcv].GetComponent<UnitStats>().getCost();
            unit.GetComponent<UnitAI>().SetMoveTarget(EnemyBasePos.position);
            unit.GetComponent<UnitAI>().setUnitState(UnitState.move);
            unit.gameObject.layer = 7;
            //truned off friendly fire
        }
        
    }
    public Vector3 RandomizeSpawn(Vector3 aroundHere)
    {
        float randx = Random.Range(xRange.x, xRange.y);
        float randy = Random.Range(yRange.x, yRange.y);

        Vector3 randSpawn=new Vector3(aroundHere.x+randx, aroundHere.y+randy ,aroundHere.z);
        return randSpawn;
    }

    public void PlayerGotNewUnit(GameObject newU)
    {
        PlayerUnitPrefabs.Add(newU);//question is if the count is above 5 then we need to replace 1, how we do that? another panel?
        PlayerUpgradeHistory.Add(newU);
        FindObjectOfType<UnitButtonManager>().UnitListChanged();
    }

    public void PlayerReplaceOldUnit(GameObject newU, int replaced)
    {
        PlayerUnitPrefabs[replaced] = newU;
        PlayerUpgradeHistory.Add(newU);
        FindObjectOfType<UnitButtonManager>().UnitListChanged();//I am thinking we can see the old units so, pop up versions over the buttons hover shows X
    }

    public List<GameObject> GetUpgradeHistory()
    {
        return PlayerUpgradeHistory;
    }

    public List<GameObject> GetCurrentUnits()
    {
        return PlayerUnitPrefabs;
    }

    public Transform GetmoveTarget(int layer)
    {
        if (layer == 6)
        {   return PlayerBasePos; }//I realize if I want players to use the commanding of unit I prob shouldn't auto command them, so they have to do it to learn
        return EnemyBasePos;
    }

    public void PlayerGetsPower(float p,bool canGoAbove)
    {
        if(canGoAbove)
        {
            playerPP += p;
        }else 
        {
            if(playerPP+p<=playerMaxPP)
            {
                playerPP += p;
            }//else they just don't get it because it would go over max or is max and this won't overwrite the penguin pandemic ability
        }
    }

    public void EnemyGetsPower(float p, bool canGoAbove)
    {
         if (canGoAbove)
        {
            enmPP += (int)p;
        }
        else
        {
            if (enmPP + p <= enmMaxPP)
            {
                enmPP += (int)p;
            }//else they just don't get it because it would go over max or is max and this won't overwrite the penguin pandemic ability
        }
    }

    //code for enemy ai summoner
    #region Enemy ai calls
    public void spawnEnemyUnit(int lcv)
    {
        //instantiate prefab at spawnPos.pos
        // pay for unit
        int cost = enemyPrefabs[lcv].GetComponent<UnitStats>().getCost();
        if (enmPP - cost >= 0)
        {
            //pay for it
            enmPP -= cost;
            //below is summoning part

            Vector3 posToSpawn = RandomizeSpawn(EnemyBasePos.position);

            var unit = Instantiate(enemyPrefabs[lcv], posToSpawn, EnemyBasePos.rotation);
            unit.GetComponent<UnitAI>().SetMoveTarget(PlayerBasePos.position);
            unit.GetComponent<UnitAI>().setUnitState(UnitState.move);
            //setting to enemy unit layer so they don't kill each other
            unit.gameObject.layer = 6;
        }
    }

    public int GetEnmPPAmount()
    {
        return enmPP;
    }

    public int GetEnmUnitCost(int slot)
    {
        return enemyPrefabs[slot].GetComponent<UnitStats>().getCost();
    }
    #endregion

    #region Faction Functions
    public Faction GetPlayerFaction()
    { return playerFaction; }

    public Faction GetEnemyFaction()
    { return EnemyFaction; }

    public void SetEnemyFaction(Faction fac)//called by the map node load type dealy
    { EnemyFaction = fac; }
    #endregion
}
