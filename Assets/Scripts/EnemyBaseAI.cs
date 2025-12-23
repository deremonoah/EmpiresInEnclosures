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
    [SerializeField] List<UnitRole> invaderComp;
    [SerializeField] List<Vector2> invaderLastKnownLocation;
    [Header("Tower")]
    [SerializeField] Transform towerTam;

    private void OnEnable()
    {
        FlowManager.instance.BattleStart += beginBattle;
    }

    void Start()
    {
        um = FindObjectOfType<UnitManager>();
        ulti = FindObjectOfType<UltimateManager>();
        //in future we will add the based on what scene or some factor it might pick a more strategic enemy
    }

    private void beginBattle()
    {
        StartCoroutine(SpamStratRoutine());

        //should decide strategy or adapt based off opponent
        //could base it off which faction it is, which I think it can check from um
    }

    //this is the spam strat
    public IEnumerator GiraffeStrat()
    {
        yield return new WaitForSeconds(0.1f);
        //what do I want the ai to do? summon a guy when they have the PP to do so,
        while (ourBase.GetHP() > 0)
        {
            if (um.GetEnmPPAmount() >= um.GetEnmUnitCost(0))//could make int random, to be fair rn this if not matter, they just spam the button
            { um.spawnEnemyUnit(0); }//they don't have multiple units or anything they do
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void followBuildStrat()//a method to check if you have enough cost for a build
    {
        //setBuild needs, cost of the plan, and spawn order, 
        if(um.GetEnmPPAmount()>=18)//2 infantry, then 2 ballers, based on
        {
            um.spawnEnemyUnit(1);
            um.spawnEnemyUnit(1);
            um.spawnEnemyUnit(2);
            um.spawnEnemyUnit(2);
        }
    }

    private void SpamEm(int one)
    {
        um.spawnEnemyUnit(one);
    }

    public IEnumerator SpamStratRoutine()
    {
        while (ourBase.GetHP() > 0)
        {
            //check for invaders
            int isInvaders = InvadersCheck();
            //Debug.Log("Invaders check found " + invaderComp.Count);
            if (isInvaders == 1)
            {
                ulti.popPlayerUlt(false);
            }
            else if(isInvaders==2)
            {
                //need to defend towers
            }

//high prio    //enemy units (maybe player units too) need to be able to follow a string of points.
               //so they can follow a path, around terrain

            //advance strategy, which should be variable
            //wait until a certain number to make specific build, or spam units if you can afford them, which could be cheapest or favorite

            //followBuildStrat();//thinking there might be an issue with spawning them istantly, but will test
            SpamEm(0);//I think in future a smart ai should look at multiple builds or strats and see which will work the best

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void lookForInvaders(Transform tam)//used to check if there are enemies threatening base or a tower
    {
        //cast pysics circle to check for player units
        Collider2D[] hits = Physics2D.OverlapCircleAll(tam.position, 14f);//might try different numbers
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

    private int InvadersCheck()//0 no, 1 base, 2 tower
    {
        lookForInvaders(this.transform);

        if (invaderComp.Count>0)
        {
            return 1;//might need to know where its being invaded, so return int, 0 no, 1 base, 2 tower
            //also we don't clear the list if we are being attacked!
        }

        if(towerTam!=null)//would be null if its destroyed pretty sure
        {
            lookForInvaders(towerTam);
            if (invaderComp.Count > 0) { return 2; }
        }

        //currently only checks around itself
        return 0;
    }
}


//https://www.reddit.com/r/gamedev/comments/wzbupc/strategy_game_devs_how_do_you_develop_your_ai_for/
//this post had a response talking about having the ai check sertain peramiters in the level then doing the following acordingly, which makes sense
//ai should ask these questions
//are enemies at my base? trigger box probably
//if they are can I ult? if not can I summon some units?
//otherwise follow my gameplan, which could be vatiable, and can be modified to set units on an optimal route or to seek out the food
//another could be check for lethal, or if enemy is low & doesn't have many units either between bases & sends a fast unit down the lane