using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLine : MonoBehaviour
{
    [SerializeField] Transform nodePos;
    private int count;
    [SerializeField] float ChildDirectionRange;//60 rn

    public Vector2 getNodePos()
    {
        return nodePos.position;
    }

    public void SetCount(int c)
    {
        count = c;
    }

    private void Start()
    {
        
        if (count>0)//not sure but was spawning a 0 one idk
        {
            var parentDirection = this.transform.rotation.eulerAngles;
            var mGen = MapGenerator.instance;

            float x = nodePos.position.x;
            float y = nodePos.position.y;

            float starting = parentDirection.z - ChildDirectionRange;
            float ending = parentDirection.z + ChildDirectionRange;


            for (float lcv = starting; lcv < ending; lcv += 10)
            {
                var quat = Quaternion.Euler(0, 0, lcv);

                float xCalc = x * Mathf.Cos(lcv);
                float yCalc = y * Mathf.Sin(lcv);

                Vector2 calcPos = new Vector3(xCalc, yCalc);
                //var nodePos = line.GetComponent<MapLine>().getNodePos();
                if (mGen.GetValue(calcPos) > 0.55f)
                {
                    SpawnLine(quat);
                }
            }

        }
    }

    private void SpawnLine(Quaternion quat)
    {
        //like previously we need to check positions, but we need to bias a cone of direction
        //this things rotation+or- some amount, but doesn't handle interconnected stuff

        var line = Instantiate(MapGenerator.instance.GetPrefab(), nodePos.position, quat, this.transform);
        line.GetComponent<MapLine>().SetCount(count - 1);
    }

}
