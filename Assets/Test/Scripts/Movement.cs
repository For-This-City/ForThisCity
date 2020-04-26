using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _playerRb;
    private Animator _playerAnim;
    
    private Vector3 _moveDir;
    private float _xMoveDir;
    private float _zMoveDir;
    
    [SerializeField] private float speed = 20f;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _xMoveDir = 0f;
        _zMoveDir = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _xMoveDir = 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _xMoveDir = -1f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _zMoveDir = 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _zMoveDir = -1f;
        }
        
        _moveDir = new Vector3(_xMoveDir, 0, _zMoveDir).normalized;
        
        // Animating the player
        _playerAnim.SetFloat("Horizontal", _moveDir.x);
        _playerAnim.SetFloat("Vertical", _moveDir.z);
        _playerAnim.SetFloat("Magnitude", _moveDir.magnitude);
    }

    private void FixedUpdate()
    {
        _playerRb.velocity = _moveDir * speed;
    }
}
