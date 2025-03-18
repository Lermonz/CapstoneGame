using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisableOnWorldX : MonoBehaviour
{
    [SerializeField] private MainMenuManager _manager;
    public int worldX;
    void Update()
    {
        this.GetComponent<Button>().interactable = _manager.CurrentWorld != worldX;
    }
}
