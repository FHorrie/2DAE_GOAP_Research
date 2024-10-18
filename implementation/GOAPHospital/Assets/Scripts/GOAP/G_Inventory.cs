using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Inventory
{
    List<GameObject> _items = new List<GameObject>();

    public void AddItem(GameObject item)
    { 
        _items.Add(item);
    }

    public GameObject FindItemWithTag(string tag)
    {
        foreach(GameObject item in _items)
        {
            if(item.tag == tag)
            {
                return item;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject item)
    {
        int removeIdx = -1;
        foreach(GameObject item2 in _items)
        {
            removeIdx++;
            if (item2 == item)
                break;
        }
        if(removeIdx >= -1)
            _items.RemoveAt(removeIdx);
    }
}
