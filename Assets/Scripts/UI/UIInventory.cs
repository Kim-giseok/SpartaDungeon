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

    int curEquipIndex;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        // Action 호출 시 필요한 함수 등록
        CharacterManager.Instance.Player.addItem += AddItem;  // 아이템 파밍 시
        controller.selectItem = SelectItem;

        // Inventory UI 초기화 로직들
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].Index = i + 1;
            slots[i].inventory = this;
            slots[i].Clear();
        }
    }

    /// <summary>
    /// 상호작용한 아이템을 슬롯에 추가합니다.
    /// </summary>
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

    /// <summary>
    /// UI 정보 새로고침
    /// </summary>
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

    /// <summary>
    /// 여러개 가질 수 있는 아이템의 정보 찾아서 return
    /// </summary>
    /// <param name="data"></param>
    /// <returns>아이템을 넣을 슬롯입니다.</returns>
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

    /// <summary>
    /// 슬롯의 item 정보가 비어있는 정보 return
    /// </summary>
    /// <returns>아이템을 넣을 슬롯입니다.</returns>
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

    /// <summary>
    /// 아이템 버리기 (실제론 매개변수로 들어온 데이터에 해당하는 아이템 생성)
    /// </summary>
    /// <param name="data"></param>
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, dropPosition.rotation);
    }

    /// <summary>
    /// 선택한 슬롯에 있는 아이템을 사용합니다.
    /// </summary>
    /// <param name="index"></param>
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        switch (selectedItem.type)
        {
            case ItemType.CONSUMABLE:
                condition.ApplyBuf(selectedItem);
                RemoveSelctedItem();
                break;
            case ItemType.EQUIPABLE:
                Equip();
                break;
        }
    }

    /// <summary>
    /// 선택된 슬롯의 아이템을 하나 줄입니다. 갯수가 0이 되면 슬롯을 비웁니다.
    /// </summary>
    void RemoveSelctedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
                //UnEquip(selectedItemIndex);
            }

            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
        }
        UpdateUI();
    }

    /// <summary>
    /// 선택된 아이템을 장착/장착해제합니다.
    /// </summary>
    void Equip()
    {
        //이미 장착중인 장비라면 장착해제
        if (slots[selectedItemIndex].equipped)
        {
            UnEquip(selectedItemIndex);
            return;
        }

        //장착중이지 않을 경우
        //장착중인 장비를 장착해제하고
        if (slots[curEquipIndex].equipped)
            UnEquip(curEquipIndex);
        //선택한 장비를 장착
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();
    }
}
