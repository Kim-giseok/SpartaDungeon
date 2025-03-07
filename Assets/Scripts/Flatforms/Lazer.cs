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
                Debug.Log("���ݴ���");
            }
        }
        else
            lastAttackTime = 0;
    }

    public string GetInteractPrompt()
    {
        return "������ ��ġ\n" + (activate ? "���鿡 ���� �������� �Խ��ϴ�. [E]Ű�� �۵�����" : "�۵����� �ʰ� �ֽ��ϴ�. [E]Ű�� �۵�");
    }

    public void OnInteract()
    {
        activate = !activate;
    }
}
