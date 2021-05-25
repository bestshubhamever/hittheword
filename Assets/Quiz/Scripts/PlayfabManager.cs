using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
//using Newtonsoft.Json;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    [Header("UI")]
    public Text messageText;
    public InputField emailInput, passwordInput;
    // Start is called before the first frame update
    void Start()
    {
       // GuestLogin();
        Debug.Log("Script attached");
    }

    public void RegisterButton() {
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess , OnError);
        }
    void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        messageText.text = "Registered and logged in";
    }
    void OnError(PlayFabError error)
    {
        messageText.text = "failure";
        Debug.Log("error logging in");
        Debug.Log(error.GenerateErrorReport());
    }
}
  /*  void GuestLogin()
    {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
    };
    PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result) {
        Debug.Log("logged in");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("error logging in");
        Debug.Log(error.GenerateErrorReport());
    } */

