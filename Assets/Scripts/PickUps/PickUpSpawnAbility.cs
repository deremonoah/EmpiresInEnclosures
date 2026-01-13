using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnUnit", menuName = "PickUpAbilities/SpawnUnit")]
public class PickUpSpawnAbility : PickUpAbility
{
    [SerializeField] GameObject spawnPrefab;
    //could have reward on destroy

    public override void ActivatePickUp(Transform origin, int isPlayerLayer)
    {
        //for spawn these are the blue prints so if the orgin layer== isPlayerLayer
        //if the one who summoned it (set by spawning item system)
        if(origin.gameObject.layer==isPlayerLayer)
        {
            var unit = Instantiate(spawnPrefab, origin.position, origin.rotation);
            unit.layer = isPlayerLayer;
        }
        else
        {
            Debug.Log("the one who didn't spawn it destroyed it");
            //should enemy get a reward?
            //should instantiate a visual for destroying it add juice
        }
    }
}
