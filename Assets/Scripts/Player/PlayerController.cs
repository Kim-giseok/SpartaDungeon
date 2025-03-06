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

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook; // 세로 최소 시야각
    public float maxXLook; // 세로 최대 시야각
    private float camCurXRot;
    public float lookSensitivity; // 카메라 민감도

    private Vector2 mouseDelta; // 마우스 변화값

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigi;

    public bool isRun;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
        Debug.DrawRay(transform.position + transform.up * 0.5f, transform.forward * 0.3f, Color.red);
    }

    /// <summary>
    /// 마우스 움직임을 읽어옵니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
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
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
                rigi.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            else if (IsWall())
                rigi.AddForce(transform.forward * -jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 지정된 방향으로 이동합니다.
    /// </summary>
    private void Move()
    {
        float totalSpeed = CharacterManager.Instance.Player.condition.isRun ? moveSpeed + runSpeed : moveSpeed;

        Vector3 dir;
        if (IsWall())
        {
            rigi.useGravity = false;
            dir = transform.up * curMovementInput.y + transform.right * curMovementInput.x;
            dir = dir.normalized * totalSpeed;
            dir += transform.forward;

            rigi.velocity = dir;
        }
        else
        {
            rigi.useGravity = true;
            dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
            dir = dir.normalized * totalSpeed;
            dir.y = rigi.velocity.y;

            if (IsGrounded())
                rigi.velocity = dir;
            else
                rigi.AddForce(dir - rigi.velocity, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// 마우스에 움직임을 따라서 카메라가 움직입니다.
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        if (IsWall())
        {
            //벽에 붙어있다면 플레이어는 회전하지 않습니다.
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, cameraContainer.localEulerAngles.y + mouseDelta.x * lookSensitivity, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, cameraContainer.eulerAngles.y, 0);
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
        }
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

    /// <summary>
    /// 지정된 키를 누르면 이동속도가 증가합니다. 키를 떼면 다시 원래대로 돌아갑니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            isRun = true;
        if (context.phase == InputActionPhase.Canceled)
            isRun = false;
    }

    /// <summary>
    /// 벽에 붙었는지 확인합니다.
    /// </summary>
    bool IsWall()
    {
        Ray ray = new Ray(transform.position + transform.up * 0f, transform.forward);

        return !IsGrounded() && Physics.Raycast(ray, 0.3f, groundLayerMask);
    }
}
