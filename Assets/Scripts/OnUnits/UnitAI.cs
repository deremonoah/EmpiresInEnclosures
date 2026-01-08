using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this scrip handles the individual unit's actions: movment and attacking
public class UnitAI : MonoBehaviour
{
    private UnitStats myStats;
    [SerializeField] bool GotToWhereIshouldHave=false;
    [SerializeField] 
    private float currentSpeed;
    [SerializeField]
    private List<Vector2> moveTargets = new List<Vector2>();
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

    private RTSController rtsController;

    private Collider2D forBuff;

    private int LayerToAttack;
    private LayerMask layerMaskToAttack;
    private float myWidth;

    private void Awake()
    {
        anim = this.gameObject.GetComponentInChildren<UnitAnimator>();
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
        currentRoutine = null;
    }

    private void Start()
    {
        myStats = GetComponent<UnitStats>();
        currentSpeed = myStats.getMoveSpeed(Terrain.normal);
        setUnitState(UnitState.move);
        mysr = GetComponentInChildren<SpriteRenderer>();
        urManager = FindObjectOfType<UnitManager>();
        rtsController = FindObjectOfType<RTSController>();

        //for player facing right and both to have refrence to correct enemy layer or layer Mask
        if (this.gameObject.layer == 7)//so on the player layer, flip the transform 180 so it faces the right way to do animations
        {
            LayerToAttack = 6;
            layerMaskToAttack = LayerMask.GetMask("EnemyUnit");
        }
        else 
        { 
            LayerToAttack = 7;
            layerMaskToAttack = LayerMask.GetMask("PlayerUnit");
        }

        //for attack calculation
        myWidth=GetComponent<Collider2D>().bounds.size.x;
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

    private void FixedUpdate()
    {

        if (unitState == UnitState.move && moveTargets.Count>0)
        {
            float step = currentSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTargets[0], step);
            iCloseTo(moveTargets[0]);
        }

        
    }
    
    public void SetMoveTarget(Vector2 pos)//used for getting agro or initial spawn
    {
        if(moveTargets.Count<6)
        {
            moveTargets.Insert(0, pos);
            moveTargets.Distinct();//doesn't handle slight movement
        }
    }

    public void CommandUnitsMoveTarget(Vector2 pos)//used when commanding units
    {
        //should there be a limit in length?
        //could check what state its in, if combat then set to the front, and limit to one flag?
        moveTargets.Add(pos);
        SortMovementPriorities();
        moveTargets.Distinct();
    }

    private void SortMovementPriorities()//moves the last position to go to be the enemy base
    {
        //so that commanding units will follow the first one you click then always end with the enemy base

        if (moveTargets.Count < 2)
        { return; }

        for(int lcv=0;lcv<moveTargets.Count;lcv++)
        {
            //if its the enemy base
            if(moveTargets[lcv]==(Vector2)urManager.GetmoveTarget(gameObject.layer).position)
            {
                moveTargets.Add(moveTargets[lcv]);//adds base to end of list
                moveTargets.RemoveAt(lcv);//removes its previous instance
            }
        }
    }

    private void iCloseTo(Vector3 target)
    {
        if(moveTargets.Count<=1)
        {
            return;
        }

        float myx = this.transform.position.x;
        float myy = this.transform.position.y;

        float targetx = target.x;
        float targety = target.y;

        //if we are within 0.1f x & y of target move position we don't have to go there anymore
        if (Mathf.Abs( myx- targetx) < myStats.getAttackRange() && Mathf.Abs(myy - targety) < myStats.getAttackRange())
        {
            moveTargets.RemoveAt(0);
        }
        //current problem if we want them to wait somewhere they won't
        //because the always keep the base at the end, maybe remove it? halt button feel clunky
    }

    public void SeeTarget(Vector2 pos, HP enmTarg)
    {

        if (attackTarget == null && currentRoutine==null)
        {
            //only makes new target if old is null or they are attacking a base
            //makes sure we don't double up on attack routines

            //helps be more natural
            //pos = RandomizePos(pos);

            float atkRng = myStats.getAttackRange();
            Vector2 curruntPos = this.gameObject.transform.position;
            if (myStats.AmRanged())
            {
                RangedTarget = pos;
                attackTarget = enmTarg;
                setUnitState(UnitState.windUp);
                currentRoutine = StartCoroutine(RangedAttackRoutine());
                notGoThereNow();//tells RTS controller to get rid of the appropriate placed flags
            }
            //are they in attack range
            else //if (atkRng+ baseBonusRange >= Mathf.Abs(curruntPos.x - pos.x) && atkRng+ baseBonusRange >= Mathf.Abs(curruntPos.y - pos.y))
            {
                Debug.Log("do we ever fucking get in the melee shit?");
                if(CheckMeleeInRange(pos))
                {
                    Debug.Log("passed check melee in range");
                    attackTarget = enmTarg;
                    currentRoutine = StartCoroutine(MeleeAttackRoutine());
                    notGoThereNow();
                }
            }
            
            SetMoveTarget(pos);
        }
        
    }

    private bool CheckMeleeInRange(Vector2 pos)
    {
        Debug.Log("in check melee");
        Vector2 myPos = transform.position;
        Vector3 direction = pos-myPos;
        float reach = myStats.getAttackRange() + (myWidth / 2);//divided by 2 so its like going out from center
        RaycastHit2D hit = Physics2D.Raycast(myPos, direction, reach, layerMaskToAttack);
        StartCoroutine(drawRoutine(myPos, direction, reach));
        if (hit.collider!=null)
        {
            Debug.Log("collider null? nope");
            if (hit.collider.gameObject.layer == LayerToAttack)
            {
                Debug.Log("layer is even right so we return true");
                return true;
            }
        }
        return false;

    }

    private IEnumerator drawRoutine(Vector2 myPos, Vector3 direction, float reach)
    {
        float timer = 0;
        while(timer<100)
        {
            Debug.DrawRay(myPos, direction * reach, Color.red);
            timer += Time.deltaTime;
            yield return null;
        }
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
        notGoThereNow();
        moveTargets.Clear();
        moveTargets.Insert(0, urManager.GetmoveTarget(gameObject.layer).position);//might be a check box on screen 
        this.setUnitState(UnitState.move);
    }

    private void notGoThereNow()
    {
        rtsController.ForgotFlag(moveTargets);//if the list is cleard after will it be null?
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
        moveTargets.Insert(0,this.gameObject.transform.position);
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
        float reach= myStats.getAttackRange()+(myWidth / 2);//this should resualt in equivalent range of the raycast
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, reach);
        foreach (Collider2D col in hits)
        {
            if (col.gameObject.layer == LayerToAttack)//based on what layer we are
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