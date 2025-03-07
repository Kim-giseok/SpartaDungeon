using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    PlayerController controller;
    PlayerCondition condition;

    public ItemSlot[] slots;
    public Transform slotPanel;
    public Transform dropPosition;      // item 버릴 때 필요한 위치

    [Header("Selected Item")]           // 선택한 슬롯의 아이템 정보 표시 위한 UI
    ItemData selectedItem;
    int selectedItemIndex;


    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        // Action 호출 시 필요한 함수 등록
        CharacterManager.Instance.Player.addItem += AddItem;  // 아이템 파밍 시

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

    public void AddItem()
    {
        //ItemObject 로직에서 Player에 넘겨준 정보를 가지고 옴
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 여러개 가질 수 있는 아이템이라면
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // 빈 슬롯 찾기
        ItemSlot emptySlot = GetEmptySlot();

        // 빈 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // 빈 슬롯 마저 없을 때
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    // UI 정보 새로고침
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯에 아이템 정보가 있다면
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    // 여러개 가질 수 있는 아이템의 정보 찾아서 return
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 슬롯의 item 정보가 비어있는 정보 return
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 아이템 버리기 (실제론 매개변수로 들어온 데이터에 해당하는 아이템 생성)
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, dropPosition.rotation);
    }
}
