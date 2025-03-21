using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public float _delay;
    public void LoadLevel (int level) {
        SetGameBehavior(true);
        SceneManager.LoadScene(level);
    }
    public void LoadMenu () {
        SetGameBehavior(false);
        SceneManager.LoadScene(0);
    }
    public void NextScene() {
        Scene nextLevel = SceneManager.GetActiveScene();
        if(SceneManager.GetSceneByBuildIndex(nextLevel.buildIndex+1) != null)
            LoadLevel(nextLevel.buildIndex+1);
    }
    public void Reload() {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    void SetGameBehavior(bool game) {
        GameBehaviour.Instance._isGame = game;
    }
    public void DelayedLoadLevel (int level) {
        StartCoroutine(DelayThenLoad(level));
    }
    IEnumerator DelayThenLoad(int level) {
        yield return new WaitForSeconds(_delay);
        LoadLevel(level);
    }
}
