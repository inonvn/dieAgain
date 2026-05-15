using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerJoystick : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _Joystick;
/*    [SerializeField] private Animator _animator;*/

    [SerializeField] private float _moveSpeed;

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(_Joystick.Horizontal * _moveSpeed, _rigidbody.linearVelocity.y, _Joystick.Vertical * _moveSpeed);
        
        if(_Joystick.Horizontal != 0 || _Joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
        }
    }
}
