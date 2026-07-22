using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager ins;
    void Awake() { ins = this; }

    public Transform weaponSelectBtns;
    public WeaponAttributes activeWeapon;

    public Image attackColliderImage;
    public Image attackBtnBg;

    public LayerMask shootLayers;

    public GameObject weaponAim, attackBtn;

    private Anim anim;
    private float animPlayedRecTime;

    [Space(30)]
    public bool isAttacking;

    void Start()
    {
        activeWeapon.SelectWeapon();

        EventTrigger eventTrigger = attackColliderImage.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        eventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerDragEntry = new EventTrigger.Entry();
        pointerDragEntry.eventID = EventTriggerType.Drag;
        pointerDragEntry.callback.AddListener((data) => { OnPointerDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(pointerDragEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        eventTrigger.triggers.Add(pointerUpEntry);
    }

    void Update()
    {
        if (activeWeapon.canAttack) 
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                OnPointerDown(null);
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                OnPointerUp(null);
            }
        }
        
    }

    public void OnPointerDown(PointerEventData data)
    {
        attackBtnBg.color = Color.green;
        StartCoroutine("Attack");
        isAttacking = true;
    }



    public void OnPointerDrag(PointerEventData data)
    {
        CameraController.ins.cameraParent.Rotate(-data.delta.y * Time.deltaTime * CameraController.ins.lookSpeed, 0, 0);
        CameraController.ins.character.Rotate(0, data.delta.x * Time.deltaTime * CameraController.ins.lookSpeed, 0, Space.World);

    }

    public void OnPointerUp(PointerEventData data)
    {
        StopAttack();

       /* if (!activeWeapon.canCompleteAttack) { TPSController.ins.PlayIdleWalkOrRunIfNotJumpingOrAttacking(); }
        else 
        {
            StopCoroutine("PlayIdleWalkOrRunIfNotJumpingOrAttackingAfterDelay");
            StartCoroutine("PlayIdleWalkOrRunIfNotJumpingOrAttackingAfterDelay", anim.GetAnimationLength() - (Time.realtimeSinceStartup - animPlayedRecTime));
        }*/
    }


    IEnumerator PlayIdleWalkOrRunIfNotJumpingOrAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //TPSController.ins.PlayIdleWalkOrRunIfNotJumpingOrAttacking();
    }



    public void StopAttack()
    {
        attackBtnBg.color = Color.white;
        StopCoroutine("Attack");
        isAttacking = false;
        //TPSController.ins.anims.activeAnimType = string.Empty;
    }

    


    IEnumerator Attack()
    {
        while (true) 
        {
            //TPSController.ins.fighter..BlendWeaponAttackAnim();

            /*PlayerAttrsManager.ins.ApplyPlayerAttrsBasedOnAnim(activeWeapon.weaponID, "Attack");
            anim = TPSController.ins.anims.PlayAttack();
            animPlayedRecTime = Time.realtimeSinceStartup;

            if (activeWeapon.aimShootWeapon) { Shoot(anim.shootingWeapon); }
            if (activeWeapon.physicsProjectileWeapon) { anim.shootingWeapon.ShootPhysicsProjectile(); }

            Camera.main.transform.DOShakePosition(activeWeapon.camShakeDuration, activeWeapon.camShakeStrength);*/

            yield return new WaitForSeconds(activeWeapon.attackInterval);
        }
    }

    void Shoot(ShootingWeaponModel shootingWeapon)
    {
        // Get screen position of UI image
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, weaponAim.transform.position);

        // Create ray from camera through UI image position
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit shootRaycastHit, Mathf.Infinity))
        {
            shootingWeapon.ShootAtTarget(shootRaycastHit);
        }
        else 
        {
            shootingWeapon.Shoot();
        }

    }

}
