using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretOrb : MonoBehaviour, IDataPersistence
{
    [SerializeField] string _costumeID;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Animator _orbAnimation;
    [SerializeField] ParticleSystem _bigParticles;
    [SerializeField] ParticleSystem _smallParticles;
    bool _collected = false;
    bool _persistentCollected = false;
    public void TouchedByPlayer()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        if (!_persistentCollected) { PauseMenu.Instance.CollectedOrb(); }
        AkSoundEngine.PostEvent("Costume_Get", gameObject);
        _collected = true;
        AnimateCollection();
        Destroy(gameObject, 1f);
        DataPersistenceManager.Instance.SaveGame();
    }
    public void SaveData(GameData data)
    {
        if (data.costumes.ContainsKey(_costumeID) && !data.costumes[_costumeID] && _collected)
        {
            data.costumes[_costumeID] = true;
        }
    }
    public void LoadData(GameData data)
    {
        if (!_collected && data.costumes[_costumeID])
        {
            _renderer.color = new Color(0.35f, 0.4f, 0.55f);
            _persistentCollected = true;
        }
    }
    void AnimateCollection()
    {
        _orbAnimation.SetTrigger("Collected");
        _bigParticles.Play();
        _smallParticles.Play();
    }
}
