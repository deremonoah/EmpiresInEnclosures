using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAI : MonoBehaviour
{
    public List<Transform> commandPos;
    private UnitManager um;
    private UltimateManager ulti;
    public float SustainedSpawnTimerMax;
    [SerializeField] BaseHP ourBase;
    [Header("Invader Information")]
    [SerializeField] float FieldOfView;
    [SerializeField] List<UnitType> invaderComp;
    [SerializeField] List<Vector2> invaderLastKnownLocation;
    [Header("Tower")]
    [SerializeField] Transform towerTam;
    [SerializeField] float SearchAreaAroundBase;
    private Vector2 GoThere;//where ai sends units
    Coroutine unitSpawner;
    Coroutine ulitmateChecker;

    private void OnEnable()
    {
        if(FlowManager.instance!=null)
        {
            FlowManager.instance.BattleStart += beginBattle;
            FlowManager.instance.BattleEnd += BattleEnded;
        }
        else { Debug.Log("in enemy base AI null flow manager"); }
    }

    private void OnDisable()
    {
        if (FlowManager.instance != null)
        {
            FlowManager.instance.BattleStart -= beginBattle;
            FlowManager.instance.BattleEnd -= BattleEnded;
        }
    }

    void Start()
    {
        um = UnitManager.instance;//we use it so much across class best to store it as a field
        ulti = FindObjectOfType<UltimateManager>();
        //in future we will add the based on what scene or some factor it might pick a more strategic enemy
    }

    private void beginBattle()
    {
        CalculateStrategies();//which starts spawner
        ulitmateChecker = StartCoroutine(UltimateCheckerRoutine());

        //should decide strategy or adapt based off opponent
        //could base it off which faction it is, which I think it can check from um
    }

    private void BattleEnded()
    {
        StopCoroutine(unitSpawner);
        StopCoroutine(ulitmateChecker);
    }

#region OneUnitStrats
    public IEnumerator SpamStrat()//change to have variance not right on que, so it can be set dynamically
    {
        yield return new WaitForSeconds(0.1f);
        //what do I want the ai to do? summon a guy when they have the PP to do so,
        while (ourBase.GetHP() > 0)
        {
            if (um.GetEnmPPAmount() >= um.GetEnmUnitCost(0))//could make int random, to be fair rn this if not matter, they just spam the button
            {
                int rand = Random.Range(0, 10);
                if(rand<4)//40% on every .3 seconds might spawn a guy if they can
                {
                    um.spawnEnemyUnit(0, GoThere);
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator SemiSpamStrat()
    {
        yield return new WaitForSeconds(0.1f);
        int ourUnitsCost = um.GetEnmUnitCost(0);
        int rand = Random.Range(1, 4);//1-3 units
        int targeNumber = ourUnitsCost * rand;
        while (ourBase.GetHP() > 0)
        {
            if (um.GetEnmPPAmount() >= targeNumber)//could make int random, to be fair rn this if not matter, they just spam the button
            {
                spawnNumberOfUnit(0,rand);
                rand = Random.Range(1, 4);//1-3 units wait
            }//they don't have multiple units or anything they do
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void spawnNumberOfUnit(int unit, int count)
    {
        for(int lcv=0;lcv<count;lcv++)
        {
            um.spawnEnemyUnit(unit, GoThere);
        }
    }

    //ball spam makes many dudes, but doesn't command them to base holds them back, then charges
    //changing where GoThere is we could have them stack up units with spam
    //set up defenders at base or tower
    #endregion

#region MulipleUnitSrats

    private IEnumerator SameBuildRoutine()//but can also charge
    {
        Debug.Log("enemy pp " + um.GetEnmPPAmount());
        List<int> comp =CalculateBuildStrat(um.GetEnmPPAmount());
        int compCost=calculateBuildTotalCost(comp);

        while (ourBase.GetHP() > 0)
        {
            if (um.GetEnmPPAmount() >= compCost)//could make int random, to be fair rn this if not matter, they just spam the button
            {
                if (AreWeAtTheirDoorstep())
                {
                    cavalaryCharge();
                }
                else
                { SpawnBuild(comp); }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    //use builds to counter enemy
    //set up defenders at their base
    //set up defenders at their tower

#endregion

    private List<int> CalculateBuildStrat(int curPP)//a method to check if you have enough cost for a build
    {
        List<int> comp = new List<int>();
        List<UnitStats> enmStats = new List<UnitStats>();

        //grab all units' costs
        int unitCount = um.enemyPrefabs.Count;
        for (int lcv =0;lcv< unitCount;lcv++)
        {
            enmStats.Add(um.enemyPrefabs[lcv].GetComponent<UnitStats>());//puts cost in each slot equivalant
        }


        //this works if it "divides" evenly. across all the units. and doesn't care about units
        int ppToSpend = curPP;
        Debug.Log("starting PP " + ppToSpend);
        int cycle = 0;
        while (ppToSpend>0 && cycle<5)//go through our enemies 5 times at max, if we have an odd number
        {
            for(int lcv=0;lcv<enmStats.Count;lcv++)
            {
                if(ppToSpend<=0)
                {
                    break;
                }
                ppToSpend -= enmStats[lcv].getCost();
                Debug.Log("pp left to budget " + ppToSpend);
                comp.Add(lcv);
            }
            cycle++; 
        }

        //if it were to care about unit role
        //would it add infantry first?

        //also could put in here if we just want a cavalry rush. check if their units are by the player base

        bool cavalryRush = AreWeAtTheirDoorstep();

        Debug.Log("comp count" + comp.Count);

        return comp;
    }

    void OnDrawGizmosSelected()//rn checks player base
    {
        // Ensure this runs only in the editor
        if (Application.isEditor)
        {
            // Set the color of the gizmo (e.g., red)
            Gizmos.color = Color.green;

            // Draw a wire sphere (which appears as a circle in 2D view)
            // The 'point' is the object's position, and the 'radius' is the overlap radius
            Gizmos.DrawWireSphere(um.PlayerBasePos.position, SearchAreaAroundBase);
        }
    }

    private bool AreWeAtTheirDoorstep()//used to if enm units at player base
    {
        //cast pysics circle to check for player units
        Vector2 playerBase = um.PlayerBasePos.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerBase, 10);//might try different numbers
        //clear invader Lists incase things were deleted and to have no dups, also for checking 2 different places

        Debug.Log("we are in door step method");

        int unitsCount=0;
        foreach (Collider2D col in hits)
        {
            if (col.gameObject.layer == 6)//6 for enemy layer
            {
                var unit = col.gameObject.GetComponent<UnitStats>();
                if (unit != null)//this means it is a player unit
                {
                    unitsCount++;
                }
            }
        }

        if(unitsCount>2)
        {
            Debug.Log("door true");
            return true;
        }
        return false;
    }

    private void cavalaryCharge()
    {
        List<int> comp = new List<int>();
        List<UnitStats> enmStats = new List<UnitStats>();
        int ppToSpend = um.GetEnmPPAmount();

        //grab all units' costs
        int unitCount = um.enemyPrefabs.Count;
        for (int lcv = 0; lcv < unitCount; lcv++)
        {
            enmStats.Add(um.enemyPrefabs[lcv].GetComponent<UnitStats>());//puts cost in each slot equivalant
        }


        //rush send in cavalry, or just look at unit with the most speed, most base speed?
        comp.Clear();//clear as its in this calculate one right now

            int fastestUnitIndex = 0;
            for (int lcv = 0; lcv < enmStats.Count; lcv++)
            {
                float bestEnmSpeed = enmStats[fastestUnitIndex].getMoveSpeed(Terrain.normal);
                float newEnmSpeed = enmStats[lcv].getMoveSpeed(Terrain.normal);

                if (newEnmSpeed > bestEnmSpeed)//if 0 compare 0 will default be biggest cause tied
                {
                    fastestUnitIndex = lcv;
                }
                //cavalry should on average have higher speed. and most optimal to send your fastest if you want to keep the pressure on
            }

            //now we know who the fastest unit is
            //so make build with as many as possible
            while (ppToSpend > 0)
            {
                if (ppToSpend - enmStats[fastestUnitIndex].getCost() >= 0)
                {
                    ppToSpend -= enmStats[fastestUnitIndex].getCost();
                    comp.Add(fastestUnitIndex);
                }
                else//so we can't put anymore in & we prob are at more than 0
                {
                    //check if another will fit in comp
                    for (int lcv = 0; lcv < enmStats.Count; lcv++)
                    {
                        int enmCost = enmStats[lcv].getCost();
                        if (enmCost <= ppToSpend)
                        {
                            ppToSpend -= enmCost;
                            comp.Add(lcv);
                        }
                    }
                    //we will assume for now it can only get 1 more unit or so
                    ppToSpend = 0;//so we don't have an infiite loop
                }
            }
        SpawnBuild(comp);
    }

    private int calculateBuildTotalCost(List<int> comp)
    {
        int tot = 0;

        foreach(int i in comp)//i is their position
        {
            tot += um.GetEnmUnitCost(i);
        }

        return tot;
    }

    private void SpawnBuild(List<int> comp)
    {
        foreach(int i in comp)//i is their position
        {
            um.spawnEnemyUnit(i);
        }
    }

    public IEnumerator UltimateCheckerRoutine()//in future might be more or just different strategy pattern stuff
    {
        while (ourBase.GetHP() > 0)
        {
            //check for invaders
            int isInvaders = InvadersCheck();
            //Debug.Log("Invaders check found " + invaderComp.Count);
            if (isInvaders == 1)
            {
                ulti.popPlayerUlt(false);//enemy uses ult
            }
            else if(isInvaders==2)
            {
                //need to defend towers
            }
            //check if enemy base isn't the target if bully has been dealt with
//high prio    //enemy units (maybe player units too) need to be able to follow a string of points.
               //so they can follow a path, around terrain

            //advance strategy, which should be variable
            //wait until a certain number to make specific build, or spam units if you can afford them, which could be cheapest or favorite

            yield return new WaitForSeconds(0.3f);
        }
    }


    private void CalculateStrategies()
    {
        //if strategies are coroutines,
        //could I have it when it finishes 1 it pulls 1 at random or looks at how many alive players there are?

        //how many units do I have?
        List<UnitStats> myUnitsBlocks = new List<UnitStats>();

        foreach(GameObject unit in UnitManager.instance.enemyPrefabs)
        {
            myUnitsBlocks.Add(unit.GetComponent<UnitStats>());
        }
        if (myUnitsBlocks.Count == 1)
        {
            unitSpawner=StartCoroutine(SpamStrat());
            //in future it will be set list
            //then a coroutine that pulls from the list, and uses it until done
            //or adapts? 
        }
        else//has more than 1 unit
        {
            //make a couple different build options, or it calculated them on the fly
            //spawming a unit or mix of units could work
            //if they are beavers or have builds or items place them and command units there
            unitSpawner = StartCoroutine(SameBuildRoutine());
        }
    }

    private void lookForInvaders(Vector2 pos,float size)//used to check if there are enemies threatening base or a tower
    {
        //cast pysics circle to check for player units
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, size);//might try different numbers
        //clear invader Lists incase things were deleted and to have no dups, also for checking 2 different places
        invaderComp.Clear();
        invaderLastKnownLocation.Clear();

        foreach (Collider2D col in hits)
        {
            if (col.gameObject.layer == 7)//7 for player layer
            {
                var unit=col.gameObject.GetComponent<UnitStats>();
                if(unit!=null)//this means it is a player unit
                {
                    invaderComp.Add(unit.getRole());
                    invaderLastKnownLocation.Add(col.transform.position);
                }
            }
        }
    }

    private int InvadersCheck()//0 no, 1 base, 2 tower, go there
    {
        lookForInvaders(this.transform.position, SearchAreaAroundBase);

        if (invaderComp.Count>0)
        {
            return 1;//might need to know where its being invaded, so return int, 0 no, 1 base, 2 tower
            //also we don't clear the list if we are being attacked!
        }

        if(towerTam!=null)//would be null if its destroyed pretty sure
        {
            lookForInvaders(towerTam.position, SearchAreaAroundBase);
            if (invaderComp.Count > 0) { return 2; }
        }

        if (GoThere != (Vector2)um.GetmoveTarget(6).position)
        {
            lookForInvaders(GoThere, 1f);//checking if specific area invaders are is clear
            if (invaderComp.Count < 1) { GoThere = um.GetmoveTarget(6).position; }
        }

        //currently only checks around itself
        return 0;
    }

    public void TheyHurtMe(GameObject bully)
    {
        GoThere = bully.transform.position;//will be where they send units
    }

    //this is called by map on enable when it instantiates
    public void UpdateBaseHP(BaseHP myHP)
    {
        ourBase = myHP;
    }
}


//https://www.reddit.com/r/gamedev/comments/wzbupc/strategy_game_devs_how_do_you_develop_your_ai_for/
//this post had a response talking about having the ai check sertain peramiters in the level then doing the following acordingly, which makes sense
//ai should ask these questions
//are enemies at my base? trigger box probably
//if they are can I ult? if not can I summon some units?
//otherwise follow my gameplan, which could be vatiable, and can be modified to set units on an optimal route or to seek out the food
//another could be check for lethal, or if enemy is low & doesn't have many units either between bases & sends a fast unit down the lane