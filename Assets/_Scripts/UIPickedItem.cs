using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickedItem : MonoBehaviour
{
    [SerializeField] private Image _itemImage;

    public void Init(Sprite sprite)
    {
        if(sprite != null)
            _itemImage.sprite = sprite;
        else
            Debug.LogError("Use a fucking sprite. There's no sprite!");
    }
}
