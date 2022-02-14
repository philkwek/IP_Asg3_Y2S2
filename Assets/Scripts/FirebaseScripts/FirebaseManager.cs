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

    public static FirebaseManager instance;
    public MenuManager menuManager;

    private string restLink = "https://ip-asg3-y2s2-default-rtdb.firebaseio.com";
    private string AuthKey = "AIzaSyB4iLqQhetQzvpCJIhczEZrRlF3daOsXJI"; //api key for firebase project 

    //account references
    public string idToken; //used for authenticating pushes to db
    public static string localId; //this is the unique account uid
    public static int userCompanyId = 0;

    public static fsSerializer serializer = new fsSerializer();

    //Image data reference
    public string imgData1;
    public string imgData2;
    public string imgData3;

    //Alert Reference
    public GameObject alertBox_login;
    public TextMeshPro alertBox_text_login;
    public GameObject alertBox_signUp;
    public TextMeshPro alertBox_text_signUp;

    public TextMeshPro projectCount;

    private void Awake()
    {
        GetProjectCount();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
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

    public void GetProjectCount()
    {

        int count = 0;
        RestClient.Get(restLink + "/projects.json").Then(response =>
        {
            fsData projectData = fsJsonParser.Parse(response.Text);
            Dictionary<string, Project> projects = null;
            serializer.TryDeserialize(projectData, ref projects);
            foreach(var project in projects.Values)
            {
                count += 1;
            }
            projectCount.text = count.ToString();
        });
    }

    public void GetUserCompanyId(string databaseId)
    {
        RestClient.Get<User>(restLink + "/users/" + databaseId + ".json?auth=" + idToken).Then(response =>
        {
            userCompanyId = response.companyId;
        });
    }

    public void SavePhotoData(string imgData)
    {

        if (imgData1 == null)
        {
            imgData1 = imgData;

        } else if (imgData2 == null)
        {
            imgData2 = imgData;

        } else if (imgData3 == null)
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

                alertBox_signUp.SetActive(true);
                alertBox_text_signUp.text = "Signed up account successfully!";
                menuManager.LoggedInAccount();
            }
       ).Catch(error =>
       {
           Debug.Log(error);
           alertBox_signUp.SetActive(true);
           alertBox_text_signUp.text = error.ToString();
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

                alertBox_login.SetActive(true);
                alertBox_text_login.text = "Logged in successfully!";
                menuManager.LoggedInAccount();

            }).Catch(error =>
            {
                Debug.Log(error);
                alertBox_login.SetActive(true);
                alertBox_text_login.text = error.ToString();
            });
    }
}
