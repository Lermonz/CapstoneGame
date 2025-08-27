using System.Collections;
using TMPro;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform LinkedTo;
    [SerializeField] Texture _palette;
    [SerializeField] SpriteRenderer _renderer;
    public float _time = 20;
    Vector3 offset;
    Vector3 startPos;
    Vector3 endPos;

    public GameObject _teleportTravelCameraFollow;
    void Start()
    {
        _renderer.material.SetTexture("_Palette", _palette);
        offset.z = Random.Range(0, 270);
        this.transform.localEulerAngles = offset;
        startPos = this.transform.position;
        endPos = LinkedTo.position;
    }
    void Update()
    {
        this.transform.localEulerAngles += 10 * Vector3.forward * Time.deltaTime;
        if (this.transform.localEulerAngles.z >= (360 + offset.z))
        {
            this.transform.localEulerAngles = offset;
        }
    }
    public void Teleport()
    {
        AkSoundEngine.PostEvent("Tele_Object", gameObject);
        StartCoroutine(TeleportTravel());
    }
    IEnumerator TeleportTravel()
    {
        _teleportTravelCameraFollow.transform.position = startPos;
        _teleportTravelCameraFollow.GetComponent<ParticleSystem>().Play();
        for (int i = 0; i < _time; i++)
        {
            _teleportTravelCameraFollow.transform.position = Vector3.Lerp(startPos, endPos, i / _time);
            yield return null;
        }
        _teleportTravelCameraFollow.GetComponent<ParticleSystem>().Stop();
    }
}
