using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitReward", menuName = "LootRewards/Unit")]
public class UnitReward : Reward
{
    [SerializeField] GameObject unitPrefab;
    //visuals? for stats or just grabs the actual units stats?

    public GameObject GetPrefab()
    {
        return unitPrefab;
    }

    public override void SelectReward()
    {
        UnitManager.instance.PlayerGotNewUnit(unitPrefab);
        EquipManagerPlayer.instance.gotNewUnit(this);
    }

    public int getCost()
    {
        return unitPrefab.GetComponent<UnitStats>().getCost();
    }

    public UnitStats getStats()
    {
        return unitPrefab.GetComponent<UnitStats>();
    }
}
