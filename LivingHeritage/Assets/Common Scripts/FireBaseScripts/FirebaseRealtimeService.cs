using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseRealtimeService : MonoBehaviour
{
    public static FirebaseRealtimeService Instance { get; private set; }
    public static bool Initialized = false;

    private DatabaseReference dbRef;

    void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                Initialized = true;
                Debug.Log("Firebase initialized.");
            }
            else
            {
                Debug.LogError("Firebase dependencies could not be resolved: " + task.Result);
            }
        });
    }

    public async Task<bool> AddUser(User user)
    {
        if (!Initialized)
            return false;

        string key = dbRef.Child("users").Push().Key;

        string json = JsonUtility.ToJson(user);

        var task = dbRef.Child("users").Child(key).SetRawJsonValueAsync(json);
        await task;

        return task.IsCompletedSuccessfully;
    }

    public async Task<double?> GetBestTime()
    {
        var task = dbRef
            .Child("users")
            .OrderByChild("overAllTime")
            .LimitToFirst(1)
            .GetValueAsync();

        await task;

        foreach (var child in task.Result.Children)
        {
            return double.Parse(child.Child("overAllTime").Value.ToString());
        }

        return null;
    }

    public async Task<List<User>> GetAllUsers()
    {
        if (!Initialized)
            return null;

        var task = await dbRef
            .Child("users")
            .GetValueAsync();

        if (task == null || !task.HasChildren)
            return new List<User>();

        List<User> users = new List<User>();

        foreach (var child in task.Children)
        {
            try
            {
                string json = child.GetRawJsonValue();
                User user = JsonUtility.FromJson<User>(json);
                users.Add(user);
            }
            catch
            {
                Debug.LogWarning($"Failed to parse user: {child.Key}");
            }
        }

        return users;
    }

}
