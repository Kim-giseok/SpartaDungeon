using System;
using System.Collections;
using UnityEngine;

public enum ItemType
{
    RESOURCE,
    EQUIPABLE,
    CONSUMABLE
}

public enum ConsumableType
{
    HEALTH,
    SPEED,
    DJUMP,
    INVINCIBLE
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
    public bool isTimlimit;
    public float bufTime;

    /// <summary>
    /// 아이템의 버프내용을 적용합니다. 중복적용을 위해서 여기에 선언해두었습니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ApplyBuf(PlayerCondition condition)
    {
        Action<float> action = null;
        switch (type)
        {
            case ConsumableType.HEALTH:
                action = condition.Heal;
                break;
            case ConsumableType.SPEED:
                action = condition.ChangeSpeed;
                break;
            case ConsumableType.DJUMP:
                action = condition.ChangeDJumpCount;
                break;
            case ConsumableType.INVINCIBLE:
                action = condition.ChangeInvincible;
                break;
        }
        action?.Invoke(value);

        if (isTimlimit)
        {
            yield return new WaitForSeconds(bufTime);
            action?.Invoke(-value);
        }
    }
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}