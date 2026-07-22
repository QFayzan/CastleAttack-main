using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    public static TopMenu ins;
    void Awake() { ins = this; }

    public RectTransform helper;

    public Text coinsCount;

    public AudioClip coinsCounting;

    private void Start()
    {
        coinsCount.text = GameData.ins.coins.ToString();
        GameData.ins.onCoinsUpdate += () =>
        {
            int currentCoins = int.Parse(coinsCount.text);
            if (GameData.ins.coins > currentCoins)
            {
                coinsCount.DOCounter(currentCoins, GameData.ins.coins, 1, false);
                AudioSource.PlayClipAtPoint(coinsCounting, Camera.main.transform.position);
            }
            else { coinsCount.text = GameData.ins.coins.ToString(); }

        };
    }






    public void Appear()
    {
        helper.DOLocalMoveY(0, .5f);
    }

    public void Hide()
    {
        helper.DOLocalMoveY(150, .5f);
    }

    public void ExitBtnOnClick()
    {
        GameSession.ins.PlayBtnSound();
        Application.Quit();
    }

    public void CoinsBtnOnClick()
    {
        GameSession.ins.PlayBtnSound();
     //mk   ScreenUtils.ActivateScreen(null, uiManager.ins.inAppScreen.gameObject);
    }

    public void SettingsBtnOnClick()
    {
        GameSession.ins.PlayBtnSound();
    }
}
