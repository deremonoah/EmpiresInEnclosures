using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpReciever : MonoBehaviour
{
    private PickUp itemOn;//what if player puts multiple in the same area? only one at a time

    public void OnThisItem(pickUp pu)
    {

    }

    private void OnDestroy()
    {
        if(itemOn!=null)
        {

        }
    }
}
