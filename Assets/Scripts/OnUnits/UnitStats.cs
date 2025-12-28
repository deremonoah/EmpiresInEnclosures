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

    [Header("armor maybe more later")]
    [SerializeField] private float Armor;

    [Header("Attack Stats")]
    [SerializeField] private float Attack;
    [SerializeField] private float AttackToBase;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AnimAttackPreHit;
    [Header("Ranged Stats")]
    [SerializeField] private Vector2 SightRange;
    [SerializeField] private float AttackRange;
    [Header("Unit Classifications")]
    [SerializeField] private UnitType ut;
    [SerializeField] private UnitRole ur;//role is fro enemy ai, while type is for resolving the type of attack I think ut past Noah shit
    [SerializeField] private float FriendlyPayOnDeath;
    [SerializeField] private float EnemyPayOnDeath;
    [SerializeField] private Sprite portrait;
    //maybe armor and stuff

    [Header("Buffs so I can check their function for now")]
    //buff stuff from aura abilities, should this be its own class? but then it would need to call another function to calcuate buff or check with an if
    [SerializeField] private float bonusAttack;
    [SerializeField] private float bonusAttackToBase;
    [SerializeField] private float bonusAttackSpeed;//bonus will be negative for faster
    [SerializeField] private float bonusMoveSpeed;
    [SerializeField] private float bonusHP;//will need to integrate unit hp using this class probably?
    [SerializeField] private float bonusRange;//certain effected classes could have a public updated from buff call
    [SerializeField] private float bonusArmor;
    //I feel like can't stack or stack limit, not sure how to calculate that though
    //maybe a list of units buffing you, and check their type, which shouldn't be done every frame

    public int getCost()
    {
        return Cost;
    }

    public float getAttack()
    {
        return Mathf.Clamp(Attack+bonusAttack,0,1000); //idk when damage would get over 1000
    }

    public float getBaseAttack()
    {
        return Mathf.Clamp(AttackToBase + bonusAttackToBase, 0, 1000);
    }

    public float getAttackSpeed()
    {
        return AttackSpeed;
    }

    public float getMaxHp()
    {
        return MaxHp+bonusHP;
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

    public float getAttackWaitTime()
    { return AnimAttackPreHit; }
}
public enum UnitType { melee, ranged, seige }
public enum UnitRole { infantry,range, fast, big}
//seige might have custome sight collider set in the editor