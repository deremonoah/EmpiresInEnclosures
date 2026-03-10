using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterPickUP : MonoBehaviour
{
    [SerializeField] PickUpType whatIpickUpFast;

    public PickUpType DoWhot()//should prob be get pickUpType
    {
        return whatIpickUpFast;
    }
}
