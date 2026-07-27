using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TPSController : MonoBehaviour
{
    public static TPSController ins;
    void Awake() { ins = this; }


    public Joystick joystick;
    public Fighter fighter;

    public Transform weaponSelectBtns;

    public Image attackColliderImage;
    public Image attackBtnBg;

    [Header("For Second WEapons")]
    public Image attackColliderImageTwo;
    public Image attackBtnBgTwo;

    public LayerMask shootLayers;

    public GameObject weaponAim, attackBtn;

    public Transform camDefaultPosition, camZoomPosition;

    public Action onJoystickDown, onJumpBtnPressed;

    [Space(30)]
    public bool isAttacking;



    void Start()
    {
        if (joystick != null) { joystick.onJoystickHalfDown += OnJoystickHalfDown; }
        if (joystick != null) { joystick.onJoystickFullDown += OnJoystickFullDown; }

        if (joystick != null) { joystick.onJoystick += OnJoystick; }
        if (joystick != null) { joystick.onJoystickUp += OnJoystickUp; }


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


       

        fighter.activeFighterModel = null;



        SelectWeapon("Stubby","Hands");
    }




    public void SelectWeapon(string fighterID, string weaponID)
    {
        fighter.SelectWeapon(fighterID, weaponID);

        if (fighter.GetActiveWeapon().lookForwardLock) 
        { 
            CameraController.ins.SetCamTarget(camZoomPosition);
            if (!fighter.takenCover) { fighter.fighterModels.transform.localEulerAngles = new Vector3(0, 15, 0); }
        } 
        else 
        { 
            CameraController.ins.SetCamTarget(camDefaultPosition);
            if (!fighter.takenCover) { fighter.fighterModels.transform.localEulerAngles = new Vector3(0, 0, 0); }
        }

        if (fighter.GetActiveWeapon().shootingWeapon != null && fighter.GetActiveWeapon().shootingWeapon.aimShooting) { weaponAim.SetActive(true); } else { weaponAim.SetActive(false); }

    }


    void OnJoystickHalfDown()
    {
        fighter.PlayWalkAnim();
        onJoystickDown?.Invoke();
    }

    void OnJoystickFullDown()
    {
        fighter.PlayRunAnim();
        onJoystickDown?.Invoke();
    }

    void OnJoystick(Vector3 dir, bool fullIntensity)
    {
        if (fullIntensity)  { fighter.Move(dir, fullIntensity); } else { fighter.Move(dir, fullIntensity); }
    }

    void OnJoystickUp()
    {
        
        fighter.StoppedMoving();
    }

    public void JumpBtnOnClick()
    {
        fighter.activeFighterModel.Jump();
        onJumpBtnPressed?.Invoke();
    }
    public void HeliUp()
    {
        if(fighter.transform.position.y < 8)
        {
            fighter.activeFighterModel.HeliUpDown(10,.2f);
        }
        else
        {
            fighter.transform.position = new Vector3(fighter.transform.position.x,8,fighter.transform.position.z);
        }
         
    }
    public void HeliDown()
    {
        if (fighter.transform.position.y > 0.5)
        {
            fighter.transform.position = new Vector3(fighter.transform.position.x,
                fighter.transform.position.y - 2,
                fighter.transform.position.z);
        }
        else
        {
            fighter.transform.position = new Vector3(fighter.transform.position.x, 0.5f, fighter.transform.position.z);
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
        attackBtnBg.color = Color.white;
        StopCoroutine("Attack");
        isAttacking = false;
        fighter.activeFighterModel.AttackStopped();
    }


   

    IEnumerator Attack()
    {
        while (true)
        {
            fighter.activeFighterModel.BlendWeaponAttackAnim();

            if (fighter.GetActiveWeapon().shootingWeapon != null) 
            {
                if (fighter.GetActiveWeapon().shootingWeapon.aimShooting) { Shoot(fighter.GetActiveWeapon().shootingWeapon); }
                if (fighter.GetActiveWeapon().shootingWeapon.physicsProjectileShooting) { fighter.GetActiveWeapon().shootingWeapon.ShootPhysicsProjectile(); }

                Camera.main.transform.DOShakePosition(.2f, .1f);
            }
            yield return new WaitForSeconds(fighter.GetActiveWeapon().attackInterval);
        }
    }
    //Second Weapon Try
    public void OnPointerDownTwo()
    {
        attackBtnBgTwo.color = Color.green;
        StartCoroutine(nameof(SecondAttack));
        
    }
    public void OnPointerUpTwo()
    {
        attackBtnBgTwo.color = Color.white;
        StopCoroutine(nameof(SecondAttack));
       
       
    }
    IEnumerator SecondAttack()
    {
        while (true)
        {

            if (fighter.GetSecondaryWeapon().shootingWeapon != null)
            {
               
                if (fighter.GetSecondaryWeapon().shootingWeapon.aimShooting) { Shoot(fighter.GetSecondaryWeapon().shootingWeapon); }
                if (fighter.GetSecondaryWeapon().shootingWeapon.physicsProjectileShooting) { fighter.GetSecondaryWeapon().shootingWeapon.ShootPhysicsProjectile(); }

                Camera.main.transform.DOShakePosition(.2f, .1f);
            }
            yield return new WaitForSeconds(fighter.GetSecondaryWeapon().attackInterval);
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







    float characterX;
    float characterZ;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.J)) { fighter.activeFighterModel.Jump(); }
        
        
        characterX = Input.GetAxis("Vertical");
        characterZ = Input.GetAxis("Horizontal");

        Vector3 dir = new Vector3(-characterX, 0, characterZ);

        if (characterX != 0 || characterZ != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
            { 
                fighter.Move(dir, true); 
            } 
            else                 
            {
                fighter.Move(dir, false);
            }
        }

        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) 
        { fighter.PlayWalkAnim(); }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) { fighter.PlayRunAnim(); }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) { fighter.PlayWalkAnim(); }
        }



        if (Input.GetKeyUp(KeyCode.W)) { if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { fighter.PlayIdleAnim(); } }
        if (Input.GetKeyUp(KeyCode.S)) { if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { fighter.PlayIdleAnim(); } }

        if (Input.GetKeyUp(KeyCode.A)) { if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D)) { fighter.PlayIdleAnim(); } }
        if (Input.GetKeyUp(KeyCode.D)) { if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W)) { fighter.PlayIdleAnim(); } }


        if (Input.GetKeyUp(KeyCode.UpArrow)) { if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) { fighter.PlayIdleAnim(); } }
        if (Input.GetKeyUp(KeyCode.DownArrow)) { if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) { fighter.PlayIdleAnim(); } }
                                                                                                                                                                
        if (Input.GetKeyUp(KeyCode.LeftArrow)) { if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow)) { fighter.PlayIdleAnim(); } }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow)) { fighter.PlayIdleAnim(); } }

        


    }
}