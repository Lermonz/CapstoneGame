using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckNewMedal : MonoBehaviour
{
    public Image _image;
    public void MedalLoad(string oldMedal, string newMedal)
    {
        MedalLoad(newMedal);
        if (oldMedal != newMedal)
        {
            this.GetComponent<Animator>().SetTrigger("NewMedal");
        }
    }
    public void MedalLoad(string medal)
    {
        Debug.Log("MedalLoad has been called!!!!");
        if (medal == "gold")
            _image.color = Color.yellow;
        else if (medal == "silver")
            _image.color = Color.grey;
        else if (medal == "bronze")
            _image.color = Color.red;
        else
            _image.color = Color.white;
    }
}
