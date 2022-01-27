using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class InteractingObjects : MonoBehaviour, IMixedRealityFocusHandler
{
    GameManager gmScript;

    private void Start()
    {
        gmScript = FindObjectOfType<GameManager>();
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        Debug.Log($"{this.gameObject.name} has entered gaze");
        if (gmScript != null)
        {
            gmScript.objectInGaze = this.gameObject;
        }
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        Debug.Log($"{this.gameObject.name} has left gaze!!!");
    }
}
