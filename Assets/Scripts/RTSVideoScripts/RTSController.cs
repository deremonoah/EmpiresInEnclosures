using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Linq;

public class RTSController : MonoBehaviour
{
    [SerializeField] private Transform selectionAreaTransform;
    private Vector3 startPosition;
    private List<UnitAI> selectedUnitRTSList;
    [Header("Flag stuff")]
    [SerializeField]
    private GameObject GreenFlagPrefab;
    [SerializeField]
    private GameObject RedFlagPrefab;
    [SerializeField]
    private List<GameObject> flagsPlaced=new List<GameObject>();
    //we need a refrence to the flags & if one of the commanded units gets into battle we should remove the flags

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
            startPosition =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(0))
        {
            //left mouse Button Held down
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            /*Debug.Log("mouse pos "+Input.mousePosition);
            Vector3 lowerLeft = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y));
            Vector3 upperRight = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y));
            Debug.Log("lowerLeft " + upperRight);
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;//back to world to screen point?*/

            //calculate center
            Vector3 center = (startPosition + currentMousePosition) / 2;
            center = new Vector3(center.x, center.y, 0f);
            selectionAreaTransform.position = center;

            Vector3 size = new Vector3(
                Mathf.Abs(startPosition.x - currentMousePosition.x),
                Mathf.Abs(startPosition.y - currentMousePosition.y),
                1f
                );
            selectionAreaTransform.localScale = size;
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

            if(selectedUnitRTSList.Count>0)//if you command units then place flag
            { 
                PlaceFlag(moveToPosition, selectedUnitRTSList.Count); 
            }

            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, .7f, 8);

            int targetPositionListIndex = 0;

            foreach (UnitAI unitRTS in selectedUnitRTSList)
            {
                if(unitRTS!=null)//as you can try to command dead units, but the list still has a refrence
                {
                    unitRTS.CommandUnitsMoveTarget(targetPositionList[targetPositionListIndex]);
                    unitRTS.SetSelectedVisible(false);
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }
            }
        }
        //flag deal with here, for units forgetting, or now thinking how unit manager needs a list of spawned units for each player
        //which to do I should send its position to unit manager, then have it look through spawned units to remove it from places to go
        //with a function like don't go there, but can player cancel flags? button on it for cancel? right click?
    }
    #region Flag Stuff
    private void PlaceFlag(Vector3 pos, int unitCount)
    {
        //show where units are headed, might use red flag to show where they will stop, would depend on other stuff
        var flag=Instantiate(GreenFlagPrefab, pos,this.transform.rotation);//this rotation for just default
        flagsPlaced.Add(flag);//when does it get removed? should check if its null
        var flagy = flag.GetComponent<RTSFlag>();
        flagy.NumberToldToGo(unitCount);
    }

    public void ForgotFlag(List<Vector2> poses)//called by units post combat as their flags are removed
    {
        
        List<GameObject> flagsToForget = new List<GameObject>();
        
        for (int fcv=flagsPlaced.Count-1;fcv>=0; fcv--)
        {
            GameObject currentFlag = flagsPlaced[fcv];

            if(currentFlag==null)
            {
                flagsPlaced.RemoveAt(fcv);
                continue;
            }

            Vector2 currentFlagPos = currentFlag.transform.position;
            bool flagToDestroy = false;

            for (int pcv=0;pcv<poses.Count;pcv++)
            {
                if(doesFLagPosMatch(currentFlagPos.x, currentFlagPos.y, poses[pcv]))
                {
                    flagToDestroy = true;
                    break;
                }
            }
            if(flagToDestroy)
            {
                flagsPlaced.RemoveAt(fcv);
                Destroy(currentFlag);
            }
        }

        /*flagsToForget.Distinct();//on the off chance of repeats
        for(int lcv=flagsToForget.Count-1;lcv>=0;lcv--)
        {
            if(flagsToForget[lcv]!=null)
            {
                GameObject bye = flagsPlaced[lcv];
                flagsToForget.RemoveAt(lcv);
                Destroy(bye);
            }
        }*/
        //trying destroying after the loop
    }

    private bool doesFLagPosMatch(float flagx, float flagy, Vector2 pos)
    {
        float posx = pos.x;
        float posy = pos.y;

        if(Mathf.Abs(flagx-posx)<1f && Mathf.Abs(flagy - posy)<1f)//the reason for this is the randomization on commanding units position, universal randomizer I think I called it
        {
            return true;
        }
        return false;
    }
    #endregion

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
