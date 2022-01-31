using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using TMPro;
using FullSerializer;


public class FirebaseManager : MonoBehaviour
{

    private string restLink = "https://ip-asg3-y2s2-default-rtdb.firebaseio.com";
    private string AuthKey = "AIzaSyB4iLqQhetQzvpCJIhczEZrRlF3daOsXJI"; //api key for firebase project 

    //account data
    public string idToken;
    public static string localId; //this is the unique account authID

    public static fsSerializer serializer = new fsSerializer();

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GetDatabaseIds();
    }

    //example function for posting data to database
    private void PostToDatabase()
    {
        //string link = restLink + username + ".json";
        //RestClient.Put(link, new User("test", 5));
    }

    private void GetDatabaseIds() 
    {
        Debug.Log("Getting from Database");
        RestClient.Get(restLink + "/users.json").Then(response =>
        {
            fsData userData = fsJsonParser.Parse(response.Text); //converts text into json
            Debug.Log(userData);
            Dictionary<string, User> users = null; 
            serializer.TryDeserialize(userData, ref users); //tries to translate json into the declared user array
            foreach (var user in users.Values)
            {
                Debug.Log(user.username);
            }
        });
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

                //Create account in the RealtimeDb from inputs
                string link = restLink + "users/" + localId + ".json";
                User newUser = new User(email, username, localId);
                RestClient.Put(link, newUser);
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
