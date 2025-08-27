using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretOrb : MonoBehaviour, IDataPersistence
{
    [SerializeField] string _costumeID;
    public void TouchedByPlayer()
    {
        DataPersistenceManager.Instance.SaveGame();
    }
    public void SaveData(GameData data)
    {
        if (data.costumes.ContainsKey(_costumeID) && !data.costumes[_costumeID])
        {
            data.costumes[_costumeID] = true;
        }
    }
    public void LoadData(GameData data) { return; }
    IEnumerator CollectedDelay()
    {
        InputManager.Instance._freezeVelocity = true;
        yield return new WaitForSeconds(0.25f);
        InputManager.Instance._freezeVelocity = false;
        Destroy(gameObject);
    }
}
