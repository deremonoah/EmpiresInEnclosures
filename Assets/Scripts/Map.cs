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

    [Header("Player SpawnRefrences")]
    [SerializeField] Transform p_placeBaseHere;//kinda fidly to set up
    [SerializeField] Transform p_placeTowerHere;

    private EquipManagerPlayer _playerEquip;
    private UnitManager _um;

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
        _playerEquip = FindObjectOfType<EquipManagerPlayer>();//only one instance in the scene 
        _um = FindObjectOfType<UnitManager>();
        SpawnPlayerBuildings();
         SpawnEnemyBuildings();
    }

    private void SpawnPlayerBuildings()
    {
        //will need to get equiped base, this also needs to handle any upgrades they have
        //set the player layer

        var pBase=Instantiate(_playerEquip.GetPlayerBase(), p_placeBaseHere.position, p_placeBaseHere.rotation);
        pBase.gameObject.name = "Player Base";
        //realizing we need to clear up all these instantiated objects over time, added to a list on here, clears ondisable?
        //do we it gets disabled?
        pBase.layer = 7;
        _um.UpdatePlayerBasePos(p_placeBaseHere);

        //tower
        var pTow = Instantiate(_playerEquip.GetPlayerTower(), p_placeTowerHere.position, p_placeTowerHere.rotation);
        pTow.layer = 7;
    }

    private void SpawnEnemyBuildings()
    {
        Debug.Log("we are in spawn enemy base");
        var eBase = Instantiate(e_BasePrefab, e_placeBaseHere.position, e_placeBaseHere.rotation);
        eBase.layer = 6;
        eBase.gameObject.name = "Enemy Base";
        BaseHP baseHp = eBase.GetComponent<BaseHP>();
        FindObjectOfType<EnemyBaseAI>().UpdateBaseHP(baseHp);
        _um.UpdateEnemyBasePos(e_placeBaseHere);

        //child it to the base "folder" easier to find in heirachy, maybe only editor code
        var Tower = Instantiate(e_TowerPrefab, e_placeTowerHere.position, e_TowerPrefab.transform.rotation);
        Tower.layer = 6;
    }
}
