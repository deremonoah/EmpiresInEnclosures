using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

//this class handles the spawning the player can do with buttons
//and will handle the enemy spawns
public class UnitManager : MonoBehaviour
{
    [Header("player stats")]
    public Transform PlayerBasePos;
    public List<GameObject> PlayerUnitPrefabs;
    [SerializeField] private float playerPP,playerStartingPP, playerMaxPP, PPRegenTimer,PPRegenTimerMax;
    public TextMeshProUGUI playerPPText, playerPPMaxText;
    [SerializeField] private List<GameObject> spawnedPlayerUnits = new List<GameObject>();
    [SerializeField] private bool playerAutoCharge;
    public event Action DontChargeBase;

    [Header("Enmey stuff")]
    public List<GameObject> enemyPrefabs;
    public Transform EnemyBasePos;
    [SerializeField] private int enmPP;
    [SerializeField] int enmStartingPP;
    [SerializeField] private int enmMaxPP;
    [SerializeField] private float enmPPRegenTimer;
    [SerializeField] private float enmPPRegenTimerMax;
    public Text enmPPText, enmPPMaxText;
    private List<GameObject> spawnedEnemyUnits = new List<GameObject>();

    [Header("Factions Refrence")]
    [SerializeField] Faction playerFaction;
    [SerializeField] List<Faction> EnemyFaction;//this needs to get updated by the map

    [Header("Spawn Varience")]
    public Vector2 xRange;
    public Vector2 yRange;

    public static UnitManager instance;



    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Debug.LogError("we got 2 Unit Managers in the scene");
            Destroy(this);//only call to destroy game object for one script on obj
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject); we reset the game by reloading scene
        }
    }

    private void Start()
    {
        updatePPText();
        playerPPMaxText.text = "" + playerMaxPP;
        //setting enemy pp for testing but maybe keep as public info
        enmPPMaxText.text = "" + enmMaxPP;//might want a set max in future for gaining max pp

        var tempList = EquipManagerPlayer.instance.getPlayerUnits();
        foreach(UnitReward go in tempList)
        {
            PlayerUnitPrefabs.Add(go.GetPrefab());
        }
    }

    private void OnEnable()
    {
        if (FlowManager.instance != null)
        {
            FlowManager.instance.BattleStart += StartOfBattleSetPP;
            FlowManager.instance.lootPanelSendOpen += ClearUnitsAfterFight;
        }
        else {
            StartOfBattleSetPP();
            Debug.Log("flow manager is null"); }
    }

    private void OnDisable()
    {
        if (FlowManager.instance != null)
        {
            FlowManager.instance.BattleStart -= StartOfBattleSetPP;
            FlowManager.instance.lootPanelSendOpen -= ClearUnitsAfterFight;
        }
    }

    private void StartOfBattleSetPP()
    {
        playerPP = playerStartingPP+EquipManagerPlayer.instance.GetPlayerStartingPPBuff();
        enmPP = enmStartingPP;
    }

    private void updatePPText()
    {
        playerPPText.text = "" + playerPP;
        enmPPText.text = "" + enmPP;
    }

    private void Update()
    {
        if(FlowManager.instance!=null)
        {
            if (FlowManager.instance.curState == gameState.inBattle)//maybe make coroutine? and a set pp to starting pp for both bases
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
        else
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
            unit.gameObject.layer = 7;
            PlayerUnitPrefabs[lcv].GetComponent<UnitStats>().getCost();
            if (playerAutoCharge)
            { unit.GetComponent<UnitAI>().SetMoveTarget(EnemyBasePos.position); }//if auto charge is on charge at enemy base
            else { unit.GetComponent<UnitAI>().SetMoveTarget(RandomizeSpawn(gameObject.transform.position)); }//I moved the enmpy manager where the square for teaching is
            //unit.GetComponent<UnitAI>().setUnitState(UnitState.move); will it start by default
            
            spawnedPlayerUnits.Add(unit);
            //truned off friendly fire
        }
        
    }
    public Vector3 RandomizeSpawn(Vector3 aroundHere)
    {
        float randx = UnityEngine.Random.Range(xRange.x, xRange.y);//adding using system adds another random call, so need to specify
        float randy = UnityEngine.Random.Range(yRange.x, yRange.y);

        Vector3 randSpawn=new Vector3(aroundHere.x+randx, aroundHere.y+randy ,aroundHere.z);
        return randSpawn;
    }

    public void PlayerGotNewUnit(GameObject newU)
    {
        PlayerUnitPrefabs.Add(newU);//question is if the count is above 5 then we need to replace 1, how we do that? another panel?
        FindObjectOfType<ButtonManager>().UnitListChanged();
    }

    public void PlayerReplaceOldUnit(GameObject newU, int replaced)
    {
        PlayerUnitPrefabs[replaced] = newU;
        FindObjectOfType<ButtonManager>().UnitListChanged();//I am thinking we can see the old units so, pop up versions over the buttons hover shows X
    }

    public List<GameObject> GetCurrentUnits()
    {
        return PlayerUnitPrefabs;
    }

    public Transform GetmoveTarget(int layer)
    {
        if (layer == 6)//enemy layer, so they get the player base pos
        {   return PlayerBasePos; }
        return EnemyBasePos; //enemy base pos which is where the enemy base is
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

    public void LoadEnemyUnitList(List<GameObject> units)
    {

        enemyPrefabs.Clear();
        for (int lcv=0; lcv < units.Count;lcv++)
        {
            enemyPrefabs.Add(units[lcv]);
        }
    }

    private void ClearUnitsAfterFight()
    {
        foreach(GameObject go in spawnedPlayerUnits)
        {
            Destroy(go);
        }

        foreach(GameObject go in spawnedEnemyUnits)
        {
            Destroy(go);
        }
        spawnedPlayerUnits.Clear();
        spawnedEnemyUnits.Clear();
    }
    
    public void SetAutoCharge(bool toggleValue)
    {
        playerAutoCharge = toggleValue;
        if(playerAutoCharge==false)
        {
            DontChargeBase.Invoke();
        }
    }

    public bool getAutoCharge()
    {
        return playerAutoCharge;
    }
    
    public void AddStructuresToLists(bool isPlayer,List<GameObject> structs)
    {
        if(isPlayer)
        {
            foreach(GameObject go in structs)
            {
                spawnedPlayerUnits.Add(go);
            }
        }
        else
        {
            foreach (GameObject go in structs)
            {
                spawnedEnemyUnits.Add(go);
            }
        }
    }

#region Enemy ai calls
    public void spawnEnemyUnit(int lcv, Vector2 whereToGo)
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
            unit.GetComponent<UnitAI>().SetMoveTarget(whereToGo);
            //unit.GetComponent<UnitAI>().setUnitState(UnitState.move);
            //setting to enemy unit layer so they don't kill each other
            unit.gameObject.layer = 6;
            spawnedEnemyUnits.Add(unit);
        }
    }
//for test buttons for enemy
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
            //unit.GetComponent<UnitAI>().setUnitState(UnitState.move);
            //setting to enemy unit layer so they don't kill each other
            unit.gameObject.layer = 6;
            spawnedEnemyUnits.Add(unit);
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

    //this is called by map on enable when it instantiates
    public void UpdateEnemyBasePos(Transform newBasePos)
    {
        EnemyBasePos = newBasePos;
    }

    public void UpdatePlayerBasePos(Transform here)
    {
        PlayerBasePos = here;
    }
#endregion

    public Transform getEnemyBasePos()
    {
        return EnemyBasePos;
    }

#region Faction Functions
    public Faction GetPlayerFaction()
    { return playerFaction; }

    public List<Faction> GetEnemyFactions()
    { return EnemyFaction; }

    public void SetEnemyFaction(List<Faction> fac)//called by the map node load type dealy
    { EnemyFaction = fac; }
#endregion

    public void removeMeFromList(GameObject unit)
    {
        if(unit.layer==7)
        {
            spawnedPlayerUnits.Remove(unit);
        }
        else if(unit.layer==6)
        {
            spawnedEnemyUnits.Remove(unit);
        }
    }
}
