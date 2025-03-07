using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public pickUp food;
    public float strength;
    public float TimeToPickUp;
    [SerializeField] private float playerTimer;
    [SerializeField] private float enemyTimer;
    private bool playerHere, enemyHere;
    private UnitManager um;
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
        Debug.Log("in trigger");
        if (team == 7 && !enemyHere)
        {
            playerTimer += Time.deltaTime;
        }
        //enemies team
        else if (team == 6 && !playerHere)
        {
            enemyTimer += Time.deltaTime;
        }

        //who won the food
        if (playerTimer > TimeToPickUp)
        { PlayerGotFood(true); }
        else if (enemyTimer > TimeToPickUp)
        {
            PlayerGotFood(false);
        }
    }

    private void PlayerGotFood(bool isPlayer)
    {
        if(isPlayer)
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
}
public enum pickUp { heal, PP, buffAttack, BuffDefense}