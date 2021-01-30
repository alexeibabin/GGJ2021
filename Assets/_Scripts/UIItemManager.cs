using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private UIPickedItem _itemPrefab;


    private UIPickedItem _lastItemSpawned;
    public void ShowItem(Sprite s, string text)
    {
        HideItem();

        _lastItemSpawned = Instantiate(_itemPrefab, transform);
        _lastItemSpawned.Init(s, text);
    }

    public void HideItem()
    {
        if(_lastItemSpawned == null)
            return;

        _lastItemSpawned.HideItem();
    } 

    internal void ClearFromView()
    {
        if(_lastItemSpawned == null)
            return;

        Debug.LogFormat("Removed {0}", _lastItemSpawned.name);
        Destroy(_lastItemSpawned.gameObject);
    }
}
