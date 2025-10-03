using UnityEngine;
public class BringBackUIControl : MonoBehaviour
{
    void OnEnable()
    {
        MainMenuManager.Instance.SetNoCancelling(true);
    }
    void OnDisable()
    {
        InputManager.Instance.DisablePlayerInput();
        MainMenuManager.Instance.SetNoCancelling(false);
    }
}