using UnityEngine;
using UnityEditor;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class FirebaseManager : MonoBehaviour
{

    public static FirebaseManager instance;
    public MenuManager menuManager;
    public Manager objManager;

    private string restLink = "https://ip-asg3-y2s2-default-rtdb.firebaseio.com";
    private string authKey = "AIzaSyB4iLqQhetQzvpCJIhczEZrRlF3daOsXJI"; //api key for firebase project 

    //account references
    public string idToken; //used for authenticating pushes to db
    public static string localId; //this is the unique account uid
    public string username;
    public static int userCompanyId = 0;
    public string companyName;

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

    // Start is called before the first frame update
    void Start()
    {
        GetProjectCount();

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "MRTK" && objManager == null)
        {
            GameObject obj = GameObject.Find("Manager");
            objManager = obj.GetComponent<Manager>();
        }
    }

    public void GetProfile(string userId)
    {
        RestClient.Get(restLink + "/users/" + userId + ".json?auth=" + idToken).Then(response =>
        {
            User user = JsonUtility.FromJson<User>(response.Text);
            username = user.username;
            userCompanyId = user.companyId;
            GetCompanyName(userCompanyId);
        });
    }

    public void GetCompanyName(int companyId)
    {
        RestClient.Get(restLink + "/companys/" + companyId + ".json?auth=" + idToken).Then(response =>
        {
            Company company = JsonUtility.FromJson<Company>(response.Text);
            companyName = company.companyName;
        });
    }

    public void GetProjectCount()
    {
        int count = 0;
        
        RestClient.Get(restLink + "/projects.json").Then(response =>
        {
            var allProjectData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Text);
            projectCount.text = allProjectData.Count.ToString();

        }).Catch(error =>
        {
            Debug.Log(error);
            projectCount.text = error.ToString();
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

        } 
    }

    public void SaveProject(string projectName, string houseType, string roomNumber)
    {
        string creator = "test";
        string dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
        int companyId = 0;
        string[] pictures = {imgData1, imgData2, imgData3}; //converts imagedata list into an array

        //gets list of all currently spawn objects in the scene
        GameObject[] furnitureUsedObj = objManager.objectInScene.ToArray();
        List<String> furnitureUsedList = new List<String>();

        for (int i = 0; i< furnitureUsedObj.Length; i++)
        {
            furnitureUsedList.Add(furnitureUsedObj[i].name);
        }
        string[] furnitureUsed = furnitureUsedList.ToArray(); //converts list to array for uploading of data
        
        //creates json for upload
        Project newProject = new Project(companyId, creator, dateCreated, furnitureUsed, houseType, projectName, roomNumber, pictures);
        string link = restLink + "/projects/.json?auth=" + idToken;
        RestClient.Post(link, newProject).Then(response => {
            Debug.Log(response.Text); //project id 
            GameObject menu = GameObject.Find("SaveProject");
            menu.GetComponent<MenuManager>().AlertSave();
        });
    }


    //function for creating account with firebase
    public void CreateAccount(string email, string username, string password)
    {

        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + authKey, userData).Then( //then gets the response from Firebase on post entry
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
                GetProfile(localId);
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
        Debug.Log(email + "/n" + password);
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        Debug.Log(userData);
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + authKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
                GetProfile(localId);

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
