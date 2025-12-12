using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour
{
    private Animator animMap;
    private string highlightedEncounter;

    [SerializeField] RectTransform ScoutPanel;
    private Vector2 ScoutStartPos;
    private float scoutTimer;
    [SerializeField] float scoutAnimDuration;
    [SerializeField] private RectTransform canvasRect;

    private void OnEnable()
    {
        //sub to flow manager for forced pop up
        animMap = GetComponent<Animator>();
        //FlowManager.instance.MapPanelSendOpen += openMap; for some reason this is null, but not for LootPanel
        scoutTimer = 0;
        ScoutStartPos = ScoutPanel.anchoredPosition;
        
    }

    public void openMap()
    {
        animMap.SetBool("OpenMap", true);
    }

    public void closemap()
    {
        animMap.SetBool("OpenMap", false);
    }

    public bool IsPanOpen()
    {
        return animMap.GetBool("Open");
    }

    public void pickMapNode()
    {
        //could take in a string, and load the data from resoruce folder
        //displays an encounter info panel, 
    }

    public void lookAtMapNode(string encounterToLoad)
    {
        highlightedEncounter = encounterToLoad;
        Vector2 mousePos = Input.mousePosition;
        Vector2 posToMove = CalculatePosition(mousePos);
        StartCoroutine(MoveScoutPanelOutRoutine(posToMove));

        //open scout panel & load appropriate info
        //map, enemy, maybe units? or sillouetts of their units, which are color selected as black
        //fun if scout panel looks hand drawn like actual scouts did it, and a little flair for
        //which character you are or hand holding it, penguin fin

        //can use dotween to have it slide into frame
    }

    private Vector2 CalculatePosition(Vector2 mousePos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            ScoutPanel.parent as RectTransform,
            mousePos,
            null,
            out localPoint
            );
        //from local point we need to check 2 offset values on the x, just realized we don't need to change y
        //should be half the size on the x, so I can change the size of the 
        float offset =ScoutPanel.rect.width / 2;
        //now need the 2 options one adding & one subtracting offset

        Vector2 canvasSize = canvasRect.rect.size;
        float canvasWidth = canvasSize.x;
        localPoint = new Vector2(localPoint.x, ScoutStartPos.y);

        Vector2 RightPoint = new Vector2(localPoint.x+offset, localPoint.y);
        Vector3 LeftPoint = new Vector2(localPoint.x + offset, localPoint.y);

        Debug.Log("canvas width/2 = " + (canvasWidth/2));
        Debug.Log("right point+offset = " + (RightPoint.x + offset));
        if(RightPoint.x>(canvasWidth/2))
        {
            localPoint = LeftPoint;
        }
        else { localPoint = RightPoint; }

        return localPoint;
    }

    public IEnumerator MoveScoutPanelOutRoutine(Vector2 desiredPosition)
    {
        while (scoutTimer < scoutAnimDuration)
        {
            ScoutPanel.anchoredPosition = Vector2.Lerp(
                ScoutStartPos,
                desiredPosition,
                scoutTimer / scoutAnimDuration
                );
            scoutTimer += Time.deltaTime;
            yield return null;
        }

        ScoutPanel.anchoredPosition = desiredPosition;


        scoutTimer = 0;
    }
}
