using UnityEngine;
using UnityEditor;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using TMPro;
using FullSerializer;
using System;
using System.IO;
using UnityEngine.UI;


public class FirebaseManager : MonoBehaviour
{ 

    private string restLink = "https://ip-asg3-y2s2-default-rtdb.firebaseio.com";
    private string AuthKey = "AIzaSyB4iLqQhetQzvpCJIhczEZrRlF3daOsXJI"; //api key for firebase project 

    //account references
    public string idToken; //used for authenticating pushes to db
    public static string localId; //this is the unique account uid
    public static int userCompanyId = 0;

    public static fsSerializer serializer = new fsSerializer();

    //Image data reference
    public string imgData1 = "";
    public string imgData2 = "";
    public string imgData3 = "";

    //Alert Reference
    public GameObject alertBox;
    public GameObject loggedInAlert;
    public GameObject signedUpAlert;

    //Debug Text Test
    public TextMeshProUGUI debugText; 

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

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
        RestClient.Get(restLink + "/users.json?auth=" + idToken).Then(response =>
        {
            fsData userData = fsJsonParser.Parse(response.Text); //converts text into json
            Dictionary<string, User> users = null;
            serializer.TryDeserialize(userData, ref users); //tries to translate json into the declared user array
            foreach (var user in users.Values)
            {
                Debug.Log(user.username);
            }
        });
    }

    public void GetUserCompanyId(string databaseId)
    {
        RestClient.Get<User>(restLink + "/users/" + databaseId + ".json?auth=" + idToken).Then(response =>
        {
            userCompanyId = response.companyId;
            Debug.Log(userCompanyId);
            SaveProject(); //for testing, remove when done
        });
    }

    public void SavePhotoData(string imgData)
    {
        debugText.text = imgData;

        if (imgData1 == "")
        {
            imgData1 = imgData;
            debugText.text = imgData;

        } else if (imgData2 == "")
        {
            imgData2 = imgData;

        } else if (imgData3 == "")
        {
            imgData3 = imgData;

        } else
        {
            SaveProject();
        }
    }

    public void SaveProject()
    {
        string creator = "test";
        string dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
        int companyId = 0;
        string[] pictures = {imgData1, imgData2, imgData3}; //converts imagedata list into an array

        //below are test values
        string[] furnitureUsed = { "chair", "table" }; //get furniture
        string houseType = "HDB"; //get housetype
        string nameOfLayout = "Photo and Data Upload Test"; //get name of layout
        int noOfBedrooms = 5; //get no of bedrooms
        
        //creates json for upload
        Project newProject = new Project(companyId, creator, dateCreated, furnitureUsed, houseType, nameOfLayout, noOfBedrooms, pictures);
        string link = restLink + "/projects/.json?auth=" + idToken;
        RestClient.Post(link, newProject).Then(response => {
            Debug.Log(response.Text); //project id 
        });
    }


    //function for creating account with firebase
    public void CreateAccount(string email, string username, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + AuthKey, userData).Then( //then gets the response from Firebase on post entry
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;

                //Create account in the RealtimeDb from inputs
                string link = restLink + "/users/" + localId + ".json?auth=" + idToken;
                User newUser = new User(email, username, localId);
                RestClient.Put(link, newUser);
                alertBox.SetActive(true);
                signedUpAlert.SetActive(true);
            }
       ).Catch(error =>
       {
           Debug.Log(error);
       });
    }

    //function for signing in user and getting their profile data
    public void SignInAccount(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
                GetUserCompanyId(localId);
                alertBox.SetActive(true);
                loggedInAlert.SetActive(true);

            }).Catch(error =>
            {
                Debug.Log(error);
            });

    }

}
