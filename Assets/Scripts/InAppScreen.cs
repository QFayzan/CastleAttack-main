// using UnityEngine;
// using UnityEngine.UI;

// public class InAppScreen : MonoBehaviour
// {
//     public static InAppScreen ins;
//     void Awake() { ins = this; }

//     public Text  coinsPackageOneTxt, coinsPackageTwoTxt, coinsPackageThreeTxt, coinsPackageFourTxt;

    
//     void Start()
//     {
//         coinsPackageOneTxt.text   = "Buy 1000 Coins (" + IAP.ins.GetProductPrice("coins_package_one") + ")";
//         coinsPackageTwoTxt.text   = "Buy 3000 Coins (" + IAP.ins.GetProductPrice("coins_package_two") + ")";
//         coinsPackageThreeTxt.text = "Buy 5000 Coins (" + IAP.ins.GetProductPrice("coins_package_three") + ")";
//         coinsPackageFourTxt.text  = "Buy 10000 Coins (" + IAP.ins.GetProductPrice("coins_package_four") + ")";
//     }

    
//     public void WatchAdOnClick()
//     {
//         GameSession.ins.PlayBtnSound();

//         UnityAdsRewarded.ins.Show(delegate
//         {
//             GameData.ins.UpdateCoins(20);
//         });
//     }
    
   


//     public void CoinsPackageOneOnClick()
//     {
//         GameSession.ins.PlayBtnSound();

//         IAP.ins.BuyProduct("Are you sure you want to purchase 1000 coins for " + IAP.ins.GetProductPrice("coins_package_one"), "coins_package_one", delegate { GameData.ins.UpdateCoins(1000); });
//     }

//     public void CoinsPackageTwoOnClick()
//     {
//         GameSession.ins.PlayBtnSound();

//         IAP.ins.BuyProduct("Are you sure you want to purchase 3000 coins for " + IAP.ins.GetProductPrice("coins_package_two"), "coins_package_two", delegate { GameData.ins.UpdateCoins(3000); });
//     }

//     public void CoinsPackageThreeOnClick()
//     {
//         GameSession.ins.PlayBtnSound();

//         IAP.ins.BuyProduct("Are you sure you want to purchase 5000 coins for " + IAP.ins.GetProductPrice("coins_package_three"), "coins_package_three", delegate { GameData.ins.UpdateCoins(5000); });
//     }

//     public void CoinsPackageFourOnClick()
//     {
//         GameSession.ins.PlayBtnSound();

//         IAP.ins.BuyProduct("Are you sure you want to purchase 10000 coins for " + IAP.ins.GetProductPrice("coins_package_four"), "coins_package_four", delegate { GameData.ins.UpdateCoins(10000); });
//     }

//     public void BackOnClick()
//     {
//         GameSession.ins.PlayBtnSound();

//         ScreenUtils.ActivateScreen(gameObject, null, delegate { TopMenu.ins.Appear(); });


//     }
// }
