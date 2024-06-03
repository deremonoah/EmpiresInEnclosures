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
    //their strategies may vary from group to group
    void Start()
    {
        um = FindObjectOfType<UnitManager>();
        StartCoroutine(GiraffeStrat());
    }

    private void Update()
    {
        if(SustainedSpawns&&susTimer<=0)
        {
            int rand = Random.Range(0, 3);
            um.spawnEnemyUnit(rand);
            susTimer = SustainedSpawnTimerMax;
        }else if(SustainedSpawns)
        {
            susTimer -= Time.deltaTime;
        }
    }

    public IEnumerator GiraffeStrat()
    {
        yield return new WaitForSeconds(0.1f);
        um.spawnEnemyUnit(1,commandPos[0],true);
        um.spawnEnemyUnit(1, commandPos[1], true);
        um.spawnEnemyUnit(2, commandPos[2], true);
        
        //instantiate 2 giraffes send them to their spots then spawn a giraffe every 5 seconds after 20 seconds
        //when base at half hp summon giraffe stack
        yield return new WaitForSeconds(17f);
        susTimer = SustainedSpawnTimerMax;
        um.spawnEnemyUnit(0, commandPos[3], true);
        um.spawnEnemyUnit(0, commandPos[4], true);
        SustainedSpawns = true;
        
    }
}
