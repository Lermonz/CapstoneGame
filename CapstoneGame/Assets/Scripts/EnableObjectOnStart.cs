using UnityEngine;

public class EnableObjectOnStart : MonoBehaviour
{
    [SerializeField] GameObject[] _objects;
    void Start()
    {
        foreach (GameObject _object in _objects)
        {
            _object.SetActive(true);
        }
    }
}
