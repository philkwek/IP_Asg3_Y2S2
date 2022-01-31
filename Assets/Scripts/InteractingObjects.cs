﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class InteractingObjects : MonoBehaviour, IMixedRealityFocusHandler
{
    Manager managerScript;
    public string nameThisObject;

    private void Start()
    {
        managerScript = FindObjectOfType<Manager>();
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        Debug.Log($"{this.gameObject.name} has entered gaze");
        if (managerScript != null)
        {
            managerScript.objectInReference = this.gameObject;
            managerScript.objectName.text = nameThisObject;
        }
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        Debug.Log($"{nameThisObject} has left gaze!!!");
    }
}
