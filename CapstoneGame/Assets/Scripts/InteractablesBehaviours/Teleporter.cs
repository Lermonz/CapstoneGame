using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject LinkedTo;
    public void PlaySFX() {
        AkSoundEngine.PostEvent("Tele_Object", gameObject);
    }
}
