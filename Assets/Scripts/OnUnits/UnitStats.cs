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
    [SerializeField] private List<AuraAbility> auraAdders;
    [SerializeField] private float bonusAttack;
    [SerializeField] private float bonusAttackToBase;
    [SerializeField] private float bonusAttackSpeed;//bonus will be negative for faster
    [SerializeField] private float bonusMoveSpeed;//hmm maybe for specific terrain?
    [SerializeField] private float bonusHP;//will need to integrate unit hp using this class probably?
    [SerializeField] private float bonusSightRange;//certain effected classes could have a public updated from buff call
    [SerializeField] private float bonusAttackRange;
    [SerializeField] private float bonusArmor;
    [SerializeField] private float bonusPayOnDeathFriendly;
    [SerializeField] private float bonusPayOnDeathEnemy;
    //I feel like can't stack or stack limit, not sure how to calculate that though
    //maybe a list of units buffing you, and check their type, which shouldn't be done every frame

    public int getCost()//could be modified?
    {
        return Cost;
    }

    public float getAttack()
    {
        return Mathf.Clamp(Attack + bonusAttack,1,1000); //idk when damage would get over 1000
    }

    public float getBaseAttack()
    {
        return Mathf.Clamp(AttackToBase + bonusAttackToBase, 1, 1000);
    }

    public float getAttackSpeed()
    {
        return AttackSpeed + bonusAttackSpeed;
    }

    public float getMaxHp()
    {
        return MaxHp + bonusHP;
    }

    public float getArmor()
    {
        return Mathf.Clamp(Armor + bonusArmor,0,100);//prob never more than 100 armor
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
        return SightRange + SightRange;
    }

    public float getAttackRange()
    {
        return AttackRange + bonusAttackRange;
    }

    public float getFriendlyPayOnDeath()
    {
        return FriendlyPayOnDeath + bonusPayOnDeathFriendly;
    }

    public float getEnemyPayOnDeath()
    {
        return EnemyPayOnDeath + bonusPayOnDeathEnemy;
    }

    public Sprite getIcon()
    {
        return portrait;
    }

    public UnitRole getRole()
    { return ur; }

    public float getAttackWaitTime()
    { return AnimAttackPreHit; }

#region buffs
    public void BuffedFrom(AuraAbility buff)
    {
        auraAdders.Add(buff);
        ResolveBuffs();
    }
    
    private void ResolveBuffs()
    {
        //buffs that are larger have priority, so if you have enemy units that debuff attack by 2 and your unit buffs your units by 1, should they combine? or only apply biggest absolute value?
        //could I handle each stat by itself?
        Debug.Log("we got in Resolve");
        foreach(AuraAbility buff in auraAdders)
        {
            float absBuffStrength = Mathf.Abs(buff.getBuffStength());
            switch (buff.getBuffType())
            {
                case BuffsType.attack:
                    bonusAttack= Mathf.Max(bonusAttack, absBuffStrength);
                    break;
                case BuffsType.attackToBase:
                    bonusAttackToBase = Mathf.Max(bonusAttackToBase, absBuffStrength);
                    break;
                case BuffsType.attackRange:
                    bonusAttackRange = Mathf.Max(bonusAttackRange, absBuffStrength);
                    break;
                case BuffsType.attackSpeed:
                    bonusAttackSpeed = Mathf.Max(bonusAttackSpeed, absBuffStrength);
                    break;
                case BuffsType.sightRange:
                    bonusHP = Mathf.Max(bonusHP, absBuffStrength);
                    break;
                case BuffsType.HP:
                    bonusHP = Mathf.Max(bonusHP, absBuffStrength);
                    break;
                case BuffsType.armor:
                    bonusArmor= Mathf.Max(bonusArmor, absBuffStrength);
                    break;
                case BuffsType.moveSpeed:
                    //will worry about which specific one later
                    break;
                case BuffsType.BonusPayOnDeathEnemy:
                    bonusPayOnDeathFriendly = Mathf.Max(bonusPayOnDeathFriendly, absBuffStrength);
                    break;
                case BuffsType.BonusPayOnDeathFriendly:
                    bonusPayOnDeathFriendly=Mathf.Max(bonusPayOnDeathFriendly, absBuffStrength);//for now only the biggest + or - applies
                    break;
                
            }

        }
    }

    public void RemovedBuffFrom(AuraAbility buff)
    {
        auraAdders.Remove(buff);
        ResolveBuffs();
    }
#endregion
}
public enum UnitType { melee, ranged, seige }
public enum UnitRole { infantry,range, fast, big}
//seige might have custome sight collider set in the editor