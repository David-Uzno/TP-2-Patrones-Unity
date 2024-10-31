using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
#region Variables
    public enum TurretType
    {
        Single = 1,
        Dual = 2,
        Catapult = 3,
    }

    [Header("Targeting")]
    public GameObject currentTarget;
    public Transform turreyHead;
    private Transform lockOnPos;

    [Header("Attack Settings")]
    public float attackDist = 10.0f;
    public float attackDamage;
    public float shootCoolDown;
    private float timer;
    public float loockSpeed;

    [Header("Rotation Settings")]
    public Vector3 randomRot;
    public Animator animator;

    [Header("Turret Type")]
    public TurretType turretType = TurretType.Single;

    [Header("Muzzle Settings")]
    public Transform muzzleMain;
    public Transform muzzleSub;
    public GameObject muzzleEff;
    public GameObject bullet;
    private bool shootLeft = true;
#endregion

#region Unity Methods
    void Start ()
    {
        InvokeRepeating("ChackForTarget", 0, 0.5f);
        //shotScript = GetComponent<TurretShoot_Base>();

        if (transform.GetChild(0).GetComponent<Animator>())
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        randomRot = new Vector3(0, Random.Range(0, 359), 0);
    }
	
    void Update() {
        if (currentTarget != null)
        {
            FollowTarget();
            float currentTargetDist = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (currentTargetDist > attackDist)
            {
                currentTarget = null;
            }
        }
        else
        {
            IdleRitate();
        }

        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && timer >= shootCoolDown && currentTarget != null)
        {
            timer = 0;

            if (animator != null)
            {
                animator.SetTrigger("Fire");
            }

            ShootTrigger();
        }
    }
#endregion

#region Turret Behavior Methods
    private void ChackForTarget()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, attackDist);
        float distAway = Mathf.Infinity;

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Player")
            {
                float dist = Vector3.Distance(transform.position, colls[i].transform.position);

                if (dist < distAway)
                {
                    currentTarget = colls[i].gameObject;
                    distAway = dist;
                }
            }
        }
    }

    private void FollowTarget()
    {
        Vector3 targetDir = currentTarget.transform.position - turreyHead.position;
        targetDir.y = 0;
        //turreyHead.forward = targetDir;

        if (turretType == TurretType.Single)
        {
            turreyHead.forward = targetDir;
        }
        else
        {
            turreyHead.transform.rotation = Quaternion.RotateTowards(turreyHead.rotation, Quaternion.LookRotation(targetDir), loockSpeed * Time.deltaTime);
        }
    }

    private void ShootTrigger()
    {
        Shoot(currentTarget);
    }
#endregion

#region Helper Methods
    Vector3 CalculateVelocity(Vector3 target, Vector3 origen, float time)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }

    public void IdleRitate()
    {
        bool refreshRandom = false;
        
        if (turreyHead.rotation != Quaternion.Euler(randomRot))
        {
            turreyHead.rotation = Quaternion.RotateTowards(turreyHead.transform.rotation, Quaternion.Euler(randomRot), loockSpeed * Time.deltaTime * 0.2f);
        }
        else
        {
            refreshRandom = true;

            if (refreshRandom)
            {

                int randomAngle = Random.Range(0, 359);
                randomRot = new Vector3(0, randomAngle, 0);
                refreshRandom = false;
            }
        }
    }

    public void Shoot(GameObject go)
    {
        Transform muzzle = (muzzleSub != null && shootLeft) ? muzzleSub : muzzleMain;
        shootLeft = !shootLeft;

        Instantiate(muzzleEff, muzzle.position, muzzle.rotation);

        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;

        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.target = go.transform;
    }
#endregion
}
