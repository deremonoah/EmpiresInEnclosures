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
    
    [SerializeField] List<ItemToBePlaced> itemsToPlace;
    [SerializeField] private List<int> itemUses;//thinking might be abilities too
    [Header("Item Ui")]
    [SerializeField] RectTransform heldIcon;
    [SerializeField] private Vector2 iconOffSet;
    private Vector2 IconStartingPos;
    //matching list of uses left? or custom item with variables
    private int heldItem=-1;
    //needs to track number of times you can place the items

    public static PlacingController instance;

    private void Awake()
    {
        if(instance !=null & instance!=this)
        {
            Debug.LogError("we got 2 placing controllers in the scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        IconStartingPos = heldIcon.position;
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

    private void placeItem()
    {
        //see where mouse is
        Vector3 placeToPlaceIt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        placeToPlaceIt = new Vector3(placeToPlaceIt.x, placeToPlaceIt.y, 0);
        var item=Instantiate(itemsToPlace[heldItem].getPrefab(),placeToPlaceIt, itemsToPlace[heldItem].getPrefab().transform.rotation);
        //set it to player layer as only player uses this for now
        item.layer = 7;

        //move Held Icon back to starting position
        heldIcon.position = IconStartingPos;
        itemUses[heldItem] -= 1;

        //they have to click on it again
        heldItem = -1;
        ButtonManager.instance.UpdateItemUses();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && heldItem>-1)
        {
            placeItem();//sets held item to -1 in placeItem
        }
        if(heldItem>-1)
        {
            //move ui next to mouse
            heldIcon.position =  new Vector2(iconOffSet.x+ Input.mousePosition.x, iconOffSet.y + Input.mousePosition.y);
        }
    }

    public void GainedNewItem(ItemToBePlaced item)//sends scriptable obj?
    {
        itemsToPlace.Add(item);
        itemUses.Add(item.getUses());
        //should we hold just the item to be placed & it has the image rather than the mess above?
        ButtonManager.instance.ItemListChanged();
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
}
