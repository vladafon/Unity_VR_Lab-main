using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class TargetController : MonoBehaviour
{

    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;

    private Vector3 _initPos;

    [SerializeField]
    private float _deadY = -10;

    public void Shooted()
    {
        _renderer.material.color = Color.green;
    }
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _initPos = transform.localPosition;
        EventManager.onRestart += ResetObject;
    }

    void Update()
    {
        if (transform.position.y < _deadY)
        {
            EventManager.RaiseRestart();
        }
    }

    private void ResetObject()
    {

        _rigidbody.velocity = Vector3.zero;

        transform.localPosition = _initPos;
        transform.rotation = Quaternion.identity;

        _renderer.material.color = Color.gray;
    }
}
