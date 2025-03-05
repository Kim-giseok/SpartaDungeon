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

    public void OnInteract()
    {
        switch (data.type)
        {
            case ItemType.RESOURCE:
                return;
            case ItemType.CONSUMABLE:
                ItemConsume();
                break;
            case ItemType.EQUIPABLE:
                break;
        }
    }

    void ItemConsume()
    {
        for (int i = 0; i < data.consumables.Length; i++)
        {
            StartCoroutine(data.consumables[i].ApplyBuf(0));
        }
    }
}
