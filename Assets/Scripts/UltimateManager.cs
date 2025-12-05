using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateManager : MonoBehaviour
{
    //this script manager the player or enemy ultimate being called
    //random thought if I went multiplayer, could I just send the data over for unit targets, and ultimate calls, and run them localy, this only works though if everything is perfectly deterministic
    private UnitManager um;
    [Header("Player Ultimate")]
    [SerializeField] GameObject playerUltPrefab;
    [SerializeField] float playerUltProgress;
    [SerializeField] float playerUltMax;
    [SerializeField] Image playerUltiProgressIcon;

    [Header("Enemy Ultimate")]
    [SerializeField] GameObject enemyUltPrefab;
    [SerializeField] float enemyUltProgress;
    [SerializeField] float enemyUltMax;
    [SerializeField] Image enemyUltiProgressIcon;
    //this could probably be a manager then have refrence to the enemy ult and player ult, then grab their refrences from game manager
    private void Start()
    {
        um = FindObjectOfType<UnitManager>();
    }

    private void FixedUpdate()
    {
        playerUltProgress += Time.deltaTime;//maybe I could multiply by a speed variable
        enemyUltProgress += Time.deltaTime;
        UpdateUltiUI();
    }

    //we are also going to have it handle the ui, and will want it to fill over time

    public void popUlt(bool isPlayer)
    {
        //opperating on just needing to spawn on the appropritate base, does need to facce the right direction though, hope the bases do
        if (isPlayer &&playerUltProgress>=playerUltMax)
        {
            var ult =Instantiate(playerUltPrefab, um.PlayerBasePos.position, um.PlayerBasePos.rotation);
            ult.GetComponent<ProjectileMulti>().SetTarget(um.EnemyBasePos.position,um.PlayerBasePos.gameObject);
            playerUltProgress = 0;
        }
        else if(enemyUltProgress>=enemyUltMax)
        {
            var ult = Instantiate(enemyUltPrefab, um.EnemyBasePos.position, um.EnemyBasePos.rotation);
            ult.GetComponent<ProjectileMulti>().SetTarget(um.PlayerBasePos.position, um.EnemyBasePos.gameObject);
            ult.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));//not sure how I fliped it in the past somwhere it checks a flip on x box for player units
            enemyUltProgress = 0;
        }
    }

    public void chargePlayerUlt(bool isPlayer, float amount)
    {
        if(isPlayer)
        {
            playerUltProgress += amount;
        }else
        { enemyUltProgress += amount; }
    }

    private void UpdateUltiUI()
    {
        playerUltiProgressIcon.fillAmount = playerUltProgress / playerUltMax;
        enemyUltiProgressIcon.fillAmount = enemyUltProgress / enemyUltMax;
    }
}
