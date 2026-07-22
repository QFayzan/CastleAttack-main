using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController ins;
    void Awake() { ins = this; }

    public bool rotateCamWithoutMouseDown = true;
    public Image colliderImage;

    public Transform cam;
    public Transform camTarget;
    public Transform camDefaultTarget;

    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public bool lookYLimitActive;
    public float lookYLimit = 60.0f;

    Vector2 rotation = Vector2.zero;

    public Transform character;
    public Transform cameraParent;


    void Start()
    {
        rotation.y = transform.eulerAngles.y;

        if (colliderImage != null)
        {
            EventTrigger eventTrigger = colliderImage.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerDragEntry = new EventTrigger.Entry();
            pointerDragEntry.eventID = EventTriggerType.Drag;
            pointerDragEntry.callback.AddListener((data) => { OnPointerDrag((PointerEventData)data); });
            eventTrigger.triggers.Add(pointerDragEntry);
        }

        if (Application.platform != RuntimePlatform.Android && rotateCamWithoutMouseDown) { StartCoroutine("RotateCamBasedOnMouse"); }
    }

    void Update()
    {
        //cam.position = Vector3.Lerp(cam.position, camTarget.position, 10*Time.deltaTime);
        //cam.rotation = Quaternion.Lerp(cam.rotation, camTarget.rotation, 10 * Time.deltaTime);

        //cam.position = camTarget.position;
        //cam.rotation = camTarget.rotation;
    }

    IEnumerator RotateCamBasedOnMouse()
    {
        yield return new WaitForSeconds(1); 
        while (true)
        {
            cameraParent.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * lookSpeed * 40, 0, 0);
            character.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * lookSpeed * 40, 0, Space.World);
            yield return null;
        }
    }

    public void OnPointerDrag(PointerEventData data)
    {
         
        cameraParent.Rotate(-data.delta.y * Time.deltaTime * lookSpeed, 0, 0);
        character.Rotate(0, data.delta.x * Time.deltaTime * lookSpeed, 0, Space.World);
        

        /*rotation.y += data.delta.x * Time.deltaTime * lookSpeed;
        rotation.x += -data.delta.y * Time.deltaTime * lookSpeed;

        rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
        if (lookYLimitActive) { rotation.y = Mathf.Clamp(rotation.y, -lookYLimit, lookYLimit); }

        cameraParent.localRotation = Quaternion.Euler(rotation.x, cameraParent.localRotation.y, 0);
        character.eulerAngles = new Vector2(character.eulerAngles.x, rotation.y);*/
    }


    public void SetCamTarget(Transform target)
    {
        if (target == camTarget) { return; }
        camTarget = target;

        CameraCollision.ins.enabled = false;

        cam.DOKill();

        cam.DOLocalRotate(target.localEulerAngles, .5f);
        cam.DOLocalMove(target.localPosition, .5f).OnComplete(delegate 
        {
            CameraCollision.ins.SetDefaults();
            CameraCollision.ins.enabled = true;
        });

    }

}