using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    public GameObject _spinSparkle;
    public GameObject _boostAfterImage;
    public GameObject _wooshCircle;
    public GameObject _dustEffect;
    public GameObject _regenerateSprite;
    public GameObject _regenerateEffect;
    public GameObject _specialJumpParticles;
    GameObject DustObject = null;

    public int _afterImageAmount;
    public float _afterImageFreq;
    public void Spin_Sparkle()
    {
        Instantiate(_spinSparkle, this.transform);
    }
    public void SpecialJump()
    {
        Instantiate(_specialJumpParticles, this.transform);
    }
    public void Woosh(float offsetY)
    {
        Instantiate(_wooshCircle,
            new Vector3(this.transform.position.x, this.transform.position.y + offsetY, this.transform.position.z),
            this.transform.rotation);
    }
    public void Boost_AfterImage(bool _isFlip)
    {
        StartCoroutine(AfterImageCoroutine(_isFlip));
    }
    public void RegenerateBoost()
    {
        StartCoroutine(RegenerateCoroutine());
    }
    public void DustEffect(float offsetY = 0, float offsetX = 0, float rotation = 0)
    {
        if(DustObject == null)
        {    
            DustObject = Instantiate(_dustEffect, new Vector3(this.transform.position.x + offsetX, this.transform.position.y + offsetY, this.transform.position.z),
            new Quaternion(this.transform.rotation.x + 90 * -rotation, this.transform.rotation.y + 90 * Math.Abs(rotation), this.transform.rotation.z + 90 * Math.Abs(rotation), this.transform.rotation.w));
        }
    }
    IEnumerator AfterImageCoroutine(bool _isFlip)
    {
        for (int i = 0; i < _afterImageAmount; i++)
        {
            GameObject VFX = Instantiate(_boostAfterImage, this.transform.position, this.transform.rotation);
            VFX.GetComponent<SpriteRenderer>().flipX = _isFlip;
            yield return new WaitForSeconds(_afterImageFreq);
        }
    }
    IEnumerator RegenerateCoroutine()
    {
        Instantiate(_regenerateSprite, this.transform);
        for (int i = 0; i < 11; i++)
        {
            yield return null;
        }
        Instantiate(_regenerateEffect, this.transform);
    }
}
