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
    DJUMP
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
    public IEnumerator ApplyBuf()
    {
        Action<float> action = null;
        switch (type)
        {
            case ConsumableType.HEALTH:
                action = CharacterManager.Instance.Player.condition.Heal;
                break;
            case ConsumableType.SPEED:
                action = CharacterManager.Instance.Player.condition.ChangeSpeed;
                break;
            case ConsumableType.DJUMP:
                action = CharacterManager.Instance.Player.condition.ChangeDJumpCount;
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

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}