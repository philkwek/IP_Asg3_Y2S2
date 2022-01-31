using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAccount()
    {
        firebaseManager.CreateAccount(createEmailInput.text, createUsernameInput.text, createPasswordInput.text);
    }

    public void SignInAccount()
    {
        firebaseManager.SignInAccount(emailInput.text, passwordInput.text);
    }
}
