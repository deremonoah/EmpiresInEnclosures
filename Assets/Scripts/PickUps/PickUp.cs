using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//realizing these names are very confusing
public class PickUp : MonoBehaviour
{
    [SerializeField] PickUpAbility _ability;
    [SerializeField] private float progTimer;
    private float _timeToPickUp;


    private bool playerHere, enemyHere;
    private UnitManager um;
    public Image ProgressBar;
    public Text progressText;

    private List<GameObject> unitsInside;
    private float refreshRate = 0.3f;
    private float refTimer;
    private List<Collider2D> results = new();
    private Collider2D _myCol;

    private void Start()
    {
        um = FindObjectOfType<UnitManager>();
        playerHere = false;
        enemyHere = false;
        _timeToPickUp = _ability.getTimeToPickUP();
        progTimer = _timeToPickUp / 2;
        refTimer = refreshRate;
        _myCol = GetComponent<Collider2D>();
        DisplayProgress();
        //get time from ability
    }

    

    private void FixedUpdate()
    {
        if (enemyHere && !playerHere)
        {
            progTimer -= Time.deltaTime;
        }
        else if (playerHere && !enemyHere)
        {
            progTimer += Time.deltaTime;
        }

        DisplayProgress();

        //we need to calculate but calling every frame is expensiv

        refTimer -= Time.deltaTime;
        if(refTimer<0)
        {
            playerHere = false;
            enemyHere = false;

            Physics2D.OverlapCollider(_myCol, new ContactFilter2D().NoFilter(), results);
            foreach(var target in results)
            {

                FigureTeam(target.gameObject);
            }
            refTimer = refreshRate;
        }

        //when someone wins
        if (Mathf.Abs(progTimer) >= _timeToPickUp)
        {
            PickedUp();//only one in there should be the only team
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        var guy = collision.gameObject;
        RemoveMeFromList(guy);
    }

    //public so when the reciever dies they remove themselves from the list
    public void RemoveMeFromList(GameObject guy)
    {
        if (unitsInside.Contains(guy))
        {
            unitsInside.Remove(guy);
        }
    }*/

    /*private void OnTriggerStay2D(Collider2D other)
    {
        int team = other.gameObject.layer;
        //player's team
        if (team == 7)
        {
            playerHere = true;
        }
        else{ playerHere = false; }
        //enemies team
        if (team == 6)
        {
            enemyHere = true;
        }
        else { enemyHere = false; }


        
        
        //when the prog timer is = to TimetoPickUp positive(player) or negative(enemy) 
        

        DisplayProgress();
    }*/

    private void FigureTeam(GameObject guy)
    {
        if(guy.layer==7)
        {
            playerHere = true;
        }
        else if(guy.layer==6)
        {
            enemyHere = true;
        }
        
    }

    private void PickedUp()
    {
        if(enemyHere)
        {
            _ability.ActivatePickUp(transform, 6);
        }
        else if(playerHere)
        {
            _ability.ActivatePickUp(transform, 7);
        }
        Destroy(this.gameObject);
    }

    private void DisplayProgress()
    {
        //displays a bar that once is filled with your team's color you get the thing, does it start in the middle?
        //would text be clearer?
        //2 timers for different colors?
        //progressText.text = ""+Mathf.Round(progTimer);

        //starts at 0.5 and timer either gets closer to 1 for player or closer to zero
        ProgressBar.fillAmount = progTimer  / _timeToPickUp;
    }
}
public enum pickUp { heal, PP, buffAttack, BuffDefense, healBase, SummonUnit }
