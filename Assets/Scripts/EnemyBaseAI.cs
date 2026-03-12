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

    private void OnEnable()
    {
        if(FlowManager.instance!=null)
        {
            FlowManager.instance.BattleStart += beginBattle;
        }
        else { Debug.Log("in enemy base AI null flow manager"); }
    }

    void Start()
    {
        um = UnitManager.instance;//we use it so much across class best to store it as a field
        ulti = FindObjectOfType<UltimateManager>();
        //in future we will add the based on what scene or some factor it might pick a more strategic enemy
    }

    private void beginBattle()
    {
        CalculateStrategies();
        StartCoroutine(UltimateCheckerRoutine());

        //should decide strategy or adapt based off opponent
        //could base it off which faction it is, which I think it can check from um
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

    //spawm 1 build
    //use builds to counter enemy
    //set up defenders at their base
    //set up defenders at their tower

#endregion

    private void CalculateBuildStrat(int curPP)//a method to check if you have enough cost for a build
    {
        //make a list of ints based off of a team comp idea
        //maybe takes in total points
        //should return the list for the coroutien to itterate through
        //using wait for seconds of random amounts between units imo. could depend on difficulty
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
            StartCoroutine(SpamStrat());
            //in future it will be set list
            //then a coroutine that pulls from the list, and uses it until done
            //or adapts? 
        }
        else//has more than 1 unit
        {
            //make a couple different build options, or it calculated them on the fly
            //spawming a unit or mix of units could work
            //if they are beavers or have builds or items place them and command units there
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