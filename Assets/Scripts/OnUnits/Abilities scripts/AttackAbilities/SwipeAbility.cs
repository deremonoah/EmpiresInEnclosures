using UnityEngine;
using System.Linq;

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
        GameObject[] onesToHit= new GameObject[extraTargets];
        Debug.Log("array length " + onesToHit.Length);
        float closestDis=swipeRange+1;


        for(int lcv=0;lcv<onesToHit.Length;lcv++)
        {
            closestDis = swipeRange + 1;
            foreach (Collider2D col in hits)
            {
                var tar = col.gameObject;
                Vector2 tarPos = col.gameObject.transform.position;

                if (tar.layer == layerToAttack)
                {
                    var newDis = Vector2.Distance(pos, tarPos);
                    if (newDis < closestDis && newDis != 0)
                    {
                        if(!onesToHit.Contains(tar))
                        {
                            closestDis = newDis;
                            onesToHit[lcv] = tar;
                            Debug.Log("we have set " + lcv);
                        }
                    }
                }
            }
        }

        //deal attackers damage to the closest one
        for(int lcv=0;lcv<onesToHit.Length;lcv++)
        {
            if (onesToHit[lcv] != null)
            {
                onesToHit[lcv].GetComponent<HP>().ThisAttackedYou(dmg);
            }
            else { Debug.Log("pos " + lcv + " is empty"); }
        }
        
    }
}
