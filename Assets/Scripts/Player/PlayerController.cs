using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Animator _playerAnimator;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed = 0.4f;

    private Rigidbody _playerRigidbody;

    private DynamicJoystick _joyStick;

    private Quaternion _lastRotation;

    private void Awake()
    {
        _joyStick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }



    void Update()
    {

        if (_joyStick.move)
        {

            _playerAnimator.SetBool("Move", true);

            Vector3 direction = _joyStick.Vertical * Vector3.forward + _joyStick.Horizontal * Vector3.right;
            if (direction != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);
            }
            transform.position += direction * _movementSpeed * Time.deltaTime;
         



        }
        else
        {
            _playerAnimator.SetBool("Move", false);

        }



    }




}
