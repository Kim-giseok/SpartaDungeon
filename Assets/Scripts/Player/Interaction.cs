using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;    // ��ȣ�ۿ� ������Ʈ üũ �ð�
    private float lastCheckTime;       // ������ ��ȣ�ۿ� üũ �ð�
    public float maxCheckDistance;     // �ִ� üũ �Ÿ�
    public LayerMask layerMask;

    public GameObject curInteractGameObject;  // ���� ��ȣ�ۿ� ���ӿ�����Ʈ
    private IInteractable curInteractable;    // ���� ��ȣ�ۿ� �������̽�

    public TextMeshProUGUI promptText;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = new Ray(transform.position, Vector3.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// �������� ������ ǥ���մϴ�.
    /// </summary>
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    /// <summary>
    /// ��ȣ�ۿ� Ű�� ������ �����ִ� �����۰� ��ȣ�ۿ��մϴ�.
    /// </summary>
    /// <param name="context"></param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
