using System.Collections;
using UnityEngine;

public class LizardShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 10f;

    public Animator animator;
    private float shootDelay = 1.5f;

    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            animator.Play("Stand");
            yield return new WaitForSeconds(shootDelay);

            animator.Play("Shoot");
            yield return new WaitForSeconds(0.2f);

            Shoot();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        projectile.tag = "Enemy";

        StartCoroutine(MoveProjectile(projectile));

         

    }

    private IEnumerator MoveProjectile(GameObject projectile)
    {
        while (projectile != null)
        {
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, shootPoint.position, projectileSpeed * Time.deltaTime);

            if (projectile.transform.position == shootPoint.position)
            {
                Destroy(projectile);
                yield break;
            }

            yield return null;
        }
    }
}