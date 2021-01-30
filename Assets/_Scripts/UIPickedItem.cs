using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickedItem : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMPro.TMP_Text _narrativeText;

    public void Init(Sprite sprite, string text)
    {
        if(sprite != null)
            _itemImage.sprite = sprite;
        else
            Debug.LogError("Use a fucking sprite. There's no sprite!");

            _narrativeText.text = text;
    }

    internal void HideItem()
    {
        _itemImage.gameObject.SetActive(false);
    }
}
