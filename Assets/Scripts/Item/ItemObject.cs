using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// ���ͷ��� ������ ��ü�� ����� �������̽�
public interface IInteractable
{
    public string GetInteractPrompt();  // UI�� ǥ���� ����
    public void OnInteract();           // ���ͷ��� ȣ��
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    /// <summary>
    /// �������� ��ȣ�ۿ��Դϴ�. ��湰 �ܿ��� ȹ���մϴ�.
    /// </summary>
    public void OnInteract()
    {
        if (data.type == ItemType.RESOURCE)
            return;

        //Player ��ũ��Ʈ�� ��ȣ�ۿ� ������ data �ѱ��.
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
