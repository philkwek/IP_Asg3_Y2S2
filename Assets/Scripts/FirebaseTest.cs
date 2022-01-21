using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class FirebaseTest : MonoBehaviour
{
    DatabaseReference databaseRef;

    private void Awake()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        databaseRef.Child("test").SetValueAsync("test!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
