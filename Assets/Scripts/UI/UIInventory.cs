using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    PlayerController controller;
    PlayerCondition condition;

    public ItemSlot[] slots;
    public Transform slotPanel;

    [Header("Selected Item")]           // 선택한 슬롯의 아이템 정보 표시 위한 UI
    ItemData selectedItem;
    int selectedItemIndex;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        // Inventory UI 초기화 로직들
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].Index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }
    }
}
