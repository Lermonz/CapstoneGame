using System.Collections;
using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifetime;
    [SerializeField] float spawnTime = 5;
    [SerializeField] ParticleSystem dustBurst;
    Quaternion zeroRotation = new Quaternion(0,0,0,0);
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }
    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(spawnTime);
        Instantiate(dustBurst, this.transform);
        GameObject BulletObj = Instantiate(bullet, this.transform.position, zeroRotation);
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
