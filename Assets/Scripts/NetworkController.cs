using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Functions;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading;
using System.Threading.Tasks;
using System;

public class NetworkController : Singleton<NetworkController>
{
    public static FirebaseAuth Auth { get; private set; }
    public static FirebaseUser User { get; private set; }
    public static FirebaseFunctions NetworkFunctions { get; private set; }
    public static DatabaseReference DB { get; private set; }
    public static string MatchID { get; set; }
    public static string UserID { get; set; }


    // Start is called before the first frame update
    async void Start()
    {
        NetworkController.MatchID = Convert.ToBase64String(
                Guid.NewGuid()
                .ToByteArray()
            )
            .Replace("/","-")
            .Replace("+","_")
            .Replace("=","");
        
        Debug.Log("Checking Firebase dependencies ...");
        await FirebaseApp.CheckAndFixDependenciesAsync();

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Debug.Log("Signing in anonymously ...");
        Auth = FirebaseAuth.DefaultInstance;
        User = await Auth.SignInAnonymouslyAsync();

        // Without this delay, cloud function gets called before dependencies are in place
        await Task.Delay(TimeSpan.FromSeconds(5.0f));
        //NetworkFunctions = FirebaseFunctions.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://quadcore-594a5.firebaseio.com/");
        DB = FirebaseDatabase.DefaultInstance.RootReference;
        AddUser("TestUser");
        

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

    static public async Task CreateNewMatchAsync(string matchID = null)
    {
        if (matchID == null)
        {
            matchID = MatchID;
        }
        Debug.Log("Creating a new QUADCORE match in firebase with ID: " + matchID);

        var function = NetworkFunctions.GetHttpsCallable("addMatch");
        var data = new Dictionary<string, object>();
        data["match_id"] = matchID;
        /*
        data["players"] = new string[]
        {
            "TestPlayer",
            null
        };
        */
        dynamic result = await function.CallAsync(data);
        Debug.Log("Write result: " + result?.Data["result"]);
    }

    private void AddUser(string name) {

        User user = new User(NetworkController.User.UserId, name);
        string json = JsonUtility.ToJson(user);

        DB.Child("users").Child(NetworkController.User.UserId).SetRawJsonValueAsync(json);
        Debug.Log("Added user: " + name);
    }

}

public class User
{
    string uid;
    string name;
    public User() {
    }

    public User(string id, string name) {
        this.uid = id;
        this.name = name;
    }
}
