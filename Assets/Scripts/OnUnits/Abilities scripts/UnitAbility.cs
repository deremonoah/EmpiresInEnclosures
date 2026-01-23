using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAbility : ScriptableObject
{
    public abstract void UseAbility(Transform origin);
}
