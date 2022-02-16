using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public FirebaseManager firebaseManager;

    //Sign up Menu Inputs
    public TextMeshProUGUI createUsernameInput;
    public TextMeshProUGUI createEmailInput;
    public TextMeshProUGUI createPasswordInput;

    //Sign in Menu Inputs
    public TextMeshProUGUI emailInput;
    public TextMeshProUGUI passwordInput;

    //Save Project 
    public TextMeshProUGUI projectName;
    public TextMeshProUGUI houseType;
    public TextMeshProUGUI roomCount;
    public GameObject alertText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firebaseManager == null)
        {
            GameObject firebase = GameObject.Find("FirebaseManager");
            if (firebase != null)
            {
                firebaseManager = firebase.GetComponent<FirebaseManager>();
            }
        }
    }

    public void CreateAccount()
    {
        firebaseManager.CreateAccount(createEmailInput.text, createUsernameInput.text, createPasswordInput.text);
    }

    public void SignInAccount()
    {
        firebaseManager.SignInAccount(emailInput.text, passwordInput.text);
    }

    public void LoggedInAccount()
    {
        SceneManager.LoadScene("MRTK");
    }

    public void SaveProject()
    {   //runs when button is pressed, gets input values and inserts into firebaseManager for upload.
        firebaseManager.SaveProject(projectName.text, houseType.text, roomCount.text);
    }

    public void AlertSave()
    {
        alertText.SetActive(true);
    }

    public void StartNew()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
