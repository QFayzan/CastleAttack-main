using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class Notice : MonoBehaviour
{
    public Text headingTxt;
    public Text noticeTxt;
    public Text firstBtnTxt;
    public Text secondBtnTxt;
    public InputField inputField;

    private Action<int> callBack;
    private Action<int, string> inputAlertCallBack;

    private Action onLoadingTimeout;
    private int timeoutTime;

    public void Show(string heading, string notice, Action<int> callBack = null, string firstBtnTxt = "Ok", string secondBtnTxt = "Cancel")
    {
        gameObject.SetActive(true);
        this.callBack = callBack;
        if (headingTxt != null) { headingTxt.text = heading; }
        noticeTxt.text = notice;

        if (this.firstBtnTxt != null) { this.firstBtnTxt.text = firstBtnTxt; }
        if (this.secondBtnTxt != null) { this.secondBtnTxt.text = secondBtnTxt; }
    }


    public void NotificationBtnOnClick(Transform btnTransform)
    {
        //AudioSource.PlayClipAtPoint(PocketLove.ins.btnSound, Camera.main.transform.position);
        GameSession.ins.PlayBtnSound();

        gameObject.SetActive(false);
        callBack?.Invoke(btnTransform.GetSiblingIndex());
        inputAlertCallBack?.Invoke(btnTransform.GetSiblingIndex(), inputField.text);
    }


    public void ShowInputAlert(string notice, Action<int, string> callBack, string placeholder = "Type here...", string firstBtnTxt = "Submit", string secondBtnTxt = "Close")
    {
        gameObject.SetActive(true);
        inputAlertCallBack = callBack;
        noticeTxt.text = notice;

        inputField.placeholder.GetComponent<Text>().text = placeholder;

        if (this.firstBtnTxt != null) { this.firstBtnTxt.text = firstBtnTxt; }
        if (this.secondBtnTxt != null) { this.secondBtnTxt.text = secondBtnTxt; }
    }





    public GameObject ShowLoadingAlert(string notice, int timeoutTime = 15, Action onLoadingTimeout = null)
    {
        gameObject.SetActive(true);
        this.onLoadingTimeout = onLoadingTimeout;
        this.timeoutTime = timeoutTime;
        StopCoroutine("LoadingAlert");
        StartCoroutine("LoadingAlert", notice);
        return gameObject;
    }

    public void HideLoadingAlert()
    {
        StopCoroutine("LoadingAlert");
        gameObject.SetActive(false);
        onLoadingTimeout = null;
    }

    IEnumerator LoadingAlert(string notice)
    {
        float alertHideTime = timeoutTime;
        gameObject.SetActive(true);
        while (alertHideTime > 0)
        {
            string dots = "";
            for (int i = 0; i < 4; i++)
            {
                dots += ".";
                noticeTxt.text = notice + dots;
                alertHideTime -= .5f;
                yield return new WaitForSeconds(.5f);
            }
        }
        onLoadingTimeout?.Invoke();
        gameObject.SetActive(false);
    }
}
