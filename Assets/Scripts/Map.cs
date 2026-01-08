using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //this is like a map label, which holds the faction's level
    [SerializeField] Faction myFaction;
    //base holder?
    [Header("Base variables")]
    [SerializeField] GameObject BasePrefab;
    [SerializeField] Transform placeBaseHere;//later can be randomly generated position

    [Header("Tower variables")]
    [SerializeField] GameObject TowerPrefab;
    [SerializeField] Transform placeTowerHere;
    

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
    }

    private void SpawnEnemyBase()
    {
        Debug.Log("we are in spawn enemy base");
        var Base = Instantiate(BasePrefab, placeBaseHere.position, placeBaseHere.rotation);
        Base.layer = 6;
        BaseHP baseHp = Base.GetComponent<BaseHP>();
        FindObjectOfType<EnemyBaseAI>().UpdateBaseHP(baseHp);
        FindObjectOfType<UnitManager>().UpdateBasePos(placeBaseHere);
        //child it to the base "folder" easier to find in heirachy, maybe only editor code
        var Tower = Instantiate(TowerPrefab, placeTowerHere.position, TowerPrefab.transform.rotation);
        Tower.layer = 6;
    }
}
