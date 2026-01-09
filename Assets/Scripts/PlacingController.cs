using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingController : MonoBehaviour
{
    // this script will allow you to put blue prints or items on the ground

    //maybe a controller Manager, like the new input system
    //basically an enum if you are controlling units or if you are placing things
    //do I need that or just if an item to spawn was selected?
    [SerializeField] List<GameObject> itemsToPlace;
    //matching list of uses left? or custom item with variables
    private int heldItem=-1;
    //needs to track number of times you can place the items


    public void holdItemToPlace(int one)
    {
        heldItem = one;
        //display the image next to cursor
        //show area it can be placed in
    }

    private void placeItem()
    {
        //see where mouse is
        Vector3 placeToPlaceIt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        placeToPlaceIt = new Vector3(placeToPlaceIt.x, placeToPlaceIt.y, 0);
        Instantiate(itemsToPlace[heldItem],placeToPlaceIt, itemsToPlace[heldItem].transform.rotation);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && heldItem>-1)
        {
            placeItem();
            heldItem = -1;
        }
    }
}
