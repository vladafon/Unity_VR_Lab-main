using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{

    private static List<GameObject> Targets;
    private static float DeadY;
    private static PhysicMaterial Material;

    private Coroutine _lifeTimeCoroutine;


    // 
    public void BulletControllerInit(IEnumerable<GameObject> targets,float deadY, PhysicMaterial material)
    {
        if (targets != null)
        {
            Targets = targets.ToList();
        }
        else
        {
            Targets = new List<GameObject>();
        }

        DeadY = deadY;
        Material = material;



        if (!gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            gameObject.AddComponent<Rigidbody>();
            rb = gameObject.GetComponent<Rigidbody>();
        }
        rb.isKinematic = true;

        var collider = gameObject.GetComponent<SphereCollider>();
        collider.material = Material;

        gameObject.SetActive(false);

        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    public void StartLifeTimeCoroutine(float lifeTime)
    {
        _lifeTimeCoroutine = StartCoroutine(BulletCoroutine(lifeTime));
    }

    void Update()
    {
        if (gameObject.activeInHierarchy == true && Math.Abs(transform.position.y) > Math.Abs(DeadY))
        {
            if (_lifeTimeCoroutine != null)
                StopCoroutine(_lifeTimeCoroutine);

            BulletReturnToPool();
        }
    }

    private void BulletReturnToPool()
    {
        var ball = gameObject;
        if (ball != null)
        {
            ball.transform.position = new Vector3(0, 0, 0);

            if (ball.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }

            if (ball.TryGetComponent<SphereCollider>(out SphereCollider collider))
            {
                collider.material = Material;
            }

            ball.SetActive(false);
        }
    }


    private IEnumerator BulletCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        _lifeTimeCoroutine = null;
        BulletReturnToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // такое решение с предварительным пробросом целей работает, но очень плохо масштабируется, лучше избегать такой вариант.
        if (Targets.Contains(collision.gameObject))
        {
            var targetScript = collision.gameObject.GetComponent<TargetController>();

            targetScript.Shooted();

            EventManager.RaiseOnShooted();

            Targets.Remove(collision.gameObject);
        }
    }

    
}
