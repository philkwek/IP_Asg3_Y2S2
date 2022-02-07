using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLibrary : MonoBehaviour
{
    public GameObject[] toDisableMenu;

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
}
