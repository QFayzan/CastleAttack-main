// using System;
// using UnityEngine;
// using Unity.Services.LevelPlay;   // SDK 9.x. If you are on package 8.5–8.x use: using com.unity3d.mediation;

// /// <summary>
// /// Minimal, production-ready rewarded-ad manager for Unity LevelPlay.
// /// Requires LevelPlay Unity package 8.5.0+ (Ads Mediation package).
// ///
// /// Setup:
// /// 1. Attach this script to a persistent GameObject (it survives scene loads).
// /// 2. Fill in your App Key and Rewarded Ad Unit ID from the LevelPlay dashboard.
// /// 3. Call ShowRewardedAd() from a button to display an ad.
// /// </summary>
// public class LevelPlayRewardedAds : MonoBehaviour
// {
//     [Header("LevelPlay IDs (from the dashboard)")]
//     [Tooltip("App Key from LevelPlay > your app")]
//     [SerializeField] private string appKey = "YOUR_APP_KEY";

//     [Tooltip("Rewarded Ad Unit ID from LevelPlay > Ad Units")]
//     [SerializeField] private string rewardedAdUnitId = "YOUR_REWARDED_AD_UNIT_ID";

//     [Tooltip("Optional. Leave empty unless using S2S reward callbacks.")]
//     [SerializeField] private string userId = "";

//     // Fired when the user actually earns the reward. Subscribe from your game code.
//     public event Action<string, int> OnRewardEarned; // (rewardName, rewardAmount)

//     private LevelPlayRewardedAd _rewardedAd;
//     private bool _isInitialized;

//     // ---------------------------------------------------------------------
//     // Initialization
//     // ---------------------------------------------------------------------
//     private void Awake()
//     {
//         // Keep the ad manager alive across scene changes.
//         DontDestroyOnLoad(gameObject);
//     }

//     private void Start()
//     {
//         LevelPlay.OnInitSuccess += OnInitSuccess;
//         LevelPlay.OnInitFailed  += OnInitFailed;

//         // Initialize the SDK. Pass userId only if you set up server-to-server rewards.
//         if (string.IsNullOrEmpty(userId))
//             LevelPlay.Init(appKey);
//         else
//             LevelPlay.Init(appKey, userId);
//     }

//     private void OnInitSuccess(LevelPlayConfiguration config)
//     {
//         Debug.Log("[LevelPlay] Init success.");
//         _isInitialized = true;

//         // The rewarded ad object MUST be created after init succeeds.
//         CreateRewardedAd();
//         LoadRewardedAd();
//     }

//     private void OnInitFailed(LevelPlayInitError error)
//     {
//         Debug.LogError($"[LevelPlay] Init failed: {error?.ErrorMessage}");
//     }

//     // ---------------------------------------------------------------------
//     // Create + subscribe to ad events
//     // ---------------------------------------------------------------------
//     private void CreateRewardedAd()
//     {
//         _rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);

//         _rewardedAd.OnAdLoaded        += OnAdLoaded;
//         _rewardedAd.OnAdLoadFailed    += OnAdLoadFailed;
//         _rewardedAd.OnAdDisplayed     += OnAdDisplayed;
//         //_rewardedAd.OnAdDisplayFailed += OnAdDisplayFailed;
//         _rewardedAd.OnAdRewarded      += OnAdRewarded;
//         _rewardedAd.OnAdClicked       += OnAdClicked;
//         _rewardedAd.OnAdClosed        += OnAdClosed;
//         _rewardedAd.OnAdInfoChanged   += OnAdInfoChanged;
//     }

//     // ---------------------------------------------------------------------
//     // Public API — call these from your game
//     // ---------------------------------------------------------------------

//     /// <summary>Loads a rewarded ad into memory so it's ready to show.</summary>
//     public void LoadRewardedAd()
//     {
//         if (!_isInitialized || _rewardedAd == null)
//         {
//             Debug.LogWarning("[LevelPlay] Not ready to load yet.");
//             return;
//         }
//         _rewardedAd.LoadAd();
//     }

//     /// <summary>Returns true if an ad is loaded and ready to display.</summary>
//     public bool IsRewardedAdReady() => _rewardedAd != null && _rewardedAd.IsAdReady();

//     /// <summary>
//     /// Shows the rewarded ad. Pass a placementName if you set up placements
//     /// in the dashboard; otherwise leave it null.
//     /// </summary>
//     public void ShowRewardedAd(string placementName = null)
//     {
//         if (_rewardedAd == null || !_rewardedAd.IsAdReady())
//         {
//             Debug.LogWarning("[LevelPlay] Rewarded ad not ready. Reloading.");
//             LoadRewardedAd();
//             return;
//         }

//         // Respect placement capping rules set in the dashboard.
//         if (!string.IsNullOrEmpty(placementName) &&
//             LevelPlayRewardedAd.IsPlacementCapped(placementName))
//         {
//             Debug.Log($"[LevelPlay] Placement '{placementName}' is capped.");
//             return;
//         }

//         _rewardedAd.ShowAd(placementName);
//     }

//     // ---------------------------------------------------------------------
//     // Ad event callbacks (run on the main thread)
//     // ---------------------------------------------------------------------
//     private void OnAdLoaded(LevelPlayAdInfo adInfo)
//         => Debug.Log($"[LevelPlay] Ad loaded: {adInfo.AdNetwork}");

//     private void OnAdLoadFailed(LevelPlayAdError error)
//     {
//         Debug.LogWarning($"[LevelPlay] Load failed: {error?.ErrorMessage}");
//         // Simple retry. For production, back off with an increasing delay.
//         Invoke(nameof(LoadRewardedAd), 5f);
//     }

//     private void OnAdDisplayed(LevelPlayAdInfo adInfo)
//         => Debug.Log("[LevelPlay] Ad displayed.");

//    /* private void OnAdDisplayFailed(LevelPlayAdDisplayInfoError infoError)
//     {
//         Debug.LogWarning($"[LevelPlay] Display failed: {infoError?.LevelPlayError?.ErrorMessage}");
//         LoadRewardedAd(); // get a fresh ad ready
//     }*/

//     // NOTE: argument order is (adInfo, reward) on SDK 8.5.1+.
//     // On some older builds it is (reward, adInfo) — swap the params if the compiler complains.
//     private void OnAdRewarded(LevelPlayAdInfo adInfo, LevelPlayReward reward)
//     {
//         Debug.Log($"[LevelPlay] Reward earned: {reward.Name} x{reward.Amount}");
//         GrantReward(reward.Name, reward.Amount);
//     }

//     private void OnAdClicked(LevelPlayAdInfo adInfo)
//         => Debug.Log("[LevelPlay] Ad clicked.");

//     private void OnAdClosed(LevelPlayAdInfo adInfo)
//     {
//         Debug.Log("[LevelPlay] Ad closed. Preloading the next one.");
//         LoadRewardedAd(); // always have the next ad ready
//     }

//     private void OnAdInfoChanged(LevelPlayAdInfo adInfo)
//         => Debug.Log("[LevelPlay] Ad info changed.");

//     // ---------------------------------------------------------------------
//     // Reward handling
//     // ---------------------------------------------------------------------
//     private void GrantReward(string rewardName, int rewardAmount)
//     {
//         // OnAdRewarded and OnAdClosed are asynchronous and can arrive in either
//         // order, so grant the reward here regardless of close timing.
//         OnRewardEarned?.Invoke(rewardName, rewardAmount);
//         // TODO: add coins/gems/lives to the player here.
//     }

//     // ---------------------------------------------------------------------
//     // Cleanup
//     // ---------------------------------------------------------------------
//     private void OnDestroy()
//     {
//         LevelPlay.OnInitSuccess -= OnInitSuccess;
//         LevelPlay.OnInitFailed  -= OnInitFailed;

//         if (_rewardedAd != null)
//         {
//             _rewardedAd.OnAdLoaded        -= OnAdLoaded;
//             _rewardedAd.OnAdLoadFailed    -= OnAdLoadFailed;
//             _rewardedAd.OnAdDisplayed     -= OnAdDisplayed;
//            // _rewardedAd.OnAdDisplayFailed -= OnAdDisplayFailed;
//             _rewardedAd.OnAdRewarded      -= OnAdRewarded;
//             _rewardedAd.OnAdClicked       -= OnAdClicked;
//             _rewardedAd.OnAdClosed        -= OnAdClosed;
//             _rewardedAd.OnAdInfoChanged   -= OnAdInfoChanged;
//             _rewardedAd.DestroyAd();
//         }
//     }
// }
