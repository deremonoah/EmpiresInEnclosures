using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this scrip handles the individual unit's actions: movment and attacking
public class UnitAI : MonoBehaviour
{
    private UnitStats myStats;
    private float currentSpeed;
    public Vector2 moveTarget;
    public HP attackTarget;
    [SerializeField] UnitState unitState;
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
    private UnitManager urManager;

    [Header("universal movement varience")]
    public Vector2 xRange;
    public Vector2 yRange;

    void Start()
    {
        myStats = GetComponent<UnitStats>();
        currentSpeed = myStats.getMoveSpeed(Terrain.normal);
        unitState = UnitState.move;
        mysr = GetComponentInChildren<SpriteRenderer>();
        urManager = FindObjectOfType<UnitManager>();
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
        if (unitState == UnitState.move)
        {
            float step = currentSpeed * Time.deltaTime;
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
        //Debug.Log(this.name + " seeing attack target is null? "+(attackTarget==null)+" currentRoutine is null? " +(currentRoutine==null));
        if (attackTarget == null)// was ||  attackTarget.GetType() == typeof(BaseHP). testing if you getting to tank with base can get you back in the game
        {
            //only makes new target if old is null or they are attacking a base
            StopAllCoroutines();
            //makes sure we don't double up on attack routines

            //helps be more natural
            pos = RandomizePos(pos);

            float atkRng = myStats.getAttackRange();
            Vector2 curruntPos = this.gameObject.transform.position;
            if (myStats.AmRanged())
            {
                RangedTarget = pos;
                attackTarget = enmTarg;
                setUnitState(UnitState.attack);
                currentRoutine = StartCoroutine(RangedAttackRoutine());
            }
            //are they in attack range
            else if (atkRng >= curruntPos.x - pos.x && atkRng >= curruntPos.y - pos.y)
            {
                attackTarget = enmTarg;
                setUnitState(UnitState.attack);
                currentRoutine = StartCoroutine(MeleeAttackRoutine());
            }
        }
        SetMoveTarget(pos);
    }

    public Vector3 RandomizePos(Vector3 aroundHere)
    {
        float randx = Random.Range(xRange.x, xRange.y);
        float randy = Random.Range(yRange.x, yRange.y);

        Vector3 randSpawn = new Vector3(aroundHere.x + randx, aroundHere.y + randy, aroundHere.z);
        return randSpawn;
    }

    public void Halt()
    {
        //this makes units stop when highlighted
    }

    public void setUnitState(UnitState state)
    {
        unitState = state;
        anim.SetAnimationState((int)unitState);
    }

    
    public void UpdateSpeed(Terrain ter)// maybe sub this to event on UnitTrainFeed, should be terrain oops
    {
        currentSpeed = myStats.getMoveSpeed(ter);
        //also need to increase sight size based on being on mountains
        if(ter==Terrain.mountain)
        {
            //will try this in a bit
            //I realize that if its only on or off, then players can just step bareley on it and get the same buff
            //as a player who walked to the center, maybe based off terrains center point? eh work with this for now
        }
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

    private void DoneFighting()
    {
        /*Debug.Log("done fighting called atk target?" );
        Debug.Log(attackTarget == null);
        Debug.Log("moveTarget?");
        Debug.Log(moveTarget == null);*/
        moveTarget = urManager.GetmoveTarget(gameObject.layer).position;
        this.setUnitState(UnitState.move);
        //moveTarget = urManager.GetmoveTarget(gameObject.layer).position;
    }

    private IEnumerator MeleeAttackRoutine()
    {
            while (attackTarget != null)
            { 
                yield return new WaitForSeconds(myStats.getAttackSpeed());
                if (attackTarget != null)
                {
                var isBase=attackTarget.gameObject.GetComponent<BaseHP>();
                if (isBase != null)
                { attackTarget.DamageThis(myStats.getAttack(true)); /*Debug.Log("attack target not null");*/ }
                else
                { attackTarget.DamageThis(myStats.getAttack(false)); }
                }
                yield return null;
            }
        //change movetarget
        DoneFighting();
    }

    private IEnumerator RangedAttackRoutine()
    {
        moveTarget = this.gameObject.transform.position;
        //could move 1st line of code from ShootShot() here if we want it maybe to miss?
        
        while (attackTarget != null)
        {
            yield return new WaitForSeconds(myStats.getAttackSpeed());
            ShootShot();
            yield return null;
        }
        DoneFighting();
    }

    private void ShootShot()
    {
        //below line makes missing harder, but idk if we need it every call
        if (attackTarget != null)
        { RangedTarget = attackTarget.gameObject.transform.position; }
        //then instantiate projectile, give it target, then wait. or maybe wait 1st? well am trying 1st
        var shot = Instantiate(ProjectilePrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        shot.GetComponent<Projectile>().SetTarget(RangedTarget, this.gameObject);
    }
}
public enum UnitState { idle,move,attack}