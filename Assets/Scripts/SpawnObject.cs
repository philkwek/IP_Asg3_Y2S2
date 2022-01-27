using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnObject : MonoBehaviour
{
    public void Spawn(GameObject objectToSpawn)
    {
        Vector3 buttonT = this.transform.position;
        var spawnedOject = Instantiate(objectToSpawn, this.transform);
    }
}
