using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed; // 달리기 상태일 때 가산할 속도
    private Vector2 curMovementInput; // 현재 입력 값
    public float jumpPower;
    public LayerMask groundLayerMask; // 레이어 정보

    private Rigidbody rigi;

    public bool isRun;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 키보드 입력에 따라 이동방향을 결정합니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// 점프키 입력 시 지면에 닿아있으면 위로 뛰어오릅니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigi.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 지정된 방향으로 이동합니다.
    /// </summary>
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed + (isRun ? runSpeed : 0);
        dir.y = rigi.velocity.y;

        rigi.velocity = dir;
    }

    /// <summary>
    /// 지면에 닿아있는지 확인합니다.
    /// </summary>
    /// <returns>지면에 닿아있으면 true, 그렇지 않으면 false를 반환합니다.</returns>
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            isRun = true;
        if (context.phase == InputActionPhase.Canceled)
            isRun = false;  
    }
}
