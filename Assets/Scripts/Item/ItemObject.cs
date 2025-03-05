using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// 인터랙션 가능한 객체에 상속할 인터페이스
public interface IInteractable
{
    public string GetInteractPrompt();  // UI에 표시할 정보
    public void OnInteract();           // 인터랙션 호출
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
                CharacterManager.Instance.Player.condition.ApplyBuf(data);
                break;
            case ItemType.EQUIPABLE:
                break;
        }
        Destroy(gameObject);
    }
}
