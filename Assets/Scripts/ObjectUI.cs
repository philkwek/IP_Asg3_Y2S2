using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    public GameObject objectUI;

    public void DestroyObject()
    {
        Destroy(objectUI);
        Destroy(this);
    }
}
