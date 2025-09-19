using UnityEngine;
using TMPro;

public class WinMenuText : MonoBehaviour
{
    [SerializeField] TMP_Text _yourTime;
    [SerializeField] TMP_Text _yourPB;
    [SerializeField] TMP_Text _diamondTime;
    [SerializeField] TMP_Text _goldTime;
    [SerializeField] TMP_Text _silverTime;
    [SerializeField] TMP_Text _bronzeTime;
    void Start()
    {
        this._diamondTime.text= string.Format("{0:00}:{1:00}.{2:000}", 
                    LevelManager.Instance._diamondTime.x,
                    LevelManager.Instance._diamondTime.y,
                    LevelManager.Instance._diamondTime.z);
        this._goldTime.text= string.Format("{0:00}:{1:00}.{2:000}", 
                    LevelManager.Instance._goldTime.x,
                    LevelManager.Instance._goldTime.y,
                    LevelManager.Instance._goldTime.z);
        this._silverTime.text= string.Format("{0:00}:{1:00}.{2:000}", 
                    LevelManager.Instance._silverTime.x,
                    LevelManager.Instance._silverTime.y,
                    LevelManager.Instance._silverTime.z);
        this._bronzeTime.text= string.Format("{0:00}:{1:00}.{2:000}", 
                    LevelManager.Instance._bronzeTime.x,
                    LevelManager.Instance._bronzeTime.y,
                    LevelManager.Instance._bronzeTime.z);
    }
    void OnEnable()
    {
        _yourTime.text= string.Format("{0:00}:{1:00}.{2:000}", 
                    Timer.Instance.m,
                    Timer.Instance.s,
                    Timer.Instance.ms);
        LevelManager.Instance.CheckPB();
        Vector3 pb = GameBehaviour.Instance.ConvertTimerToVector3(LevelManager.Instance._personalBest);
        _yourPB.text= string.Format("{0:00}:{1:00}.{2:000}", 
                    pb.x,
                    pb.y,
                    pb.z);
        DataPersistenceManager.Instance.SaveGame();
    }
}
