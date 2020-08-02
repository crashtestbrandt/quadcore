using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Functions;
using System.Threading;
using System.Threading.Tasks;
using System;

public class NetworkController : MonoBehaviour
{
    public static FirebaseUser user;
    public static FirebaseFunctions functions;

    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log("Checking Firebase dependencies ...");
        await FirebaseApp.CheckAndFixDependenciesAsync();

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Debug.Log("Signing in anonymously ...");
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        await auth.SignInAnonymouslyAsync();
        /*.ContinueWith(async task =>
        {
            await Task.Delay(TimeSpan.FromSeconds(5.0f));
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            
            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName,
                user.UserId
                );
        });
        */
        await Task.Delay(TimeSpan.FromSeconds(5.0f));
        Debug.Log("Attempting to call cloud function ...");
        functions = FirebaseFunctions.DefaultInstance;

        var data = new Dictionary<string, object>();
        //data["text"] = "This is a fucking test.";
        data["firstNumber"] = 5;
        data["secondNumber"] = 10;
        //data["push"] = true;

        //var function = functions.GetHttpsCallable("addMessage");
        var function = functions.GetHttpsCallable("addNumbers");
        dynamic result = await function.CallAsync(data);
        //var keys = result.Data.Keys;
        Debug.Log("Operation result: " + result.Data["operationResult"]);

        data = new Dictionary<string, object>();
        data["text"] = "This is a test message while authenticated!";
        function = functions.GetHttpsCallable("addMessage");
        result = await function.CallAsync(data);
        Debug.Log("Write result: " + result.Data["writeResult"]);
        Debug.Log("Original: " + result.Data["original"]);

    }
/*
    private Task<string> addMessage(string text)
    {
        var data = new Dictionary<string, object>();
        data["text"] = text;
        data["push"] = true;

        var function = functions.GetHttpsCallable("addMessage");
        return function.CallAsync(data).ContinueWith((task) =>
        {
            return (string) task.Result.Data;
        }
        );
    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }
}
