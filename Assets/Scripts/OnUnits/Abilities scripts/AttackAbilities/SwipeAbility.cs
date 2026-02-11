using UnityEngine;

[CreateAssetMenu(fileName = "SwipeAbility", menuName = "Abilities/SwipeAbility")]

public class SwipeAbility : AttackAbility
{
    [SerializeField] int extraTargets;
    [SerializeField] float swipeRange;

    public override void UseAttackAbility(HP target, GameObject attacker, int layerToAttack)
    {
        //ai handles regular attack
        //this will find the nearest number of extraTargets to the target, in range and hurt them

        //get target position
        Vector2 pos = target.transform.position;
        //get attacker's damage
        UnitStats dmg = attacker.GetComponent<UnitStats>();
        //cast physics circle there
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, swipeRange);
        GameObject oneToHit=null;
        float closestDis=swipeRange+1;

        foreach(Collider2D col in hits)
        {
            var tar = col.gameObject;
            Vector2 tarPos= col.gameObject.transform.position;

            if (tar.layer==layerToAttack)
            {
                var newDis = Vector2.Distance(pos, tarPos);
                if(newDis<closestDis && newDis!=0)
                {
                    closestDis = newDis;
                    oneToHit = tar;
                }
            }
        }

        //deal attackers damage to the closest one
        if (oneToHit!=null)
        {
            oneToHit.GetComponent<HP>().ThisAttackedYou(dmg);
        }
    }
}
