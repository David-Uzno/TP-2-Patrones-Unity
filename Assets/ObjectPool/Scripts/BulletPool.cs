using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
#region Variables
    public static BulletPool Instance { get; private set; }

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public int poolSize = 20;

    [Header("Bullet Pool")]
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
#endregion

#region Unity Methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
#endregion

#region Pool Management
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        GameObject bullet;
        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Dequeue();
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
        }

        bullet.SetActive(true);
        bullet.GetComponent<Projectile>().ResetState();
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
#endregion
}
