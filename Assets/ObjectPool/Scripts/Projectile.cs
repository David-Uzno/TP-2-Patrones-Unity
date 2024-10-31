using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
#region Variables
    [Header("Projectile Type")]
    public TurretAI.TurretType type = TurretAI.TurretType.Single;
    public bool catapult;

    [Header("Movement Settings")]
    public Transform target;
    public float speed = 1;
    public float turnSpeed = 1;
    public bool lockOn; 

    [Header("Explosion Settings")]
    public float knockBack = 0.1f;
    public float boomTimer = 1;
    private float initialBoomTimer;

    [Header("Effects")]
    public ParticleSystem explosion;
#endregion

#region Unity Methods
    private void Awake()
    {
        initialBoomTimer = boomTimer;
    }

    private void Update()
    {
        boomTimer -= Time.deltaTime;

        if (boomTimer <= 0)
        {
            ExplodeAndReturn();
        }
        
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            float step = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Time.deltaTime, 0.0f);

            transform.Translate(Vector3.forward * step);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
#endregion

#region State Management
    public void ResetState()
    {
        boomTimer = initialBoomTimer;
        lockOn = catapult;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (type == TurretAI.TurretType.Single && target != null)
        {
            Vector3 dir = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
#endregion

#region Calculation Methods
    Vector3 CalculateCatapult(Vector3 target, Vector3 origen, float time)
    {
        Vector3 distance = target - origen;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExplodeAndReturn();
        }
    }

    private void ExplodeAndReturn()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        BulletPool.Instance.ReturnBullet(gameObject);
    }
#endregion
}
