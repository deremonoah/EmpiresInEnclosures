using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManagerPlayer : MonoBehaviour
{
    //the job of this class is to handle any upgrades to player units or bases
    [SerializeField] private GameObject playerBasePrefab;
    [SerializeField] private GameObject playerTowerPrefab;//serialize field for now
    public static EquipManagerPlayer instance;

    [SerializeField] List<AuraAbility> UnitStatbuffs;

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

    public void GainedNewBuff(BuffReward buff)
    {
        //maybe a fork here for it buff is to base gotta handle it
        Debug.LogError("still need to actually code handling the buff in EquipManagerPlayer");
    }

    public List<AuraAbility> getPlayerBuffs()
    {
        return UnitStatbuffs;
    }


}
