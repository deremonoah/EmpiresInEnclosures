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

    public float getTimeToPickUp()
    {
        var abl=itemPrefab.GetComponent<PickUp>().getAbility();
        return abl.getTimeToPickUP();
    }

    public PickUpType getPickUpType()
    {
        var abl = itemPrefab.GetComponent<PickUp>().getAbility();
        return getPickUpType();
    }

    public override void SelectReward()
    {
        PlacingController.instance.GainedNewItem(this);
        EquipManagerPlayer.instance.gotnewItem(this);
    }
}
