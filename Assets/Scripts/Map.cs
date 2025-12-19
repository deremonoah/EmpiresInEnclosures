using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //this is like a map label, which holds the faction's level
    [SerializeField] Faction myFaction;

    public Faction GetFaction()
    {
        return myFaction;
    }
}
