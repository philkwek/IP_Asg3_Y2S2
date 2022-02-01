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
            objectInScene.Remove(objectInReference);
            Destroy(objectInReference);
            Debug.Log($"<color=red>Destroying {objectInReference.name}</color>");

        }
        else
        {
            Debug.Log("<color=green>Nothing is in reference. Please check again</color>");
            objectName.text = "";
        }

    }

    public void Rotate()
    {
        Vector3 objectRotation = objectInReference.transform.eulerAngles;
        float yRotation;

        if(objectInReference != null)
        {
            Debug.Log("Attempting to rotate object and " + objectRotation.y);

            if(objectRotation.y >= 270)
            {
                Debug.Log("Rotating to 0");
                yRotation = 0;
            } 
            else if(objectRotation.y >= 180)
            {
                Debug.Log("Rotating to 270");
                yRotation = 270;
            } 
            else if(objectRotation.y >= 90)
            {
                Debug.Log("Rotating to 180");
                yRotation = 180;
            } 
            else
            {
                Debug.Log("Rotating to 90");
                yRotation = 90;
            }
            objectInReference.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
            Debug.Log(objectInReference.transform.eulerAngles);
        }
    }
}
