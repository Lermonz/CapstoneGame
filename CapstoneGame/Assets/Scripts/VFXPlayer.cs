using System.Collections;
using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    public GameObject _spinSparkle;
    public GameObject _boostAfterImage;

    public int _afterImageAmount;
    public float _afterImageFreq;
    public void Spin_Sparkle() {
        Instantiate(_spinSparkle, this.transform);
    }
    public void Boost_AfterImage(bool _isFlip) {
        StartCoroutine(AfterImageCoroutine(_isFlip));
    }
    IEnumerator AfterImageCoroutine(bool _isFlip) {
        for(int i = 0; i < _afterImageAmount; i++) {
            GameObject VFX = Instantiate(_boostAfterImage, this.transform.position, this.transform.rotation);
            VFX.GetComponent<SpriteRenderer>().flipX = _isFlip;
            yield return new WaitForSeconds(_afterImageFreq);
        }
    }
}
