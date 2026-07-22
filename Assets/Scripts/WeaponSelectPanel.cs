using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectPanel : MonoBehaviour
{
    public string weaponId;

    public float unlockCost;


    public GameObject unlockBtn;

    public Text costText;


    void Start()
    {
        costText.text = unlockCost.ToString();
    }


    public void UnlockBtnOnClick()
    {
        if (int.Parse(SelectionScreen.ins.coinBalance.text) < unlockCost) { return; }
        
        SelectionScreen.ins.unlockedWeapons.Add(weaponId);
        unlockBtn.SetActive(false);

        SelectionScreen.ins.coinBalance.text = (int.Parse(SelectionScreen.ins.coinBalance.text) - unlockCost).ToString();
        SelectionScreen.ins.coinBalanceHomeScreen.text = (int.Parse(SelectionScreen.ins.coinBalanceHomeScreen.text) - unlockCost).ToString();
    }
}
