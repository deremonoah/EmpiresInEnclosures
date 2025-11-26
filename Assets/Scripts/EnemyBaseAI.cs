using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAI : MonoBehaviour
{
    public List<Transform> commandPos;
    private UnitManager um;
    private bool SustainedSpawns;
    private float susTimer;
    public float SustainedSpawnTimerMax;
    [SerializeField] BaseHP ourBase;
    //their strategies may vary from group to group
    void Start()
    {
        um = FindObjectOfType<UnitManager>();
        //in future we will add the based on what scene or some factor it might pick a more strategic enemy
        StartCoroutine(GiraffeStrat());
    }

    private void Update()
    {
        
    }

    //this is the spam strat
    public IEnumerator GiraffeStrat()
    {
        yield return new WaitForSeconds(0.1f);
        //what do I want the ai to do? summon a guy when they have the PP to do so,
        while (ourBase.GetHP() > 0)
        {
            if (um.GetEnmPPAmount() >= um.GetEnmUnitCost(0))//could make int random, to be fair rn this if not matter, they just spam the button
            { um.spawnEnemyUnit(0); }//they don't have multiple units or anything they do
            yield return new WaitForSeconds(0.3f);
        }
    }
}
