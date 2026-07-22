using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NoticeUtils : MonoBehaviour
{
    public static NoticeUtils ins;
    void Awake() { ins = this; }

    public bool internetCheck;
    
    public Notice oneBtnAlert;
    public Notice twoBtnAlert;
    public Notice loadingAlert;
    public Notice inputAlert;
    public Notice twoBtnAlertHeading;
    public GameObject internetNotice;




    void Start()
    {
        DontDestroyOnLoad(this);
        if (internetCheck) { StartCoroutine("InternetCheck"); }
    }

    IEnumerator InternetCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!internetCheck) { yield break; }
            if (Application.internetReachability == NetworkReachability.NotReachable /*&& !Application.isEditor*/)
            {internetNotice.SetActive(true);}
            else
            {internetNotice.SetActive(false);}
        }
    }

   

    public void ShowOneBtnAlert(string notice, Action<int> callBack = null, string btnTxt = "Ok")
    {
        oneBtnAlert.Show(string.Empty, notice, callBack, btnTxt);
    }

    public void ShowTwoBtnAlert(string notice, Action<int> callBack = null, string btnTxt = "Ok", string secondBtnTxt = "Cancel")
    {
        twoBtnAlert.Show(string.Empty, notice, callBack, btnTxt, secondBtnTxt);
    }

    public void ShowTwoBtnAlertHeading(string heading, string notice, Action<int> callBack = null, string btnTxt = "Ok", string secondBtnTxt = "Cancel")
    {
        twoBtnAlertHeading.Show(heading, notice, callBack, btnTxt, secondBtnTxt);
    }

    public void ShowLoadingAlert(string notice)
    {
        loadingAlert.ShowLoadingAlert(notice);
    }

    public void ShowInputAlert(string notice, Action<int, string> callBack, string placeholder = "Type here...", string firstBtnTxt = "Submit", string secondBtnTxt = "Close")
    {
        inputAlert.ShowInputAlert(notice, callBack, placeholder = "Type here...", firstBtnTxt = "Submit", secondBtnTxt = "Close");
    }

    public void HideLoadingAlert()
    {
        loadingAlert.HideLoadingAlert();
    }
}
