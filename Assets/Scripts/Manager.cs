using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Tools;

public class Manager : MonoBehaviour
{
    //The tmp that says what name of the object user last gazed at
    public TextMeshPro objectName;

    //A list of object currently in scene
    public List<GameObject> objectInScene = new List<GameObject>();
    [HideInInspector]
    //The object you are looking at
    public GameObject objectInReference;
    public GameObject objectButtons;

    [Header("Object Interection Buttons")]
    public GameObject animationPlayButton;
    public BaseObjectCollection collectionScript;

    [Header("Materials")]
    public Material[] dysonFan;


    [Header("Configuation")]
    public RotationHandlesConfiguration rotationConfig;
    public ScaleHandlesConfiguration scaleConfig;

    private void Start()
    {
        //Set text to empty
        objectName.text = "";
    }

    private void Update()
    {
        if(objectInReference != null)
        {
            objectButtons.SetActive(true);
        } else
        {
            objectButtons.SetActive(false);
        }
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

    public void DestroyAllObject()
    {
        for (int i = 0; i < objectInScene.Count; i++)
        {
            Destroy(objectInScene[i]);
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

    public void MoveObejct()
    {
        //Debug.Log("Moving " + objectInReference.name + " is now free to move");
        TapToPlace tapScript = objectInReference.GetComponent<TapToPlace>();
        if(tapScript != null)
        {
            tapScript.enabled = !tapScript.enabled;
            //tapScript.StartPlacement();
        } else
        {
            Debug.Log($"{objectInReference.name} has no magnatism attached");
        }
    }

    public void CycleMaterial()
    {
        ObjectMaterials objectMatScript = objectInReference.GetComponent<ObjectMaterials>();
        if(objectMatScript.materials != null)
        {
            int currengtPos = objectMatScript.materialPos;
            int materialArrayLength = objectMatScript.materials.Length;

            currengtPos += 1;

            if (currengtPos >= materialArrayLength)
            {
                currengtPos = 0;
            }

            Debug.Log("cycling through material");
            objectInReference.GetComponent<MeshRenderer>().material = objectMatScript.materials[currengtPos];
            objectMatScript.materialPos = currengtPos;
        } else
        {
            Debug.Log("This object does not havbe any other material to cycle through");
        }
    }

    public void PlayAnimation()
    {
        Animator objectAnimator = objectInReference.GetComponent<Animator>();
        if(objectAnimator != null)
        {
            objectAnimator.SetTrigger("Change");
        }
    }

    public void UpdateCollection()
    {
        collectionScript.UpdateCollection();
    }
}
