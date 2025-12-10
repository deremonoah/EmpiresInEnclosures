using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this holds all the stats and is where they would be set on the scriptable object
public class UnitStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float MaxHp;
    [SerializeField] private int Cost;

    [Header("Speed Stats")]// header not added?
    [SerializeField] private float SwimSpeed;
    [SerializeField] private float MountainSpeed;
    [SerializeField] private float MoveSpeed;

    [Header("Attack Stats")]
    [SerializeField] private float Attack;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private Vector2 SightRange;
    [SerializeField] private float AttackRange;
    [Header("Unit Classifications")]
    [SerializeField] private UnitType ut;
    [SerializeField] private UnitRole ur;//role is fro enemy ai, while type is for resolving the type of attack I think ut past Noah shit
    [SerializeField] private float FriendlyPayOnDeath;
    [SerializeField] private float EnemyPayOnDeath;
    [SerializeField] private Sprite portrait;
    //maybe armor and stuff

    public int getCost()
    {
        return Cost;
    }

    public float getAttack()
    {
        return Attack;
    }

    public float getAttackSpeed()
    {
        return AttackSpeed;
    }

    public float getMaxHp()
    {
        return MaxHp;
    }

    public float getMoveSpeed(Terrain ter)
    {
        if(ter==Terrain.water)
        { return SwimSpeed; }
        else if(ter==Terrain.mountain)
        { return MountainSpeed; }
        return MoveSpeed;
    }

    public bool AmRanged()
    {
        if (ut == UnitType.ranged) { return true; }
        return false;
    }

    public Vector2 getSightRange()
    {
        return SightRange;
    }

    public float getAttackRange()
    {
        return AttackRange;
    }

    public float getFriendlyPayOnDeath()
    {
        return FriendlyPayOnDeath;
    }

    public float getEnemyPayOnDeath()
    {
        return EnemyPayOnDeath;
    }

    public Sprite getIcon()
    {
        return portrait;
    }

    public UnitRole getRole()
    { return ur; }
}
public enum UnitType { melee, ranged, seige }
public enum UnitRole { infantry,range, fast, big}
//seige might have custome sight collider set in the editor