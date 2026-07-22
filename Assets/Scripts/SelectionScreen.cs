using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScreen : MonoBehaviour
{
    public static SelectionScreen ins;
    void Awake() { ins = this; }

    public Text coinBalance, coinBalanceHomeScreen;


    public Homescreen homescreen;

    public Transform weapons;

    public List<string> unlockedWeapons;


    void Start()
    {
        for (int i = 0; i < weapons.childCount; i++)
        {
            WeaponSelectPanel weaponSelectPanel = weapons.GetChild(i).GetComponent<WeaponSelectPanel>();
            weaponSelectPanel.unlockBtn.SetActive(!unlockedWeapons.Contains(weaponSelectPanel.weaponId));
        }
    }



    public void BackBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
        ScreenUtils.ActivateScreen(gameObject, homescreen.gameObject);
    }

    public void CoinsBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    public void SettingsBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    
    public void PreviousBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
        ScreenUtils.ActivateScreen(gameObject, homescreen.gameObject);
        
    }

    public void NextBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
    }

    

}
