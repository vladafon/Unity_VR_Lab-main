using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GunController : MonoBehaviour
{
    [SerializeField]
    private float _shootingForce = 1500;


    [SerializeField]
    private int _bulletPoolSize = 10;

    [SerializeField]
    private int _bulletLifeTime = 10;


    [SerializeField]
    private PhysicMaterial _bulletMaterial;

    [SerializeField]
    private float _bulletDeadY = -10;


    [SerializeField]
    private List<GameObject> _targets;

    [SerializeField]
    private Vector3 _bulletScale = Vector3.one;


    private List<GameObject> _bullets;


    public void Shoot(Vector3 bulletPos, Vector3 direction)
    {
        Debug.Log(direction);
        var bullet = _bullets.FirstOrDefault(c => c.activeInHierarchy == false);

        if (bullet != null)
        {
            bulletPos += new Vector3(0, 0, 0);

            bullet.transform.position = bulletPos;

            bullet.SetActive(true);

            var rb = bullet.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            rb.AddForce(direction * _shootingForce * rb.mass);

            var bc = bullet.GetComponent<BulletController>();

            bc.StartLifeTimeCoroutine(_bulletLifeTime);

        }
    }

    void Start()
    {
        BulletPoolInit();

        Debug.Log($"Pool size {_bulletPoolSize}");

        EventManager.onRestart += ResetObject;
    }

    private void BulletPoolInit()
    {
        // Если инициализировать список при объявлении, можно полностью избежать следующей строки. Если список пустой, foreach не будет выполнен и не выдаст ошибку.
        if (_bullets != null && _bullets.Count > 0)
        {
            foreach(var bullet in _bullets)
            {
                Destroy(bullet);
            }
        }

        // можно также использовать _bullets.Clear()
        _bullets = new List<GameObject>();

        for (int i = 0; i < _bulletPoolSize; i++)
        {
            var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.localScale = _bulletScale;
            BulletInit(bullet);
            _bullets.Add(bullet);
        }
    }

    private void BulletInit(GameObject bullet)
    {
        if (!bullet.TryGetComponent(out BulletController bc))
        {
            bc = bullet.AddComponent<BulletController>();
        }

        bc.BulletControllerInit(_targets, _bulletDeadY, _bulletMaterial);
    }

    private void ResetObject()
    {
        BulletPoolInit();
    }
}
