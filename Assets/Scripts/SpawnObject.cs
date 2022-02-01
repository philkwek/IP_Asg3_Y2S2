using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnObject : MonoBehaviour
{
    Manager managerScript;

    private void Start()
    {
        managerScript = FindObjectOfType<Manager>();
    }

    public void Spawn(GameObject objectToSpawn)
    {
        Vector3 objectTransfomrPos = this.transform.position;
        //Spawn object in front of button
        Vector3 buttonT = new Vector3(objectTransfomrPos.x, objectTransfomrPos.y, objectTransfomrPos.z + 0.5f);
        var spawnedOject = Instantiate(objectToSpawn, buttonT, Quaternion.identity);
        managerScript.AddList(spawnedOject);
    }
}
