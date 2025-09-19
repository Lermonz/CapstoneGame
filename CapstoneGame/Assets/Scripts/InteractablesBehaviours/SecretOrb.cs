using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretOrb : MonoBehaviour, IDataPersistence
{
    [SerializeField] string _costumeID;
    [SerializeField] Animator _orbAnimation;
    [SerializeField] ParticleSystem _bigParticles;
    [SerializeField] ParticleSystem _smallParticles;
    bool _collected = false;
    public void TouchedByPlayer()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        DataPersistenceManager.Instance.SaveGame();
        PauseMenu.Instance.CollectedOrb();
        AkSoundEngine.PostEvent("Costume_Get", gameObject);
        _collected = true;
        AnimateCollection();
        Destroy(gameObject, 1f);
    }
    public void SaveData(GameData data)
    {
        if (data.costumes.ContainsKey(_costumeID) && !data.costumes[_costumeID] && _collected)
        {
            data.costumes[_costumeID] = true;
        }
    }
    public void LoadData(GameData data) { return; }
    void AnimateCollection()
    {
        _orbAnimation.SetTrigger("Collected");
        _bigParticles.Play();
        _smallParticles.Play();
    }
}
