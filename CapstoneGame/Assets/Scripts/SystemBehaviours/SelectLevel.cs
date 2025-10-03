using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public float _delay = 0.5f;
    public void LoadLevel(int level)
    {
        SetGameBehavior(true);
        SceneManager.LoadScene(level);
    }
    public void LoadMenu(bool deathDelay = true)
    {
        SetGameBehavior(false);
        StartCoroutine(DelayThenLoad(0, deathDelay));
    }
    public void NextScene(bool deathDelay = true)
    {
        Scene nextLevel = SceneManager.GetActiveScene();
        if (SceneManager.GetSceneByBuildIndex(nextLevel.buildIndex + 1) != null)
            StartCoroutine(DelayThenLoad(nextLevel.buildIndex + 1, deathDelay));
    }
    public void Reload(bool deathDelay = true)
    {
        StartCoroutine(DelayThenLoad(SceneManager.GetActiveScene().buildIndex, deathDelay));
    }
    void SetGameBehavior(bool game)
    {
        GameBehaviour.Instance._isGame = game;
    }
    public void DelayedLoadLevel(int level)
    {
        StartCoroutine(DelayThenLoad(level));
    }
    IEnumerator DelayThenLoad(int level, bool deathDelay = true)
    {
        float delayTime = deathDelay ? _delay : 0.05f;
        yield return new WaitForSecondsRealtime(delayTime);
        ScreenWipe();
        yield return new WaitForSecondsRealtime(0.5f);
        LoadLevel(level);
    }
    void ScreenWipe()
    {
        Debug.Log("ScreenWipe");
        if(SceneManager.GetActiveScene().buildIndex > 0)
            PauseMenu.Instance.OnResetLevel();
    }
}
