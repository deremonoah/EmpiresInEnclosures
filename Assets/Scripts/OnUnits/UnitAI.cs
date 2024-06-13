using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this scrip handles the individual unit's actions: movment and attacking
public class UnitAI : MonoBehaviour
{
    private UnitStats myStats;
    private float moveSpeed;
    public Vector2 moveTarget;
    private HP attackTarget;
    private UnitState us;
    private UnitAnimator anim;
    private Coroutine currentRoutine;
    //ranged variables
    private Vector2 RangedTarget;
    public GameObject ProjectilePrefab;
    public GameObject shootPoint;
    //CodeMonkey
    private GameObject selectedGameObject;
    //fixing facing the wrong way
    private SpriteRenderer mysr;

    void Start()
    {
        myStats = GetComponent<UnitStats>();
        moveSpeed = myStats.getMoveSpeed();
        us = UnitState.move;
        mysr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Awake()
    {
        anim = this.gameObject.GetComponentInChildren<UnitAnimator>();
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    //make these sections later
    private void Update()
    {
        /*if(attackTarget==null)
        {
            us = UnitState.move;
            if (currentRoutine != null)
            { StopCoroutine(currentRoutine); }
        }*/
        if (us == UnitState.move)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
        }
        //maybe change to a move routine
    }
    
    public void SetMoveTarget(Vector2 pos)
    {
        moveTarget = pos;
    }

    public void SeeTarget(Vector2 pos, HP enmTarg)
    {
        //Debug.Log(this.name + " seeing");
        if (attackTarget == null && currentRoutine==null)
        {
            //only makes new target if old is null
            float atkRng = myStats.getAttackRange();
            Vector2 curruntPos = this.gameObject.transform.position;
            if (myStats.AmRanged())
            {
                RangedTarget = pos;
                attackTarget = enmTarg;
                currentRoutine = StartCoroutine(RangedAttackRoutine());
            }
            //are they in attack range
            else if (atkRng >= curruntPos.x - pos.x && atkRng >= curruntPos.y - pos.y)
            {
                attackTarget = enmTarg;
                currentRoutine = StartCoroutine(MeleeAttackRoutine());
            }
        }
        SetMoveTarget(pos);
    }

    public void Halt()
    {
        //this makes units stop when highlighted
    }

    public void setUnitState(UnitState state)
    {
        us = state;
        anim.SetAnimationState((int)us);
    }

    

    //attack stuff
    //attack type weather its ranged or melee (cavalry just moves faster probably)
    //based on attack speed that would be how often we check for a target(unit or base) then attack
    //on trigger stay?
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (us!=UnitState.attack && collision.gameObject.layer!=gameObject.layer)
        {
            us = UnitState.attack;
            anim.SetAnimationState((int)us);
            attackTarget = collision.gameObject.GetComponent<HP>();
            currentRoutine = StartCoroutine(MeleeAttackRoutine());
        }
    }*/

    private IEnumerator DoneFighting()
    {
        Debug.Log("done fighting called atk target?" );
        Debug.Log(attackTarget == null);
        Debug.Log("moveTarget?");
        Debug.Log(moveTarget == null);
        yield return new WaitForSeconds(0f);
        if (moveTarget==null)
        {
            Debug.Log("in fighting if");
            setUnitState(UnitState.move);
            moveTarget = FindObjectOfType<UnitManager>().GetmoveTarget(gameObject.layer).position;
            attackTarget = null;
        }
    }

    private IEnumerator MeleeAttackRoutine()
    {
        if (attackTarget!= null)
        {
            setUnitState(UnitState.attack);
            yield return new WaitForSeconds(myStats.getAttackSpeed());
            if (attackTarget != null)
            { 
                attackTarget.DamageThis(myStats.getAttack());
                currentRoutine = StartCoroutine(MeleeAttackRoutine());
            }
            else { StartCoroutine(DoneFighting()); }
        }
        else { StartCoroutine(DoneFighting()); }

    }

    private IEnumerator RangedAttackRoutine()
    {
        moveTarget = this.gameObject.transform.position;
        setUnitState( UnitState.attack);
        //below line should fix the missing if the nemy unit moved form initial position
        RangedTarget = attackTarget.gameObject.transform.position;
        //then instantiate projectile, give it target, then wait. or maybe wait 1st? well am trying 1st
        var shot = Instantiate(ProjectilePrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        shot.GetComponent<Projectile>().SetTarget(RangedTarget, this.gameObject);
        yield return new WaitForSeconds(myStats.getAttackSpeed());
        if (attackTarget != null)
        { currentRoutine= StartCoroutine(RangedAttackRoutine()); }
        else { StartCoroutine(DoneFighting()); }
    }
}
public enum UnitState { idle,move,attack}