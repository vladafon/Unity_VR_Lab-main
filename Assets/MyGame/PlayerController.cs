using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GunController _gun;

    //Make sure to attach these Buttons in the Inspector
    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Transform _handPos;

    [SerializeField]
    private int _maxScore = 3;

    private int _score;

    private bool _canShoot;

    private UnityEngine.XR.InputDevice _device;

    void Start()
    {
        _canShoot = true;
        _score = 0;

        EventManager.onShooted += AddScore;
        EventManager.onRestart += ResetObject;

        _restartButton.onClick.AddListener(OnRestartButtonClick);

        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

        foreach (var device in leftHandedControllers)
        {
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
            _device = device;
            break;
        }
    }

    void Update()
    {
        Fire();
    }

    void FireCallback(InputAction.CallbackContext context)
    {
        if (_canShoot && context.started)
        {
            Debug.Log("Fire!");
            _gun.Shoot(_handPos.position, _handPos.transform.forward);
        }
    }

    void Fire()
    {
        if (_canShoot 
                && _device.isValid
                && _device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerValue) 
                && triggerValue)
        {
            Debug.Log("Fire!");
            _gun.Shoot(_handPos.position, _handPos.transform.forward);
        }
    }

    private void AddScore()
    {
        Debug.Log("+1");
        _score++;
        Debug.Log(_score);

        if (_score >= _maxScore)
        {
            EventManager.RaiseGameOver();
            _canShoot = false;
        }
    }

    private void ResetObject()
    {
        gameObject.transform.rotation = Quaternion.identity;
        _score = 0;
        _canShoot = true;
    }

    void OnRestartButtonClick()
    {
        Debug.Log("Restart");
        EventManager.RaiseRestart();
    }

}
