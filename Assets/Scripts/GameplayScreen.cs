using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    public static GameplayScreen ins;
    void Awake() { ins = this; }

    public Homescreen homeScreen;
    public SelectionScreen selectionScreen;

    public Transform weaponBtns;

    public WeaponAttributes pistolBtn;

    public Button attackFromLeftBtn, takeCoverBtn, attackFromRightBtn;
    public GameObject jumpBtn, attackBtn, face1Btn, face2Btn, weaponAim;
    public GameObject climbBtn;
    public GameObject dropBtn;
    public Slider trajectoryAimingStrength;

    public Action onAttackFromLeftClicked, onTakeCoverClicked, onAttackFromRightClicked;

    //Heli Stuff
    public Slider heliHeightAdjuster;

    void Start()
    {
        for (int i = 0; i < WeaponManager.ins.transform.childCount; i++) 
        {
            WeaponAttributes weaponAttributes = WeaponManager.ins.transform.GetChild(i).GetComponent<WeaponAttributes>();
            //weaponAttributes.weaponSelectBtn.GetComponent<Button>().interactable = selectionScreen.unlockedWeapons.Contains(weaponAttributes.weaponID);
        }
    }


    public void ClimbBtnOnClick()
    {
        ClimblingMedium.activeClimblingMedium.ClimbUp();
    }

    public void DropBtnOnClick()
    {
        ClimblingMedium.activeClimblingMedium.ComeDown();
    }


    public void HomeBtnOnClick()
    {
        CastleAttack.ins.PlayBtnSound();
        ScreenUtils.ActivateScreen(gameObject, homeScreen.gameObject);
    }


    public void AttackFromLeftOnClick()
    {
        onAttackFromLeftClicked?.Invoke();
    }

    public void TakeCoverOnClick()
    {
        onTakeCoverClicked?.Invoke();
    }

    public void AttackFromRightOnClick()
    {
        onAttackFromRightClicked?.Invoke();
    }
}
