using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffReward", menuName = "LootRewards/Buff")]
public class BuffReward : Reward
{
    [SerializeField] AuraAbility theBuffItself;
    public override void SelectReward()
    {
        EquipManagerPlayer.instance.GainedNewBuff(theBuffItself);
    }


}
