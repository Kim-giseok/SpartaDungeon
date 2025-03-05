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
    SPEED
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
    public bool isTimlimit;
    public float bufTime;
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