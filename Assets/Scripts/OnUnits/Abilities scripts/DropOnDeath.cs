using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropOnDeathAbility",menuName ="Abilities/dropOnDeathAbility")]
public class DropOnDeath : OnDeathAbility
{
    [SerializeField]
    private GameObject dropPrefab;
    [SerializeField]
    private int numberToDrop;

    public override void UseAbility(Transform origin)
    {
        Debug.Log("Did we get in use ability");
        for(int lcv=0;lcv<numberToDrop;lcv++)
        {
            Instantiate(dropPrefab, origin.position,origin.rotation);//maybe ramdomize around origin?
        }
    }
}
