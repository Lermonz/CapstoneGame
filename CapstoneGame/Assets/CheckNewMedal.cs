using EasyTextEffects;
using UnityEngine;
using UnityEngine.UI;

public class CheckNewMedal : MonoBehaviour
{
    public Image _image;
    [SerializeField] Sprite[] _medalSprites;
    [SerializeField] GameObject[] _diamondInfo;
    [SerializeField] GameObject[] _developerInfo;
    [SerializeField] TextEffect[] _textEffects;
    public void MedalLoad(string oldMedal, string newMedal)
    {
        MedalLoad(newMedal);
        if (oldMedal != newMedal && newMedal != "none")
        {
            this.GetComponent<Animator>().SetTrigger("NewMedal");
        }
    }
    public void MedalLoad(string medal)
    {
        _image.color = Color.black;
        if (medal != null)
        {
            _image.color = Color.white;
            //Debug.Log("MedalLoad has been called!!!!" + medal);
            if (medal == "gold")
            {
                _image.sprite = _medalSprites[0];
                HideSpecificInfo(_developerInfo);
                return;
            }
            else if (medal == "silver")
            {
                _image.sprite = _medalSprites[1];
            }
            else if (medal == "bronze")
            {
                _image.sprite = _medalSprites[2];
            }
            else if (medal == "diamond")
            {
                _image.sprite = _medalSprites[3];
                return;
            }
            else if (medal == "developer")
            {
                _image.sprite = _medalSprites[4];
                EnableTextEffects();
                return;
            }
            else
            {
                _image.color = Color.black;
            }
        }
        HideSpecificInfo(_diamondInfo);
        HideSpecificInfo(_developerInfo);
    }
    void HideSpecificInfo(GameObject[] _infoObjects)
    {
        foreach (GameObject diamondObject in _infoObjects)
        {
            diamondObject.SetActive(false);
        }
    }
    void EnableTextEffects()
    {
        if(_textEffects.Length == 0) { return; }
        foreach (TextEffect textEffect in _textEffects)
        {
            textEffect.enabled = true;
        }
    }
}
