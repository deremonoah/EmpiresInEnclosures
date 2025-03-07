using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this holds all the stats and is where they would be set on the scriptable object
public class UnitStats : MonoBehaviour
{
    [SerializeField] private int Cost;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float MaxHp;
    [SerializeField] private float Attack;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private Vector2 SightRange;
    [SerializeField] private float AttackRange;
    [SerializeField] private UnitType ut;
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

    public float getMoveSpeed()
    {
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
}
public enum UnitType { melee, ranged, seige }
//seige might have custome sight collider set in the editor