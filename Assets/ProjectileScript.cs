using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Animator animationLoop;

    private void Awake()
    { 
        animationLoop = GetComponent<Animator>();
        animationLoop.Play("FireballAnimation");
        StartCoroutine(DestroyProjectile());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(6f);
    }
}
