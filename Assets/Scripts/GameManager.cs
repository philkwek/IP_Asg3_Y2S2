using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject objectInGaze;

    [Header("Quick Action Buttons")]
    public GameObject buttonControlMenu;
    public TextMeshPro objectName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (objectInGaze != null)
        {
            buttonControlMenu.SetActive(true);
            Debug.Log($"{objectInGaze.name} is in gaze and tracking. Over.");
            objectName.text = objectInGaze.name;
        }
    }

    public void Rotate()
    {
        if (objectInGaze != null)
        {
            objectInGaze.transform.Rotate(0, 90, 0);
        }
    }
}
