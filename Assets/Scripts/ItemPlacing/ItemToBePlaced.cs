using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemPlaceAble",menuName = "ItemPlaceAble")]
public class ItemToBePlaced : ScriptableObject
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] int uses;
    [SerializeField] Sprite icon;

    public GameObject getPrefab()
    {
        return itemPrefab;
    }

    public int getUses()
    {
        return uses;
    }

    public Sprite getIcon()
    {
        return icon;
    }
}
