using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this scrip handles the individual unit's actions: movment and attacking
public class UnitAI : MonoBehaviour
{
    private UnitStats myStats;
    [SerializeField] bool GotToWhereIshouldHave=false;
    [SerializeField] 
    private float currentSpeed;
    private Vector2 moveTarget;
    [SerializeField]
    private HP attackTarget;

    [SerializeField] 
    UnitState unitState;
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
        setUnitState(UnitState.move);
        mysr = GetComponentInChildren<SpriteRenderer>();
        urManager = FindObjectOfType<UnitManager>();
        
        if (this.gameObject.layer == 7)//so on the player layer, flip the transform 180 so it faces the right way to do animations
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    private void Awake()
    {
        anim = this.gameObject.GetComponentInChildren<UnitAnimator>();
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
        currentRoutine = null;
    }

    private void OnDisable()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    //make these sections later
    private void Update()
    {

        if (unitState == UnitState.move)
        {
            float step = currentSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, step);
        }

    }
    
    public void SetMoveTarget(Vector2 pos)
    {
        moveTarget = pos;
    }

    public void SeeTarget(Vector2 pos, HP enmTarg)
    {
        //to catch any non hp sighted things, so it won't start a routine early
        if(enmTarg == null)
        {
            return;
        }

        if (attackTarget == null && currentRoutine==null)
        {
            //only makes new target if old is null or they are attacking a base
            //makes sure we don't double up on attack routines

            //helps be more natural
            pos = RandomizePos(pos);
            float baseBonusRange = 0;

            if(enmTarg.GetType()==typeof(BaseHP))//checks if refrence is base
            {
                baseBonusRange = 4;//units couldn't get to the center of the transform because of the hitbox
                //and setting visually would make sense they can punch the outside of the base
            }

            float atkRng = myStats.getAttackRange();
            Vector2 curruntPos = this.gameObject.transform.position;
            if (myStats.AmRanged())
            {
                RangedTarget = pos;
                attackTarget = enmTarg;
                setUnitState(UnitState.windUp);
                currentRoutine = StartCoroutine(RangedAttackRoutine());
            }
            //are they in attack range
            else if (atkRng+ baseBonusRange >= Mathf.Abs(curruntPos.x - pos.x) && atkRng+ baseBonusRange >= Mathf.Abs(curruntPos.y - pos.y))
            {
                Debug.Log("set target to" + enmTarg.name);
                attackTarget = enmTarg;
                currentRoutine = StartCoroutine(MeleeAttackRoutine());
            }
        }
        SetMoveTarget(pos);
    }

    private Vector3 RandomizePos(Vector3 aroundHere)
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

    private void setUnitState(UnitState state)
    {
        unitState = state;
        anim.SetAnimationState((int)unitState);
    }

    
    public void UpdateSpeed(Terrain ter)// maybe sub this to event on UnitTrainFeed, should be terrain oops
    {
        currentSpeed = myStats.getMoveSpeed(ter);//would be cool if animation ran slowerWhile on mountain or faster while on water
        //also need to increase sight size based on being on mountains
        if(ter==Terrain.mountain)
        {
            //will try this in a bit
            //I realize that if its only on or off, then players can just step bareley on it and get the same buff
            //as a player who walked to the center, maybe based off terrains center point? eh work with this for now
        }
    }
    

    private void DoneFighting()
    {
        GotToWhereIshouldHave = true;
        moveTarget = urManager.GetmoveTarget(gameObject.layer).position;
        this.setUnitState(UnitState.move);
    }

    private IEnumerator MeleeAttackRoutine()
    {
        GotToWhereIshouldHave = false;
        this.setUnitState(UnitState.windUp);
        yield return new WaitForSeconds(myStats.getAttackSpeed());
        if(attackTarget !=null)
        {
            this.setUnitState(UnitState.strike);
            yield return new WaitForSeconds(myStats.getAttackWaitTime());//for testing will need an animation
            if (attackTarget != null)
            {
                attackTarget.ThisAttackedYou(myStats); /*Debug.Log("attack target not null");*/
                yield return new WaitForSeconds(0.5f);//could be recovery time
            }
            else
            {
                //make a circle collider around you based on your range & see if you can find an enemy in it
                var newTarget = checkForOpponentToAttack();
                if(newTarget!=null)
                {
                    attackTarget = newTarget;
                    attackTarget.ThisAttackedYou(myStats); /*Debug.Log("attack target not null");*/
                    yield return new WaitForSeconds(0.5f);//could be recovery time
                }// if it can't find anyone it should move on
            }
        }
        
        yield return null;

        //change movetarget
        if(attackTarget==null)
        {
            DoneFighting();
        }
        else
        {
            yield return MeleeAttackRoutine();
        }
        currentRoutine = null;
    }

    private IEnumerator RangedAttackRoutine()
    {
        moveTarget = this.gameObject.transform.position;
        //could move 1st line of code from ShootShot() here if we want it maybe to miss?
        
        while (attackTarget != null)
        {
            yield return new WaitForSeconds(myStats.getAttackSpeed());
            if(attackTarget!=null)
            {
                ShootShot();
            }
            yield return null;
        }
        DoneFighting();
        currentRoutine = null;
    }

    private void ShootShot()
    {
        //below line makes missing harder, but idk if we need it every call
        if (attackTarget != null)
        { 
            RangedTarget = attackTarget.gameObject.transform.position; 
        }
        //then instantiate projectile, give it target, then wait. or maybe wait 1st? well am trying 1st
        var shot = Instantiate(
            ProjectilePrefab, 
            shootPoint.transform.position, 
            shootPoint.transform.rotation
            );
        shot.GetComponent<Projectile>().SetTarget(RangedTarget, this.gameObject);
    }

    private HP checkForOpponentToAttack()
    {
        int layerToAttack = -1;
        if(this.gameObject.layer == 7)
        {
            layerToAttack = 6;//player attack enemy
        }
        else if(this.gameObject.layer == 6)
        {
            layerToAttack = 7;//enemy attack player
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, myStats.getAttackRange());
        foreach (Collider2D col in hits)
        {
            if (col.gameObject.layer == layerToAttack)//based on what layer we are
            {
                HP target = col.gameObject.GetComponent<HP>();
                if (target != null)//this means it is a player unit
                {
                    return target;
                }
            }
        }
        return null;//incase there isn't one near by
    }
}
public enum UnitState 
{ 
    idle,
    move,
    windUp,
    strike
}