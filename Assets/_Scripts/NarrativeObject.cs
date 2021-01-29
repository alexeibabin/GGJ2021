using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeObject : MonoBehaviour
{
    [SerializeField] private NarrativeObject[] _nextSteps;
    [SerializeField] private Vector3 _offsetFromPlayer;

    [SerializeField] private float _timeToShowNewItem = 2.5f;

    //  This initializes the item once the previous one was picked up
    public void Init() 
    {
        PlaceInWorld();
    }

    private void PlaceInWorld()
    {
        transform.position = Game.i.player.transform.position + _offsetFromPlayer;
    }

    //  This is called when the item is picked up
    public void OnPickUp() {
        StartCoroutine(ShowItem());
    }

    private IEnumerator ShowItem()
    {
        //  Show the item

        yield return new WaitForSeconds(_timeToShowNewItem);

        OnStepEnd();
    }

    //  This is called one the step ends and it's in charge of setting up the next items
    public void OnStepEnd() {

        foreach (var step in _nextSteps)
        {
            var newStep = Instantiate(step, transform.parent);
            newStep.Init();
        }
    }


}
