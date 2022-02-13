using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Input;


public class SpawnObject : MonoBehaviour
{
    Manager managerScript;

    private void Start()
    {
        managerScript = FindObjectOfType<Manager>();
    }

    public void Spawn(GameObject objectToSpawn)
    {
        GameObject spawnedObject = Instantiate(objectToSpawn, new Vector3(0,0,0.5f), Quaternion.identity);
        spawnedObject.AddComponent<BoxCollider>();
        spawnedObject.AddComponent<NearInteractionGrabbable>();
        spawnedObject.AddComponent<ObjectMaterials>();

        //Setting ObjectManipulatorr settings
        spawnedObject.AddComponent<ObjectManipulator>();
        //Edit ManipulationData IF I CAN FREAKING FIND THE DOCUMANTATION THAT TELL YOU SOMETHING OMG

        //Setting SolverHandler
        spawnedObject.AddComponent<SolverHandler>();
        //spawnedObject.GetComponent<SolverHandler>().TrackedTargetType = Microsoft.MixedReality.Toolkit.Utilities.TrackedObjectType.ControllerRay;
        //spawnedObject.GetComponent<SolverHandler>().TrackedHandness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right;

        //Setting the name of object
        spawnedObject.AddComponent<InteractingObjects>();
        spawnedObject.GetComponent<InteractingObjects>().nameThisObject = objectToSpawn.name;


        //Setting configuration for boundsControl
        spawnedObject.AddComponent<BoundsControl>();
        spawnedObject.GetComponent<BoundsControl>().RotationHandlesConfig = managerScript.rotationConfig;
        spawnedObject.GetComponent<BoundsControl>().ScaleHandlesConfig = managerScript.scaleConfig;

        //Setting TapToPlace settings
        spawnedObject.AddComponent<TapToPlace>();
        spawnedObject.GetComponent<TapToPlace>().KeepOrientationVertical = true;
        spawnedObject.GetComponent<TapToPlace>().RotateAccordingToSurface = false;

        //Disable the default offset as it is not accurate
        spawnedObject.GetComponent<TapToPlace>().UseDefaultSurfaceNormalOffset = false;
        //set it mauanally
        spawnedObject.GetComponent<TapToPlace>().SurfaceNormalOffset = spawnedObject.GetComponent<BoxCollider>().size.y/2;


        managerScript.AddList(spawnedObject);
    }
}
