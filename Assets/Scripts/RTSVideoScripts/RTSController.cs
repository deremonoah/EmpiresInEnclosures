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
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(unitRTS);
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();
            foreach(UnitAI unitRTS in selectedUnitRTSList)
            {
                unitRTS.SetMoveTarget(moveToPosition);
            }
        }
    }
}
