using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private UIPickedItem _itemPrefab;


    private UIPickedItem _lastItemSpawned;
    public void ShowItem(Sprite s)
    {
        HideItem();

        _lastItemSpawned = Instantiate(_itemPrefab, transform);
        _lastItemSpawned.Init(s);
    }

    public void HideItem()
    {
        if(_lastItemSpawned == null)
            return;

        Debug.LogFormat("Removed {0}", _lastItemSpawned.name);
        Destroy(_lastItemSpawned.gameObject);
    }
}
