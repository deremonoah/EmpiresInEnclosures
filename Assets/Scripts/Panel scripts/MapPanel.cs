using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour
{
    private Animator animMap;
    private NodeData highlightedNode;

    //scout panel data
    [SerializeField] RectTransform ScoutPanel;
    [SerializeField]private Vector2 ScoutStartPos;
    private float scoutTimer;
    [SerializeField] float scoutAnimDuration;
    [SerializeField] private RectTransform canvasRect;
    private MapLoader ml;
    [SerializeField]
    private List<NodeData> ConqueredNodes;//idea here is once you beat a node it gets added here

    private void OnEnable()
    {
        //sub to flow manager for forced pop up
        animMap = GetComponent<Animator>();
        //FlowManager.instance.MapPanelSendOpen += openMap; for some reason this is null, but not for LootPanel
        scoutTimer = 0;
        ScoutStartPos = ScoutPanel.localPosition;
        ml = FindObjectOfType<MapLoader>();
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
        return animMap.GetBool("OpenMap");
    }

    private void GenerateMap()
    {
        //probably using prefabs for the nodes
        //and as I build the level I need to be assigning nearby nodes
        //but how to actually place them out?
    }

    public void pickMapNode()
    {
        if(highlightedNode.getNodeType()==NodeType.enemy)
        {
            ml.loadLevel(highlightedNode.getMainFactionOnNode());//this tells the map loader to load the correct map
            //tell unit manager what the new faction is & give their unit list
            UnitManager um = FindObjectOfType<UnitManager>();
            um.LoadEnemyUnitList(highlightedNode.getUnits());
            um.SetEnemyFaction(highlightedNode.GetIncludedFactions());
        }
        if(highlightedNode.getNodeType()==NodeType.shop)
        {
            ConqueredNodes.Add(highlightedNode);
        }
    }

    public void lookAtMapNode(NodeData node)
    {
        highlightedNode = node; //technically we also should handle it being a shop, but one thing at a time
        if(CanMoveToThisNode(highlightedNode))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 posToMove = CalculatePosition(mousePos);
            StartCoroutine(MoveScoutPanelOutRoutine(posToMove));

            //open scout panel & load appropriate info
            //map, enemy, maybe units? or sillouetts of their units, which are color selected as black
            //fun if scout panel looks hand drawn like actual scouts did it, and a little flair for
            //which character you are or hand holding it, penguin fin

            //can use dotween to have it slide into frame

            //need highlighted encounter to be on there
        }
    }

    private bool CanMoveToThisNode(NodeData no)//maybe in future we just highlight all the nodes a player could go to
    {
        List<NodeData> nearbyNodes = no.GetNearbyNodes();
        //first check if we have conquered it
        for(int lcv=0;lcv<ConqueredNodes.Count;lcv++)
        {
            if(no==ConqueredNodes[lcv])
            { return false; }
        }

        //then check if it is a valid place the player can move
        for(int lcv=0;lcv<ConqueredNodes.Count;lcv++)
        {
            for(int i=0;i<nearbyNodes.Count;i++)
            {
                if(nearbyNodes[i]==ConqueredNodes[lcv])
                {
                    return true;
                }
            }
        }
        return false;
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
        float offset =ScoutPanel.rect.width / 1.4f;
        //now need the 2 options one adding & one subtracting offset

        Vector2 canvasSize = canvasRect.rect.size;
        float canvasWidth = canvasSize.x;
        localPoint = new Vector2(localPoint.x, ScoutStartPos.y);

        Vector2 RightPoint = new Vector2(localPoint.x+offset, localPoint.y);
        Vector3 LeftPoint = new Vector2(localPoint.x - offset, localPoint.y);

        if(RightPoint.x>(canvasWidth/2))
        {
            
            localPoint = LeftPoint;
        }
        else { localPoint = RightPoint; }

        return localPoint;
    }

    public void ReturnScoutPanel()
    {
        StartCoroutine(MoveScoutPanelOutRoutine(ScoutStartPos));
        scoutTimer = 0;
    }

    public IEnumerator MoveScoutPanelOutRoutine(Vector2 desiredPosition)
    {
        while (scoutTimer < scoutAnimDuration)
        {
            ScoutPanel.anchoredPosition = Vector2.Lerp(
                ScoutPanel.localPosition,
                desiredPosition,
                scoutTimer / scoutAnimDuration
                );
            scoutTimer += Time.deltaTime;
            yield return null;
        }

        ScoutPanel.localPosition = desiredPosition;

        //maybe also have a penguin image move to the node

        scoutTimer = 0;
    }

    public void AddConqueredNode(NodeData node)
    {
        ConqueredNodes.Add(node);
        //maybe we could add a color change to the ui element for the node
    }

    //called by the flow manager when the player wins a fight
    public void PlayerBeatNode() 
    {
        ConqueredNodes.Add(highlightedNode);
        //should move the penguin there or change image, maybe change display image to penguin.
    }
}
