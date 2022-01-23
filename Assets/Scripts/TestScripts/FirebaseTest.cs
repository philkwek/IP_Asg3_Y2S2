using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;


public class FirebaseTest : MonoBehaviour
{
    //test variables
    public string username;
    public int userScore;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        PostToDatabase();
    }

    public void TestUploadData()
    {
        
    }

    private void PostToDatabase()
    {
        RestClient.Post("https://ip-asg3-y2s2-default-rtdb.firebaseio.com/.json", new User("test", 5));
    }

}
