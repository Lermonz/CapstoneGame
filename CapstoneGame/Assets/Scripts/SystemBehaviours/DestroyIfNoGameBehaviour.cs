using UnityEngine;

public class DestroyIfNoGameBehaviour : MonoBehaviour
{
    void Update()
    {
        if(this.GetComponent<GameBehaviour>() == null) {
            Destroy(this.gameObject);
        }
    }
}
