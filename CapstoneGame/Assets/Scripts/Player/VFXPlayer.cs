using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    public GameObject _spinSparkle;
    public GameObject _boostAfterImage;
    public GameObject _wooshCircle;
    public GameObject _dustEffect;

    public int _afterImageAmount;
    public float _afterImageFreq;
    public void Spin_Sparkle()
    {
        Instantiate(_spinSparkle, this.transform);
    }
    public void Woosh(float offsetY) {
        Instantiate(_wooshCircle, 
            new Vector3(this.transform.position.x, this.transform.position.y+offsetY,this.transform.position.z), 
            this.transform.rotation);
    }
    public void Boost_AfterImage(bool _isFlip) {
        StartCoroutine(AfterImageCoroutine(_isFlip));
    }
    public void DustEffect(float offsetY) {
        Instantiate(_dustEffect, new Vector3(this.transform.position.x, this.transform.position.y+offsetY,this.transform.position.z), this.transform.rotation);
    }
    IEnumerator AfterImageCoroutine(bool _isFlip) {
        for(int i = 0; i < _afterImageAmount; i++) {
            GameObject VFX = Instantiate(_boostAfterImage, this.transform.position, this.transform.rotation);
            VFX.GetComponent<SpriteRenderer>().flipX = _isFlip;
            yield return new WaitForSeconds(_afterImageFreq);
        }
    }
}
