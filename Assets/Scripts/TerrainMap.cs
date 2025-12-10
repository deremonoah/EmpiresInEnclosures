using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap : MonoBehaviour
{
    //this script is for mountains, rivers on maps, maybe other stuff like snow?
    [SerializeField] Terrain myTerrain;
    //on trigger enter tell it what terrain it is in
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //on Terrain specific layer can only interact with itself
        var isUnit = collision.gameObject.GetComponent<UnitTrainFeet>();
        if (isUnit != null)
        {
            isUnit.TerrainChange(myTerrain);
        }
    }
    //on trigger exit set it back to normal? what about water next to mountain?
    private void OnTriggerExit2D(Collider2D collision)
    {
        var isUnit = collision.gameObject.GetComponent<UnitTrainFeet>();
        if (isUnit != null)
        {
            isUnit.TerrainChange(Terrain.normal);
        }
    }
}
public enum Terrain { normal, water, mountain }