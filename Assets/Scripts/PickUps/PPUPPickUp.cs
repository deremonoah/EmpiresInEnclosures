using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PPUpPickUp", menuName = "PickUpAbilities/PPUpPickUp")]
public class PPUPPickUp : PickUpAbility
{
    [SerializeField] float strength;
    [SerializeField] float TimeToPickUp;

    public override void ActivatePickUp(Transform origin, int isPlayerLayer)
    {
        if(isPlayerLayer==7)//player got it
        {
            UnitManager.instance.PlayerGetsPower(strength, true);
            Debug.Log("player got PP");
        }
        else
        {
            UnitManager.instance.EnemyGetsPower(strength, true);
            Debug.Log("enemy got PP");
        }
    }


    public float getTimeToPickUP()
    {
        return TimeToPickUp;
    }
}
