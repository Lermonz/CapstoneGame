using UnityEngine;

public class SetTime : MonoBehaviour
{
    public void SetTimeTo(float scale)
    {
        Time.timeScale = scale;
        Debug.Log("timeScale: " + Time.timeScale);
    }
}
