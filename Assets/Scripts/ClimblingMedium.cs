using DG.Tweening;
using UnityEngine;

public class ClimblingMedium : MonoBehaviour
{
    public static ClimblingMedium activeClimblingMedium;

    public Transform lowerPoint, lowerLandingPoint;
    public Transform upperPoint, upperLandingPoint;


   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }


        activeClimblingMedium = this;
        if (Vector3.Distance(lowerPoint.position, TPSController.ins.fighter.transform.position) < Vector3.Distance(upperPoint.position, TPSController.ins.fighter.transform.position))
        {
            GameplayScreen.ins.climbBtn.SetActive(true);
            GameplayScreen.ins.dropBtn.SetActive(false);
        }
        else 
        {
            GameplayScreen.ins.climbBtn.SetActive(false);
            GameplayScreen.ins.dropBtn.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") { return; }


        GameplayScreen.ins.climbBtn.SetActive(false);
        GameplayScreen.ins.dropBtn.SetActive(false);
    }


    public void ClimbUp()
    {
        GameplayScreen.ins.gameObject.SetActive(false);

        GameplayScreen.ins.climbBtn.SetActive(false);

        TPSController.ins.fighter.enabled = false;

        //TPSController.ins.fighter.fighterModels.gameObject.SetActive(false);
        //TPSController.ins.fighter.climbAnimation.gameObject.SetActive(true);


        TPSController.ins.fighter.transform.position = lowerPoint.position;
        TPSController.ins.fighter.transform.eulerAngles = lowerPoint.eulerAngles;
        TPSController.ins.fighter.climbAnimation.transform.localEulerAngles = Vector3.zero;

        Transform camTarget = CameraController.ins.camTarget;

        CameraController.ins.SetCamTarget(TPSController.ins.camDefaultPosition);
        TPSController.ins.fighter.PlayClimbAnim();

        TPSController.ins.fighter.transform.DOMove(upperPoint.position, 4).SetEase(Ease.Linear).OnComplete(delegate 
        {
            GameplayScreen.ins.gameObject.SetActive(true);

            TPSController.ins.fighter.transform.position = upperLandingPoint.position;
            TPSController.ins.fighter.transform.eulerAngles = upperLandingPoint.eulerAngles;

            //TPSController.ins.GetComponent<TPSController>().enabled = true;
            //TPSController.ins.GetComponent<CharacterController>().enabled = true;

            //TPSController.ins.fighter.fighterModels.gameObject.SetActive(true);
            //TPSController.ins.fighter.climbAnimation.gameObject.SetActive(false);
            TPSController.ins.fighter.PlayIdleAnim();


            CameraController.ins.SetCamTarget(camTarget);

            TPSController.ins.fighter.enabled = true;
            if (TPSController.ins.fighter.GetActiveWeapon() != null) { TPSController.ins.fighter.activeFighterModel.SelectWeapon(TPSController.ins.fighter.GetActiveWeapon().weaponID); }
        });
    }

    public void ComeDown()
    {

        GameplayScreen.ins.gameObject.SetActive(false);

        GameplayScreen.ins.climbBtn.SetActive(false);

        TPSController.ins.fighter.enabled = false;

        TPSController.ins.fighter.fighterModels.gameObject.SetActive(false);
        TPSController.ins.fighter.climbAnimation.gameObject.SetActive(true);


        TPSController.ins.fighter.transform.position = upperPoint.position;
        TPSController.ins.fighter.transform.eulerAngles = upperPoint.eulerAngles;
        TPSController.ins.fighter.climbAnimation.transform.localEulerAngles = Vector3.zero;
        
        Transform camTarget = CameraController.ins.camTarget;

        CameraController.ins.SetCamTarget(TPSController.ins.camDefaultPosition);

        TPSController.ins.fighter.transform.DOMove(lowerPoint.position, 4).SetEase(Ease.Linear).OnComplete(delegate
        {
            GameplayScreen.ins.gameObject.SetActive(true);

            TPSController.ins.fighter.transform.position = lowerLandingPoint.position;
            TPSController.ins.fighter.transform.eulerAngles = lowerLandingPoint.eulerAngles;

            //TPSController.ins.GetComponent<TPSController>().enabled = true;
            //TPSController.ins.GetComponent<CharacterController>().enabled = true;

            TPSController.ins.fighter.fighterModels.gameObject.SetActive(true);
            TPSController.ins.fighter.climbAnimation.gameObject.SetActive(false);

            CameraController.ins.SetCamTarget(camTarget);

            TPSController.ins.fighter.enabled = true;
            if (TPSController.ins.fighter.GetActiveWeapon() != null) { TPSController.ins.fighter.activeFighterModel.SelectWeapon(TPSController.ins.fighter.GetActiveWeapon().weaponID); }
        });


        
    }
}
