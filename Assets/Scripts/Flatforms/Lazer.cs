using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour, IInteractable
{
    [Header("Layzer info")]
    public Transform startP;
    public float senceDistance;

    public LayerMask playerL;
    bool activate = true;

    Ray ray = new();
    RaycastHit hit;

    [Header("Attack info")]
    public float atk;
    public float attackDelay;
    float lastAttackTime;

    void Update()
    {
        Debug.DrawRay(startP.position, transform.forward * senceDistance, Color.red);
        ray.origin = startP.position;
        ray.direction = transform.forward;

        if (Physics.Raycast(ray, out hit, senceDistance, playerL) && activate)
        {
            if (Time.time - lastAttackTime >= attackDelay)
            {
                lastAttackTime = Time.time;
                Debug.Log("공격당함");
            }
        }
        else
            lastAttackTime = 0;
    }

    public string GetInteractPrompt()
    {
        return "레이저 장치\n" + (activate ? "정면에 서면 데미지를 입습니다. [E]키로 작동정지" : "작동하지 않고 있습니다. [E]키로 작동");
    }

    public void OnInteract()
    {
        activate = !activate;
    }
}
