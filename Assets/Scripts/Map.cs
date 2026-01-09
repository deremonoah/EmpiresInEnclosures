using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //this is like a map label, which holds the faction's level
    [SerializeField] Faction myFaction;
    //base holder
    [Header("Enemy Base variables")]
    [SerializeField] GameObject e_BasePrefab;
    [SerializeField] Transform e_placeBaseHere;//later can be randomly generated position

    [Header("Enemy Tower variables")]
    [SerializeField] GameObject e_TowerPrefab;
    [SerializeField] Transform e_placeTowerHere;
    

    public Faction GetFaction()
    {
        return myFaction;
    }

    //disables itself before its onEnableHappens?
    /*private void Awake()
    {
        this.gameObject.SetActive(false);
    }*/

    private void OnEnable()
    {
         SpawnPlayerBase();
         SpawnEnemyBase();
    }

    private void SpawnPlayerBase()
    {
        //will need to get equiped base, this also needs to handle any upgrades they have
        //set the player layer

        //based off player layer on start it should flip its x? 180
        //pBase.gameObject.name = "Player Base";
    }

    private void SpawnEnemyBase()
    {
        Debug.Log("we are in spawn enemy base");
        var eBase = Instantiate(e_BasePrefab, e_placeBaseHere.position, e_placeBaseHere.rotation);
        eBase.layer = 6;
        eBase.gameObject.name = "Enemy Base";
        BaseHP baseHp = eBase.GetComponent<BaseHP>();
        FindObjectOfType<EnemyBaseAI>().UpdateBaseHP(baseHp);
        FindObjectOfType<UnitManager>().UpdateBasePos(e_placeBaseHere);
        //child it to the base "folder" easier to find in heirachy, maybe only editor code
        var Tower = Instantiate(e_TowerPrefab, e_placeTowerHere.position, e_TowerPrefab.transform.rotation);
        Tower.layer = 6;
    }
}
