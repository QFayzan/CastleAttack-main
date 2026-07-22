// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Purchasing;
// using UnityEngine.Purchasing.Extension;

// [System.Serializable]
// public class IAP : MonoBehaviour, IDetailedStoreListener
// {
//     public static IAP ins;
//     public void Awake() { if (ins == null) { ins = this; } }

//     public List<ProductInfo> products = new List<ProductInfo>();

//     IStoreController m_StoreController; // The Unity Purchasing system.
//     IExtensionProvider extensions; 

//     public Action onPurchaseSuccessful;
//     private Action<string> onProductPurchase;

//     [System.Serializable]
//     public class ProductInfo
//     {
//         public string productId;
//         public ProductType productType;
//     }

//     void Start()
//     {
//         if (products.Count > 0) { InitializePurchasing(); }
//     }


//     public void InitializePurchasing()
//     {
//         ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

//         //Add products that will be purchasable and indicate its type.
//         for (int i = 0; i < products.Count; i++) { configurationBuilder.AddProduct(products[i].productId, products[i].productType); }

//         UnityPurchasing.Initialize(this, configurationBuilder); 
//     }

   

//     public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//     {
//         Debug.Log("In-App Purchasing successfully initialized");
//         m_StoreController = controller;
//     }


//     public void BuyProduct(string noticeText, string productId, Action<string> onProductPurchase)
//     {
//         if (Application.internetReachability == NetworkReachability.NotReachable) 
//         { 
//             NoticeUtils.ins.ShowOneBtnAlert("Please enable your internet connection");
//             return;
//         }

//         if (string.IsNullOrEmpty(noticeText))
//         {
//             this.onProductPurchase = onProductPurchase;
//             m_StoreController.InitiatePurchase(productId);
//         }
//         else 
//         {
//             NoticeUtils.ins.ShowTwoBtnAlert(noticeText, (i) =>
//             {
//                 if (i == 0)
//                 {
//                     this.onProductPurchase = onProductPurchase;
//                     m_StoreController.InitiatePurchase(productId);
//                 }
//             });
//         }

        
        
//     }


//     public string GetProductPrice(string productId)
//     {
//         foreach (var product in m_StoreController.products.all)
//         {
//             if (product.definition.id == productId) 
//             {
//                 return product.metadata.localizedPriceString;
//             }
            
//             //Debug.Log(string.Format("string: {0}", product.metadata.localizedPriceString));

//             //Debug.Log(string.Format("decimal: {0}", product.metadata.localizedPrice.ToString()));
//         }
//         return string.Empty;
//     }




//     public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
//     {
//         //Retrieve the purchased product
//         var product = purchaseEvent.purchasedProduct;
//         Debug.Log($"Purchase Complete - Product: {product.definition.id}");

//         onPurchaseSuccessful?.Invoke();
//         onProductPurchase?.Invoke(product.transactionID);
//         //AnalyticsManager.ins.ProductPurchased(product.definition.id);

//         //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
//         return PurchaseProcessingResult.Complete;
//     }


//     public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
//     {
//         Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureDescription}");
//     }

//     public void OnInitializeFailed(InitializationFailureReason error)
//     {
//         Debug.Log($"In-App Purchasing initialize failed: {error}");
//     }

//     public void OnInitializeFailed(InitializationFailureReason error, string message)
//     {

//     }


//     public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//     {

//     }

//     /*void RestorePurchases()
//     {
//         extensions.GetExtension<IAppleExtensions>().RestoreTransactions((result, str) =>
//         {
//             if (result)
//             {
//                 purchasesRestored = true;
//                 onSuccess?.Invoke();
//             }
//             else
//             {
//                 NoticeUtils.ins.ShowOneBtnAlert("Some problem occurred while restoring purchases.");
//                 onFailed?.Invoke();
//             }
//         });
//     }*/
// }
