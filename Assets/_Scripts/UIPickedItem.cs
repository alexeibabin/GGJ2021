using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickedItem : MonoBehaviour
{
    [SerializeField] private Image _itemImage;

    public void Init(Sprite sprite)
    {
        _itemImage.sprite = sprite;
    }
}
