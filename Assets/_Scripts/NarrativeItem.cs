using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeItem : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private NarrativeItem[] _nextSteps;

    [Header("References")]
    [SerializeField] private Vector3 _offsetFromPlayer;

    [SerializeField] private float _timeUntilStepEnd = 5f;

    [SerializeField] private Sprite _itemSprite;

    //  This initializes the item once the previous one was picked up
    public void Init() 
    {
        PlaceInWorld();
    }

    private void PlaceInWorld()
    {
        transform.position = Game.i.player.transform.position + _offsetFromPlayer;
        //  Needs to account for the terrain height in that new position
    }

    //  This is called when the item is picked up
    public void OnPickUp() {

        Debug.LogFormat("Picked up {0} Item!", name);
        StartCoroutine(ShowItem());
    }

    private IEnumerator ShowItem()
    {
        //  Show the item
        Game.i.itemManager.ShowItem(_itemSprite);

        yield return new WaitForSeconds(_timeUntilStepEnd);

        EndStep();
    }

    //  This is called one the step ends and it's in charge of setting up the next items
    public void EndStep() {

        Game.i.itemManager.HideItem();

        foreach (var step in _nextSteps)
        {
            var newStep = Instantiate(step, transform.parent);
            newStep.Init();
        }
    }


}
