using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public TextMeshPro objectName;

    public List<GameObject> objectInScene = new List<GameObject>();
    [HideInInspector]
    public GameObject objectInReference;

    private void Start()
    {
        //Set text to empty
        objectName.text = "";
    }

    public void AddList(GameObject toAdd)
    {
        objectInScene.Add(toAdd);
        Debug.Log($"Added{toAdd.name} to the scene and list");
        for(int i = 0; i < objectInScene.Count; i++)
        {
            Debug.Log($"{objectInScene[i].name} is in the scene");
        }
    }

    public void DestroyObject()
    {
        if(objectInReference != null)
        {
            Destroy(objectInReference);
            Debug.Log($"<color=red>Destroying {objectInReference.name}</color>");

        }
        else
        {
            Debug.Log("<color=green>Nothing is in reference. Please check again</color>");
            objectName.text = "";
        }

    }
}
