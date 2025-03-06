using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed; // �޸��� ������ �� ������ �ӵ�
    private Vector2 curMovementInput; // ���� �Է� ��
    public float jumpPower;
    public LayerMask groundLayerMask; // ���̾� ����

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook; // ���� �ּ� �þ߰�
    public float maxXLook; // ���� �ִ� �þ߰�
    private float camCurXRot;
    public float lookSensitivity; // ī�޶� �ΰ���

    private Vector2 mouseDelta; // ���콺 ��ȭ��

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
    /// ���콺 �������� �о�ɴϴ�.
    /// </summary>
    /// <param name="context"></param>
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Ű���� �Է¿� ���� �̵������� �����մϴ�.
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
    /// ����Ű �Է� �� ���鿡 ��������� ���� �پ�����ϴ�.
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
    /// ������ �������� �̵��մϴ�.
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
    /// ���콺�� �������� ���� ī�޶� �����Դϴ�.
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        if (IsWall())
        {
            //���� �پ��ִٸ� �÷��̾�� ȸ������ �ʽ��ϴ�.
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
    /// ���鿡 ����ִ��� Ȯ���մϴ�.
    /// </summary>
    /// <returns>���鿡 ��������� true, �׷��� ������ false�� ��ȯ�մϴ�.</returns>
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
    /// ������ Ű�� ������ �̵��ӵ��� �����մϴ�. Ű�� ���� �ٽ� ������� ���ư��ϴ�.
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
    /// ���� �پ����� Ȯ���մϴ�.
    /// </summary>
    bool IsWall()
    {
        Ray ray = new Ray(transform.position + transform.up * 0f, transform.forward);

        return !IsGrounded() && Physics.Raycast(ray, 0.3f, groundLayerMask);
    }
}
