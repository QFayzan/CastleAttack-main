using UnityEngine;

public class HeliBtns : MonoBehaviour
{
    public GameObject Up, Down, Jump;

    void OnEnable()
    {
        Jump.SetActive(false);
        Up.SetActive(true);
        Down.SetActive(true);
    }
    void OnDisable()
    {
         Jump.SetActive(true);
        Up.SetActive(false);
        Down.SetActive(false);
    }
}
