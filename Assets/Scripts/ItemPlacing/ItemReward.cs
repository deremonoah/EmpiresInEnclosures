using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemPlaceAble",menuName = "LootRewards/ItemPlaceAble")]
public class ItemReward : Reward
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] int uses;

    public GameObject getPrefab()
    {
        return itemPrefab;
    }

    public int getUses()
    {
        return uses;
    }

    public override void SelectReward()
    {
        PlacingController.instance.GainedNewItem(this);
        EquipManagerPlayer.instance.gotnewItem(this);
    }
}
