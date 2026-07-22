using UnityEngine;

public class Homescreen : MonoBehaviour
{
    public GameObject allScreensBg;

    public GameplayScreen gameplayScreen;
    public SelectionScreen selectionScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        allScreensBg.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StubbyOnClick()
    {
        //CastleAttack.ins.PlayBtnSound();
        //ScreenUtils.ActivateScreen(gameObject, selectionScreen.gameObject);
    }




    public void ExitBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
        Application.Quit();
    }

    public void CoinsBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    public void SettingsBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    public void PlayBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();

        allScreensBg.SetActive(false);
        ScreenUtils.ActivateScreen(gameObject, gameplayScreen.gameObject, delegate { TopMenu.ins.gameObject.SetActive(false); });
    }

    public void PreviousBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    public void NextBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
        ScreenUtils.ActivateScreen(gameObject, selectionScreen.gameObject);
    }

    public void WinsBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    public void KdBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    public void CapturesBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }
}
