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

public class NetworkController : Singleton<NetworkController>
{
    public static FirebaseUser User { get; private set; }
    public static FirebaseFunctions NetworkFunctions { get; private set; }


    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log("Checking Firebase dependencies ...");
        await FirebaseApp.CheckAndFixDependenciesAsync();

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Debug.Log("Signing in anonymously ...");
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        User = await auth.SignInAnonymouslyAsync();

        // Without this delay, cloud function gets called before dependencies are in place
        await Task.Delay(TimeSpan.FromSeconds(5.0f));
        Debug.Log("Attempting to call cloud function ...");
        NetworkFunctions = FirebaseFunctions.DefaultInstance;

        /*
        var data = new Dictionary<string, object>();
        data["firstNumber"] = 5;
        data["secondNumber"] = 10;

        var function = NetworkFunctions.GetHttpsCallable("addNumbers");
        dynamic result = await function.CallAsync(data);
        //var keys = result.Data.Keys;
        Debug.Log("Operation result: " + result.Data["operationResult"]);

        data = new Dictionary<string, object>();
        data["text"] = "This is a test message while authenticated!";
        function = NetworkFunctions.GetHttpsCallable("addMessage");
        result = await function.CallAsync(data);
        Debug.Log("Write result: " + result.Data["writeResult"]);
        Debug.Log("Original: " + result.Data["original"]);
        */

    }
}
