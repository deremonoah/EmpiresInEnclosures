using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class handles the spawning the player can do with buttons
//and will handle the enemy spawns
public class UnitManager : MonoBehaviour
{
    public Transform PlayerBasePos;
    public List<GameObject> PlayerUnitPrefabs;
    public List<GameObject> enemyPrefabs;
    public Transform EnemyBasePos;
    [SerializeField] private float PP, MaxPP=12, PPRegenTimer,PPRegenTimerMax;
    public Text ppText, ppMaxText;

    private void Start()
    {
        ppText.text = "" + PP;
        ppMaxText.text = "" + MaxPP;
    }

    private void Update()
    {
        PPRegenTimer -= Time.deltaTime;
        if(PPRegenTimer<=0)
        {
            //set up this way if you get pp over max with the penguin pandemic ability it can go over. And with the death of stuff that will have to change
            if (PP+1 <= MaxPP) { PP++; }
            PPRegenTimer = PPRegenTimerMax;
        }
        ppText.text = "" + PP;
    }

    public void spawnPlayerUnit(int lcv)
    {
        int unitCost = PlayerUnitPrefabs[lcv].GetComponent<UnitStats>().getCost();
        if (PP >= unitCost)
        {
            PP -= unitCost;
            ppText.text = "" + PP;
            //instantiate prefab at spawnPos.pos
            var unit = Instantiate(PlayerUnitPrefabs[lcv], PlayerBasePos.position, PlayerBasePos.rotation);
            PlayerUnitPrefabs[lcv].GetComponent<UnitStats>().getCost();
            unit.GetComponent<UnitAI>().SetMoveTarget(EnemyBasePos.position);
            unit.GetComponent<UnitAI>().setUnitState(UnitState.move);
            unit.gameObject.layer = 7;
            //truned off friendly fire
        }
        
    }
    public void spawnEnemyUnit(int lcv)
    {
        //instantiate prefab at spawnPos.pos
        var unit = Instantiate(enemyPrefabs[lcv], EnemyBasePos.position, EnemyBasePos.rotation);
        unit.GetComponent<UnitAI>().SetMoveTarget(PlayerBasePos.position);
        unit.GetComponent<UnitAI>().setUnitState(UnitState.move);
        //setting to enemy unit layer so they don't kill each other
        unit.gameObject.layer = 6;
    }

    public void spawnEnemyUnit(int lcv,Transform pos, bool stay)
    {
        //instantiate prefab at spawnPos.pos
        var unit = Instantiate(enemyPrefabs[lcv], pos.position, EnemyBasePos.rotation);
        if (!stay)
        {
            unit.GetComponent<UnitAI>().SetMoveTarget(PlayerBasePos.position);
            unit.GetComponent<UnitAI>().setUnitState(UnitState.move);
        }
        else { unit.GetComponent<UnitAI>().SetMoveTarget(pos.position); }
        //setting to enemy unit layer so they don't kill each other
        unit.gameObject.layer = 6;
    }

    public Transform GetmoveTarget(int layer)
    {
        if (layer == 6)
        {   return PlayerBasePos; }
        return EnemyBasePos;
    }

    public void PlayerGetsPower(float p,bool canGoAbove)
    {
        if(canGoAbove)
        {
            PP += p;
        }else 
        {
            if(PP+p<=MaxPP)
            {
                PP += p;
            }//else they just don't get it because it would go over max or is max and this won't overwrite the penguin pandemic ability
        }
    }

    public void EnemyGetsPower(float p, bool canGoAbove)
    {
        /*make it for enemy later
         * if (canGoAbove)
        {
            PP += p;
        }
        else
        {
            if (PP + p <= MaxPP)
            {
                PP += p;
            }//else they just don't get it because it would go over max or is max and this won't overwrite the penguin pandemic ability
        }*/
    }
}
