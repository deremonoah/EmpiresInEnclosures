using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManagerPlayer : MonoBehaviour
{
    //the job of this class is to handle any upgrades to player units or bases
    [SerializeField] private GameObject playerBasePrefab;
    [SerializeField] private GameObject playerTowerPrefab;//serialize field for now
    public static EquipManagerPlayer instance;

    [SerializeField] List<AuraAbility> playerBuffs;

    private void Awake()
    {
        if(instance!=null & instance != this)
        {
            Debug.LogError("2 equip managers in the scene");
        }
        else
        {
            instance = this;
        }
    }
    //handle buffs to tower or base
    //handle blue prints in here?
    //handle equp buffs like to all units or bases or unit types

    public void InitialEquip(Faction fac)
    {
        //this will get called when player selects their zoonets, for initial base & tower prefabs
    }

    public GameObject GetPlayerBase()
    {
        return playerBasePrefab;
    }

    public GameObject GetPlayerTower()
    {
        return playerTowerPrefab;
    }

    public void GainedNewBuff(AuraAbility buff)
    {
        playerBuffs.Add(buff);
        //infuture this might need to handle different kinds of buffs
    }

    //or more GainedNewBuff(buffToTower)

    public List<AuraAbility> getPlayerBuffs()
    {
        return playerBuffs;
    }

    public float GetPlayerBaseBuff()
    {
        float totalBasebuff = 0;
        foreach(AuraAbility buff in playerBuffs)
        {
            if(buff.getTypeToBuff()==UnitType.Base)
            {
                totalBasebuff += buff.getBuffStength();
            }
        }
        return totalBasebuff;
    }
}
