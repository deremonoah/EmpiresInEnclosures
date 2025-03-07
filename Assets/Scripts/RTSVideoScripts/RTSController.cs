using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class RTSController : MonoBehaviour
{
    [SerializeField] private Transform selectionAreaTransform;
    private Vector3 startPosition;
    private List<UnitAI> selectedUnitRTSList;

    private void Awake()
    {
        selectedUnitRTSList = new List<UnitAI>();
        selectionAreaTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Left Mouse Button Pressed
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition =UtilsClass.GetMouseWorldPosition();
        }

        if(Input.GetMouseButton(0))
        {
            //left mouse Button Held down
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y));
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y));
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if(Input.GetMouseButtonUp(0))
        {
            //Left Mouse Button Released
            selectionAreaTransform.gameObject.SetActive(false);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());

            selectedUnitRTSList.Clear();

            // Select Units within Selection Area
            foreach(Collider2D collider2D in collider2DArray)
            {
                UnitAI unitRTS = collider2D.GetComponent<UnitAI>();
                if(unitRTS != null)
                {
                    if (unitRTS.gameObject.layer == 7)
                    {
                        unitRTS.SetSelectedVisible(true);
                        selectedUnitRTSList.Add(unitRTS);
                    }
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            //Right mouse down
            Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();

            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, 1f, 5);
            /*=new List<Vector3>
             * { old list code
                moveToPosition + new Vector3(0,0),
                moveToPosition + new Vector3(1,0),
                moveToPosition + new Vector3(2,0),
                moveToPosition + new Vector3(3,0),
            };*/

            int targetPositionListIndex = 0;

            foreach (UnitAI unitRTS in selectedUnitRTSList)
            {
                unitRTS.SetMoveTarget(targetPositionList[targetPositionListIndex]);
                unitRTS.SetSelectedVisible(false);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition,float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for(int lcv =0;lcv<ringDistanceArray.Length;lcv++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[lcv], ringPositionCountArray[lcv]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int lcv = 0;lcv<positionCount;lcv++)
        {
            float angle = lcv * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }
}
