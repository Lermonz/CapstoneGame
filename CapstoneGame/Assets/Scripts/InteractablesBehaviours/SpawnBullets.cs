using System.Collections;
using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifetime;
    [SerializeField] float spawnTime = 3;
    [SerializeField] float initialOffset = 0;
    [SerializeField] ParticleSystem dustBurst;
    Vector3 deltaVector;
    float spawnPosOffset = 0.5f;
    Quaternion zeroRotation = new Quaternion(0,0,0,0);
    void Start()
    {
        float delta = Mathf.Deg2Rad*this.transform.localEulerAngles.z;
        deltaVector = new Vector3(-Mathf.Cos(delta),-Mathf.Sin(delta),0);
        StartCoroutine(SpawnTimer(initialOffset));
    }
    IEnumerator SpawnTimer(float offset = 0)
    {
        yield return new WaitForSeconds(spawnTime + offset);
        Instantiate(dustBurst, this.transform.position+deltaVector*spawnPosOffset, zeroRotation);
        GameObject BulletObj = Instantiate(bullet, this.transform.position+deltaVector*spawnPosOffset, zeroRotation);
        if(BulletObj.GetComponent<LinearTranslateWithLifetime>() != null)
        {
            LinearTranslateWithLifetime BulletProperties = BulletObj.GetComponent<LinearTranslateWithLifetime>();
            BulletProperties.speed = bulletSpeed;
            BulletProperties.lifetime = bulletLifetime;
            BulletProperties.direction = this.transform.localEulerAngles.z;
            BulletProperties.dustBurst = dustBurst;
        }
        StartCoroutine(SpawnTimer());
    }
}
