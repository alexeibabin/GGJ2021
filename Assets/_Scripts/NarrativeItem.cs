using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeItem : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private string _narrativeText;
    [SerializeField] private float _timeUntilStepEnd = 5f;
    [SerializeField] private float _timeUntilItemFades = 2f;

    [Header("Next steps")]
    [SerializeField] private NarrativeItem[] _nextSteps;



    //  This initializes the item once the previous one was picked up
    public void Init() 
    {
    }

    //  This is called when the item is picked up
    public void OnPickUp() {

        Debug.LogFormat("Picked up {0} Item!", name);
        StartCoroutine(ShowItem());
    }

    private IEnumerator ShowItem()
    {
        //  Show the item
        Game.i.itemManager.ShowItem(_itemSprite, _narrativeText);

        yield return new WaitForSeconds(_timeUntilStepEnd);

        Game.i.itemManager.ClearFromView();

        EndStep();
    }

    //  This is called one the step ends and it's in charge of setting up the next items
    public void EndStep() {

        Game.i.itemManager.HideItem();

        foreach (var step in _nextSteps)
        {
            //var newStep = Instantiate(step, transform.parent);
            //newStep.Init();
            step.gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position, 1);
    }
}
