using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHP : HP
{
    public override bool Die() 
    {
        Debug.Log("somebody lost");
        return true;
    }
}
