using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyUI.Toast;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class PlayFabSIgnup : MonoBehaviour
{
    [SerializeField] GameObject signupTab, loginTab;
    [SerializeField] Text  userEmail, userPassword, userAge, userCity, userEmaillogin, userPasswordlogin, errortext;
    string encryptPassword;
   
    public void SignUpTab() {
        signupTab.SetActive(true);
        loginTab.SetActive(false);
    }
    public void LogInTab() {
        loginTab.SetActive(true);
        signupTab.SetActive(false);
        
    }
    string Encrpt(string pw)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] epw = System.Text.Encoding.UTF8.GetBytes(pw);
        epw = x.ComputeHash(epw);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in epw) { s.Append(b.ToString("x2").ToLower()); }
        return s.ToString();
    }
    public void Register() {
        var registerRequest = new RegisterPlayFabUserRequest {  Email = userEmail.text,Password = Encrpt(userPassword.text), RequireBothUsernameAndEmail = false }; 
             PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnregisterRequestSuccess, OnRegisterFailure);
    }
    void OnregisterRequestSuccess(RegisterPlayFabUserResult result) { Debug.Log("signing in"); errortext.text = "signed in"; Toast.Show("signed in"); }
    void OnRegisterFailure(PlayFabError error) { Debug.Log("error signing in"); Debug.Log(error.GenerateErrorReport()); Toast.Show(error.GenerateErrorReport()); }
    public void Login() {
        var request = new LoginWithEmailAddressRequest { Email = userEmaillogin.text, Password = Encrpt(userPasswordlogin.text) };
        PlayFabClientAPI.LoginWithEmailAddress(request, loginsuccess, loginfailure);
    
    }
    public void loginsuccess(LoginResult result)
    { var emailAddress = "bestshubhamever@gmail.com"; Debug.Log("logging in"); AddOrUpdateContactEmail(emailAddress); SceneManager.LoadScene("GameScene"); }

    public void loginfailure(PlayFabError error) { Debug.Log("error logging in"); Debug.Log(error.GenerateErrorReport()); Toast.Show(error.GenerateErrorReport()); }
   
    void AddOrUpdateContactEmail( string emailAddress)
    {
        var request = new AddOrUpdateContactEmailRequest
        {
            
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            Debug.Log("The player's account has been updated with a contact email");
        }, FailureCallback);
    }
    
    void FailureCallback(PlayFabError error)
    {
        Toast.Show(error.GenerateErrorReport());
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void GuestLogin()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("logged in");
        Toast.Show("logged in");
        SceneManager.LoadScene("GameScene");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("error logging in");
        Toast.Show(error.GenerateErrorReport());
        Debug.Log(error.GenerateErrorReport());
    }
}
