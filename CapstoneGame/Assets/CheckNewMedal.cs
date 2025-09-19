using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckNewMedal : MonoBehaviour
{
    public Image _image;
    [SerializeField] Sprite[] _medalSprites;
    [SerializeField] GameObject[] _diamondInfo;
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
                _image.sprite = _medalSprites[0];
            else if (medal == "silver")
            {
                _image.sprite = _medalSprites[1];
                HideDiamondInfo();
            }
            else if (medal == "bronze")
            {
                _image.sprite = _medalSprites[2];
                HideDiamondInfo();
            }
            else if (medal == "diamond")
                _image.sprite = _medalSprites[3];
            else
            {
                _image.color = Color.black;
                HideDiamondInfo();
            }
        }
    }
    void HideDiamondInfo()
    {
        foreach (GameObject diamondObject in _diamondInfo) {
            diamondObject.SetActive(false);
        }
    }
}
