using System.Collections.Generic;
using UnityEngine;

public class EquipManagerPlayer : MonoBehaviour
{
    //the job of this class is to handle any upgrades to player units or bases
    [SerializeField] private GameObject playerBasePrefab;
    [SerializeField] private GameObject playerTowerPrefab;//serialize field for now
    public static EquipManagerPlayer instance;

    [SerializeField] List<AuraAbility> playerBuffs;

    [SerializeField] private List<UnitReward> playerUnits;//we now set the rewards for initial equips, if we want thing to work as intended
    [SerializeField] private List<Reward> playerItems;
    [SerializeField] private List<Reward> playerUpgradeHistory;

    
    private void Awake()
    {
        if(instance!=null & instance != this)
        {
            //Debug.LogError("2 equip managers in the scene");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            //for now
            foreach(UnitReward re in playerUnits)
            {
                playerUpgradeHistory.Add(re);
            }

            foreach (ItemReward re in playerItems)
            {
                playerUpgradeHistory.Add(re);
            }
            //placingController & UnitManager grab initial list from here
        }
    }
    //handle buffs to tower or base
    //handle blue prints in here? no thats in itemsToBePlaced
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

    public float GetPlayerStartingPPBuff()
    {
        float totalPPbuff = 0;
        foreach (AuraAbility buff in playerBuffs)
        {
            if (buff.getBuffType() == BuffsType.PPStartUP)
            {
                totalPPbuff += buff.getBuffStength();
            }
        }
        return totalPPbuff;
    }

    public void gotNewUnit(Reward re)
    {
        playerUnits.Add((UnitReward)re);
        playerUpgradeHistory.Add(re);
        ButtonManager.instance.UnitListChanged();
    }

    public void gotnewItem(Reward re)
    {
        playerItems.Add(re);
        playerUpgradeHistory.Add(re);
        ButtonManager.instance.UpdateItemList();
    }

    public List<UnitReward> getPlayerUnits()
    {
        return playerUnits;
    }

    public List<Reward> getPlayerItems()
    {
        return playerItems;
    }

    public List<Reward> GetUpgradeHistory()
    {
        return playerUpgradeHistory;
    }

    public UnitReward getUnitAtIndex(int index)
    {
        return playerUnits[index];
    }


}
