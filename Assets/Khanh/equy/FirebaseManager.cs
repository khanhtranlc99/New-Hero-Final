
using System;
using System.Collections;
using System.Collections.Generic;
#if USE_FIREBASE
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Firebase;
using Firebase.Analytics;
#endif
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private const string TAG = "[FIREBASE]";
#region Instance
    private static FirebaseManager instance;
    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FirebaseManager>();
                if (!instance)
                {
                    instance = Instantiate(Resources.Load<FirebaseManager>("FirebaseManager"));
                }
            }
            return instance;
        }
    }

    public static bool Exist => instance != null;

#endregion

#region Inspector Variables
#endregion

#region Member Variables
#if USE_FIREBASE
    public FirebaseApp app;
#endif
    private bool isOk = false;
#endregion
    
#region Unity Methods

    private void Awake()
    {
        if (FindObjectsOfType<FirebaseManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
#if USE_FIREBASE
        GlobalEventManager.Instance.EvtSendEvent += SendEvent;
        GlobalEventManager.Instance.EvtUpdateUserProperties += SetUserPropeties;
#endif
        Init();
    }

    private void OnDestroy()
    {
#if USE_FIREBASE
        GlobalEventManager.Instance.EvtSendEvent -= SendEvent;
        GlobalEventManager.Instance.EvtUpdateUserProperties -= SetUserPropeties;
#endif
    }

#endregion

#region Public Methods
    public void Init()
    {
        Debug.Log(TAG+" Init");
#if USE_FIREBASE
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                app = FirebaseApp.DefaultInstance;
                isOk = true;
                FetchValue();
                Debug.Log(TAG+" Init done");
                GlobalEventManager.Instance.OnLoadingStart();
            } else {
                Debug.Log(TAG+" Init fail");
            }
        });
#endif
    }
#endregion

#region Private Methods
#if USE_FIREBASE
    private void SendEvent(string eName, Parameter[] parameters)
    {
        if(!isOk)
            return;
        if(parameters== null)
            FirebaseAnalytics.LogEvent(eName);
        else
            FirebaseAnalytics.LogEvent(eName,parameters);
    }
    private void SetUserPropeties()
    {
        if(!isOk)
            return;       
    }
#endif
#endregion

#region Remote Config
    private void FetchValue()
    {
     
    
        SetDefaultValue();
        TimeSpan time = new TimeSpan(0, 0, 10);
#if USE_FIREBASE
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(time).ContinueWithOnMainThread(task =>
        {
          
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                // if (!PlayerPrefs.HasKey("colorBackGround") && !PlayerPrefs.HasKey("FistTimeOpen"))
                // {
                //     string colorBackGround = FirebaseRemoteConfig.DefaultInstance.GetValue("colorBackGround").StringValue;
                //     if (colorBackGround != null)
                //     {
                //         if (int.Parse(colorBackGround) == 1)
                //         {
                //             AppConfig.Instance.BACK_GROUND = true;
                //             //Light
                //         }
                //         else if (int.Parse(colorBackGround) == 0)
                //         {
                //             AppConfig.Instance.BACK_GROUND = false;
                //             //Dark
                //         }
                //     }
                //     if (colorBackGround == null)
                //     {
                    

                //     }
                // }

                
            }
        });
#endif
    }

    private void SetDefaultValue()
    {
#if USE_FIREBASE
        if (PlayerPrefs.GetInt("SetDefaultConfig", 0) == 0)
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            defaults.Add("active_churn", "0");
            defaults.Add("active_spam_ads", "0");
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
            PlayerPrefs.SetInt("SetDefaultConfig", 1);
        }
#endif
    }
#endregion
    
}

public enum USER_PROPETIES
{
    HighestLevel,
    Cats,
    Gem,
    Coin,
    Ads,
    Iap,
    Member
}
public enum EVENT_NAME
{
    PlayLevel,
    WinLevel,
    LoseLevel,
    ads_banner,
    ads_click,
    ads_interstitial_load,
    ads_interstitial_show,
    ads_rewarded_show,
    ads_rewarded_complete,
    CoinEarn,
    CoinSpend,
    GemEarn,
    GemSpend,
    IAP,
    UseKittyPaw,
    UseCatFish,
    UseAngleMeow,
    UseSupeMeow,
    GetKittyPaw,
    GetCatFish,
    GetAngleMeow,
    GetSupeMeow,
    EquipCat,
    UpgradeCat,
    GetCat,
    TutorialBegin,
    TutorialComplete
}