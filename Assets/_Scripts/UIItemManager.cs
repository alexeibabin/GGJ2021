using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private UIPickedItem _itemPrefab;


    public void ShowItem(Sprite s)
    {
        var item = Instantiate(_itemPrefab, transform);
        item.Init(s);
    }
}
