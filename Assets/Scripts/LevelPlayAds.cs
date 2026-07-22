// using Unity.Services.LevelPlay;
// using UnityEngine;

// public class LevelPlayAds : MonoBehaviour
// {
//     private string myGameIdAndroid = "26eab234d";
//     private string myGameIdIOS = "26dbf9e8d";

//     LevelPlayBannerAd banner;
//     private string bannerAdUnitIdAndroid = "h6qkm48te259f3nc";
//     private string bannerAdUnitIdIOS = "v30zqlrtdo8ze504";

//     LevelPlayRewardedAd rewarded;
//     private string rewardedAdUnitIdAndroid = "kxkd43hqeonuajpl";
//     private string rewardedAdUnitIdIOS = "v30zqlrtdo8ze504";

//     private string gameId;
//     private string bannerAdUnitId;
//     private string rewardedAdUnitId;


//     void Start()
//     {
//         //Debug.Log("LevelPlayAds");

// #if UNITY_ANDROID

//         gameId = myGameIdAndroid;
//         bannerAdUnitId = bannerAdUnitIdAndroid;
//         rewardedAdUnitId = rewardedAdUnitIdAndroid;
// #endif


// #if UNITY_IOS
        
//         gameId = myGameIdIOS;
//         bannerAdUnitId = bannerAdUnitIdIOS;
//         rewardedAdUnitId = rewardedAdUnitIdIOS;
// #endif

//         LevelPlay.SetMetaData("is_test_suite", "enable");


//         LevelPlay.Init(gameId);
//         LevelPlay.OnInitSuccess += OnInitSuccess;

//         /*banner = new LevelPlayBannerAd(bannerAdUnitId);
//         banner.LoadAd();
//         banner.OnAdLoaded += BannerAdLoaded;*/

//         rewarded = new LevelPlayRewardedAd(rewardedAdUnitId);
//         rewarded.LoadAd();
//         rewarded.OnAdLoaded += RewardedAdLoaded;
        
//     }


//     void OnInitSuccess(LevelPlayConfiguration config)
//     {
//         Debug.Log("OnInitSuccess");
//         //LevelPlay.LaunchTestSuite();
//     }

//     void BannerAdLoaded(LevelPlayAdInfo info)
//     {
//         Debug.Log("Banner loaded");
//         //banner.ShowAd();
//     }

//     void RewardedAdLoaded(LevelPlayAdInfo info)
//     {
//         Debug.Log("Rewarded loaded");
//         rewarded.ShowAd();
//     }

//     public void LaunchTestSuite()
//     {
//         Debug.Log("LaunchTestSuite");
//         LevelPlay.LaunchTestSuite();
//     }
// }
