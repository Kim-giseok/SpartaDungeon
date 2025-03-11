using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;    // 상호작용 오브젝트 체크 시간
    private float lastCheckTime;       // 마지막 상호작용 체크 시간
    public float maxCheckDistance;     // 최대 체크 거리
    public LayerMask layerMask;

    public GameObject curInteractGameObject;  // 현재 상호작용 게임오브젝트
    private IInteractable curInteractable;    // 현재 상호작용 인터페이스
    public Transform dropPosition; //아이템 버릴 위치

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

            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
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
    /// 아이템의 정보를 표시합니다.
    /// </summary>
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    /// <summary>
    /// 상호작용 키를 누르면 보고있는 아이템과 상호작용합니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            //아이템을 획득할 수 없다면 제자리에 다시 놓아두기 위해 아이템의 위치와 회전값을 기억해둡니다.
            Vector3 originP = dropPosition.position;
            Quaternion originR = dropPosition.rotation;
            dropPosition.SetPositionAndRotation(curInteractGameObject.transform.position, curInteractGameObject.transform.rotation);

            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);

            dropPosition.SetPositionAndRotation(originP, originR);
        }
    }
}
