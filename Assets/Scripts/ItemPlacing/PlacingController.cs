using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacingController : MonoBehaviour
{
    // this script will allow you to put blue prints or items on the ground

    //maybe a controller Manager, like the new input system
    //basically an enum if you are controlling units or if you are placing things
    //do I need that or just if an item to spawn was selected?
    
    [SerializeField] List<ItemReward> itemsToPlace=new List<ItemReward>();
    [SerializeField] private List<int> itemUses;//thinking might be abilities too
    [Header("Item Ui")]
    [SerializeField] RectTransform heldIcon;
    [SerializeField] private Vector2 iconOffSet;
    private Vector2 IconStartingPos;
    //matching list of uses left? or custom item with variables
    private int heldItem=-1;
    //needs to track number of times you can place the items

    public static PlacingController instance;

    private List<GameObject> placedItems = new List<GameObject>();

    private void Awake()
    {
        if(instance !=null & instance!=this)
        {
            Debug.LogError("we got 2 placing controllers in the scene");
            Destroy(this);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        FlowManager.instance.BattleEnd += RemoveItemsAfterBattle;
    }

    private void OnDisable()
    {
        FlowManager.instance.BattleEnd -= RemoveItemsAfterBattle;
    }

    private void Start()
    {
        IconStartingPos = heldIcon.position;

        var tempList = EquipManagerPlayer.instance.getPlayerItems();
        foreach (ItemReward go in tempList)
        {
            GainedNewItem(go);
        }
        ButtonManager.instance.UpdateItemList();
    }

    public void holdItemToPlace(int one)
    {
        if(itemUses[one]>0)
        {
            heldItem = one;
            heldIcon.gameObject.GetComponent<Image>().sprite= itemsToPlace[heldItem].getIcon();
            //show area it can be placed in, for now anywhere
        }
    }

    private void placeItemPlayer()
    {
        //see where mouse is
        Vector3 placeToPlaceIt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        placeToPlaceIt = new Vector3(placeToPlaceIt.x, placeToPlaceIt.y, 0);
        var item=Instantiate(itemsToPlace[heldItem].getPrefab(),placeToPlaceIt, itemsToPlace[heldItem].getPrefab().transform.rotation);
        //set it to player layer as only player uses this for now
        item.layer = 7;
        //item gets added to spawned list in pickUp start(), for items spawned by units

        //move Held Icon back to starting position
        heldIcon.position = IconStartingPos;
        itemUses[heldItem] -= 1;

        //they have to click on it again
        heldItem = -1;
        ButtonManager.instance.UpdateItemUses();
    }

    private void cancelItemPlace()
    {
        heldIcon.position = IconStartingPos;
        heldItem = -1;
    }

    private void placeItemEnemey()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && heldItem>-1)//does it make more sense for right click to place item?
        {
            placeItemPlayer();//sets held item to -1 in placeItem
        }
        if(Input.GetKeyDown(KeyCode.Mouse1)&&heldItem>-1)
        {
            cancelItemPlace();
        }
        if(heldItem>-1)
        {
            //move ui next to mouse
            heldIcon.position =  new Vector2(iconOffSet.x+ Input.mousePosition.x, iconOffSet.y + Input.mousePosition.y);
        }
    }

    public void GainedNewItem(ItemReward item)//sends scriptable obj?
    {
        itemsToPlace.Add(item);
        itemUses.Add(item.getUses());
        //should we hold just the item to be placed & it has the image rather than the mess above?
        ButtonManager.instance.UpdateItemList();
    }

    public int GetItemCount()
    {
        return itemsToPlace.Count;
    }

    public int GetItemsCurrentUses(int thisOne)
    {
        return itemUses[thisOne];
    }

    public Sprite GetItemsIcon(int thisOne)
    {
        return itemsToPlace[thisOne].getIcon();
    }

    public void RemoveItemsAfterBattle()
    {
        for(int lcv=0;lcv<placedItems.Count;lcv++)
        {
            if(placedItems[lcv]!=null)
            {
                Destroy(placedItems[lcv]);
            }
        }

        placedItems.Clear();//so we don't have a long list of nulls
    }

    public void IamItemPlaced(GameObject item)
    {
        placedItems.Add(item);
    }

    public void RemoveMeFromList(GameObject item)
    {
        placedItems.Remove(item);
    }
}
