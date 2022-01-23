using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;


public class FirebaseTest : MonoBehaviour
{
    //test variables
    public string username = "test";
    public int userScore;

    public string email = "test@test.com";
    public string password = "testing123";

    //account data
    public string AuthKey = "AIzaSyB4iLqQhetQzvpCJIhczEZrRlF3daOsXJI"; //api key for firebase project 

    public string idToken;
    public static string localId; //this is the unique account authId

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        PostToDatabase();
        CreateAccount(email, username, password);
    }

    //example function for posting data to database
    private void PostToDatabase()
    {
        string link = "https://ip-asg3-y2s2-default-rtdb.firebaseio.com/" + username + ".json";
        RestClient.Put(link, new User("test", 5));
    }

    //example function for creating account with firebase
    public void CreateAccount(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then( //then gets the response from Firebase on post entry
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
            }
       ).Catch(error =>
       {
           Debug.Log(error);
       });
    }

    //example function for signing in user
    public void SignInAccount(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;

            }).Catch(error =>
            {
                Debug.Log(error);
            });
    }

}
