using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
#if UNITY_IOS
    private string gameId = "1234567";
#elif UNITY_ANDROID
    private string gameId = "1234567";
#endif

    private bool testMode = true;

    void Start()
    {
        if(!Advertisement.isInitialized)
            Advertisement.Initialize(gameId, testMode);
    }
}
