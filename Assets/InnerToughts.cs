using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnerToughts : MonoBehaviour
{
    private List<IEnumerator> _enumeratorList = new List<IEnumerator>();
    private Text _text;

    private IEnumerator Start()
    {
        _enumeratorList.Add(ShowMessageText("I have never felt so alone..."));
        while (true)
        {
            if (_enumeratorList.Count > 0)
            {
                yield return _enumeratorList[0];
                _enumeratorList.RemoveAt(0);
            }

            yield return null;
        }
    }

    public void EnqueueThoughts(string thought)
    {
        _enumeratorList.Add(ShowMessageText(thought));
    }

    private IEnumerator ShowMessageText(string text)
    {
        _text.gameObject.SetActive(true);
        _text.text = text;
        yield return new WaitForSeconds(text.Length * 0.33f);
        _text.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
    }
}