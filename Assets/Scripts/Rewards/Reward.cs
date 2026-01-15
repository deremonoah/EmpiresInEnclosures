using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : ScriptableObject
{
    [SerializeField] string rName;
    [SerializeField] Sprite icon;
    [SerializeField] string description;

    public string getName()
    {
        return rName;
    }

    public Sprite getIcon()
    {
        return icon;
    }

    public string getDescription()
    {
        return description;
    }

    public virtual void SelectReward()
    {
        //overridden by child classes for specific uses
        //the term is polymorphism I believe
    }
}
