using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this holds all the stats and is where they would be set on the scriptable object
public class UnitStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int Cost;
    [SerializeField] private float MaxHp;

    [Header("Attack Stats")]
    [SerializeField] private float Attack;
    [SerializeField] private float AttackToBase;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AnimAttackPreHit;
    [Header("Ranged Stats")]
    [SerializeField] private float AttackRange;
    [SerializeField] private float SightRange;


    [Header("Speed Stats")]// header not added?
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float SwimSpeed;
    [SerializeField] private float MountainSpeed;
    

    [Header("Unit Classifications")]
    [SerializeField] private AttackType _unitType;
    [SerializeField] private UnitType _unitRole;//role is fro enemy ai, while type is for resolving the type of attack I think ut past Noah shit

    [Header("armor maybe more later")]
    [SerializeField] private float Armor;

    [Header("Unit Death Stats")]
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
    [SerializeField] private float bonusMoveSpeed;
    [SerializeField] private float bonusWaterMoveSpeed;
    [SerializeField] private float bonusMountianMoveSpeed;
    [SerializeField] private float bonusAllSpeed;
    [SerializeField] private float bonusHP;//will need to integrate unit hp using this class probably?
    [SerializeField] private float bonusSightRange;//certain effected classes could have a public updated from buff call
    [SerializeField] private float bonusAttackRange;
    [SerializeField] private float bonusArmor;
    [SerializeField] private float bonusPayOnDeathFriendly;
    [SerializeField] private float bonusPayOnDeathEnemy;
    //I feel like can't stack or stack limit, not sure how to calculate that though
    //maybe a list of units buffing you, and check their type, which shouldn't be done every frame

    private void Start()//for getting auras from the equipManager
    {
        List<AuraAbility> unitBuffs = new List<AuraAbility>();
        if(gameObject.layer==7)
        {
            unitBuffs=EquipManagerPlayer.instance.getPlayerBuffs();
        }
        else if(gameObject.layer==6)
        {
            //thinking equipManager should be for both player and enemy
        }
        foreach(AuraAbility buff in unitBuffs)
        {
            BuffedFrom(buff);
        }
    }

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
        { return Mathf.Clamp(SwimSpeed+ bonusAllSpeed + bonusWaterMoveSpeed,0, 99); }

        else if(ter==Terrain.mountain)
        { return Mathf.Clamp(MountainSpeed + bonusAllSpeed + bonusMountianMoveSpeed, 0, 99); }

        return Mathf.Clamp(MoveSpeed + bonusMoveSpeed + bonusAllSpeed, 0, 99);
    }

    public bool AmRanged()
    {
        if (_unitType == AttackType.ranged) { return true; }
        return false;
    }

    public float getSightRange()
    {
        return SightRange;
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

    public UnitType getRole()
    { return _unitRole; }

    public float getAttackWaitTime()
    { return AnimAttackPreHit; }

#region buffs
    public void BuffedFrom(AuraAbility buff)
    {
        auraAdders.Add(buff);
        //ResolveBuffsConservative();
        ResolveBuffAllIn();
    }
    
    private void ResolveBuffAllIn()//allows buffs to stack
    {
        auraAdders.Distinct();//so a buff target isn't on there twice
        ClearBuffsForRecalculate();
        foreach (AuraAbility buff in auraAdders)
        {
            float buffValue = buff.getBuffStength();
            if(_unitRole==buff.getTypeToBuff())
            {
                switch (buff.getBuffType())
                {
                    case BuffsType.attack:
                        bonusAttack += buffValue;
                        break;
                    case BuffsType.attackToBase:
                        bonusAttackToBase += buffValue;
                        break;
                    case BuffsType.attackRange:
                        bonusAttackRange += buffValue;
                        break;
                    case BuffsType.attackSpeed:
                        bonusAttackSpeed += buffValue;
                        break;
                    case BuffsType.sightRange:
                        bonusHP += buffValue;
                        break;
                    case BuffsType.HP:
                        bonusHP += buffValue;
                        break;
                    case BuffsType.armor:
                        bonusArmor += buffValue;
                        break;
                    case BuffsType.AllmoveSpeed:
                        bonusAllSpeed += buffValue;
                        break;
                    case BuffsType.BonusPayOnDeathEnemy:
                        bonusPayOnDeathFriendly += buffValue;
                        break;
                    case BuffsType.BonusPayOnDeathFriendly:
                        bonusPayOnDeathFriendly += buffValue;//for now only the biggest + or - applies
                        break;
                    case BuffsType.normalMoveSpeed:
                        bonusMoveSpeed += buffValue;
                        break;
                    case BuffsType.waterMoveSpeed:
                        bonusWaterMoveSpeed += buffValue;
                        break;
                    case BuffsType.mountainMoveSpeed:
                        bonusMountianMoveSpeed += buffValue;
                        break;
                }
            }
        }
    }

    private void ClearBuffsForRecalculate()
    {
        bonusAttack = 0;
        bonusAttackToBase = 0;
        bonusAttackSpeed = 0;
        bonusMoveSpeed = 0;
        bonusWaterMoveSpeed = 0;
        bonusMountianMoveSpeed = 0;
        bonusHP = 0;
        bonusSightRange = 0;
        bonusAttackRange = 0;
        bonusArmor = 0;
        bonusPayOnDeathFriendly = 0;
        bonusPayOnDeathEnemy = 0;
    }

    private void ResolveBuffsConservative()//doesn't allow them to stack
    {
        //buffs that are larger have priority, so if you have enemy units that debuff attack by 2 and your unit buffs your units by 1, should they combine? or only apply biggest absolute value?
        //could I handle each stat by itself?
        Debug.Log("we got in Resolve");

        auraAdders.Distinct();

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
                case BuffsType.AllmoveSpeed:
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
        ResolveBuffsConservative();
    }
#endregion
}
public enum AttackType { melee, ranged }
public enum UnitType { 
    infantry,
    range, 
    fast, 
    big, 
    tower,
    Base , 
    all}//all is for buff aura to use & includes towers but not bases(as bases don't have a unitAI component)
//seige might have custome sight collider set in the editor