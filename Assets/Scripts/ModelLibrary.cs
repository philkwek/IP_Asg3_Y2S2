using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;


public class ModelLibrary : MonoBehaviour
{
    public GameObject[] toDisableMenu;
    public BaseObjectCollection furnitureCollectScript;

    private void Start()
    {
        for(int i = 0; i < toDisableMenu.Length; i++)
        {
            toDisableMenu[i].SetActive(false);
        }
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeInHierarchy);
    }

    /// <summary>
    /// Used to trigger script activation on button pressed
    /// </summary>
    /// <param name="modelLibObject">This is to refence the object that holds these script</param>
    public void TriggerScriptForMainLibrary(GameObject modelLibObject)
    {
        //Ensures you are able to get reference
        if(modelLibObject != null)
        {
            //Finds script reference
            RadialView radialScript = modelLibObject.GetComponent<RadialView>();

            BoundsControl boundscript = modelLibObject.GetComponent<BoundsControl>();
            NearInteractionGrabbable grabScript = modelLibObject.GetComponent<NearInteractionGrabbable>();
            ObjectManipulator manipulateScript = modelLibObject.GetComponent<ObjectManipulator>();

            //If the radial script is enable then disable the ability to grab and scale. 
            boundscript.enabled = !radialScript.enabled;
            grabScript.enabled = !radialScript.enabled;
            manipulateScript.enabled = !radialScript.enabled;
        } else
        {
            Debug.Log("<color=red>FIX THE REFERENCE TO LIBRARY</color>");
        }
    }
}
