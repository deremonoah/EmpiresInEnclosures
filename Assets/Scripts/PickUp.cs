using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public pickUp food;
    public float strength;
    public float TimeToPickUp;
    [SerializeField] private float progTimer;

    private bool playerHere, enemyHere;
    private UnitManager um;
    public Image ProgressBar;
    public Text progressText;

    private void Start()
    {
        um = FindObjectOfType<UnitManager>();
        playerHere = false;
        enemyHere = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        int team = other.gameObject.layer;
        //player's team
        if (team == 7 && !enemyHere)
        {
            progTimer += Time.deltaTime;
        }
        //enemies team
        else if (team == 6 && !playerHere)
        {
            progTimer -= Time.deltaTime;
        }

        //when the prog timer is = to TimetoPickUp positive(player) or negative(enemy) 
        if (Mathf.Abs(progTimer) >= TimeToPickUp)
        { PlayerGotFood(); }

        DisplayProgress();
    }

    private void PlayerGotFood()
    {
        //if player has reached the time alternative is enemy reached negative version
        if(progTimer>=TimeToPickUp)
        { 
            //player got it
            switch(food)
            {
                case pickUp.PP:
                    um.PlayerGetsPower(strength, true);
                    break;
            }

        }
        else
        {
            //enemy got it
            switch (food)
            {
                case pickUp.PP:
                    um.EnemyGetsPower(strength, true); ;
                    break;
            }
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
        ProgressBar.fillAmount = (progTimer + TimeToPickUp) / (TimeToPickUp * 2);
    }
}
public enum pickUp { heal, PP, buffAttack, BuffDefense}