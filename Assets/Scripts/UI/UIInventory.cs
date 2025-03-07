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
    public Transform dropPosition;      // item ���� �� �ʿ��� ��ġ

    [Header("Selected Item")]           // ������ ������ ������ ���� ǥ�� ���� UI
    ItemData selectedItem;
    int selectedItemIndex;


    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;

        // Action ȣ�� �� �ʿ��� �Լ� ���
        CharacterManager.Instance.Player.addItem += AddItem;  // ������ �Ĺ� ��

        // Inventory UI �ʱ�ȭ ������
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
        //ItemObject �������� Player�� �Ѱ��� ������ ������ ��
        ItemData data = CharacterManager.Instance.Player.itemData;

        // ������ ���� �� �ִ� �������̶��
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

        // �� ���� ã��
        ItemSlot emptySlot = GetEmptySlot();

        // �� ������ �ִٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // �� ���� ���� ���� ��
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    // UI ���� ���ΰ�ħ
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ���Կ� ������ ������ �ִٸ�
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

    // ������ ���� �� �ִ� �������� ���� ã�Ƽ� return
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

    // ������ item ������ ����ִ� ���� return
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

    // ������ ������ (������ �Ű������� ���� �����Ϳ� �ش��ϴ� ������ ����)
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, dropPosition.rotation);
    }
}
