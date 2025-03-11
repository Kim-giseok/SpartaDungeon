using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour, IInteractable
{
    [Header("Layzer info")]
    public Transform startP;
    public float senceDistance;

    bool activate = true;

    Ray ray = new();
    RaycastHit hit;

    [Header("Attack info")]
    public int atk;
    public float attackDelay;
    float lastAttackTime;

    void Update()
    {
#if UNITY_EDITOR
        Debug.DrawRay(startP.position, transform.forward * senceDistance, Color.red);
#endif
        ray.origin = startP.position;
        ray.direction = transform.forward;

        if (Physics.Raycast(ray, out hit, senceDistance) && activate)
        {
            if (!hit.collider.gameObject.TryGetComponent<IDamagable>(out var target)) return;

            if (Time.time - lastAttackTime >= attackDelay)
            {
                lastAttackTime = Time.time;
                target.TakePhysicalDamage(atk);
            }
        }
        else
            lastAttackTime = 0;
    }

    public string GetInteractPrompt()
    {
        return "레이저 장치\n" + (activate ? "정면에 서면 데미지를 입습니다. [E]키로 작동정지" : "작동하지 않고 있습니다. [E]키로 작동");
    }

    /// <summary>
    /// 상호작용으로 레이저를 켜고 끌 수 있습니다.
    /// </summary>
    public void OnInteract()
    {
        activate = !activate;
    }
}
