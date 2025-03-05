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

    Coroutine[] coroutines;

    private void Start()
    {
        coroutines = new Coroutine[data.consumables.Length];
    }

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
        for (int i = 0; i < coroutines.Length; i++)
        {
            if (coroutines[i] == null)
                coroutines[i] = StartCoroutine(ApplyBuf(0));
        }
    }

    IEnumerator ApplyBuf(int idx)
    {
        Action<float> action = null;
        switch (data.consumables[idx].type)
        {
            case ConsumableType.HEALTH:
                action = CharacterManager.Instance.Player.condition.Heal;
                break;
            case ConsumableType.SPEED:
                action = CharacterManager.Instance.Player.condition.ChangeSpeed;
                break;
        }
        action?.Invoke(data.consumables[idx].value);

        if (data.consumables[idx].isTimlimit)
        {
            yield return new WaitForSeconds(data.consumables[idx].bufTime);
            action?.Invoke(-data.consumables[idx].value);
        }
        coroutines[idx] = null;
    }
}
