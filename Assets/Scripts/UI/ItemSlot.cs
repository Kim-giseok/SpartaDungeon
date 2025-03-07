using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;   // ������ ������

    public UIInventory inventory;

    public Image icon;
    public TextMeshProUGUI quatityText;  // ����ǥ�� Text
    public TextMeshProUGUI IndexText;  // ����ǥ�� Text
    private Outline outline;             // ���ý� Outline ǥ������ ������Ʈ

    int index;                    // �� ��° Slot���� index �Ҵ�
    public int Index
    {
        get => index;
        set
        {
            index = value;
            IndexText.text = index.ToString();
        }
    }
    public bool equipped;                // ��������
    public int quantity;                 // ����������

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// UI(���� �� ĭ) ������Ʈ�� ���� �Լ�
    /// �����۵����Ϳ��� �ʿ��� ������ �� UI�� ǥ��
    /// </summary>
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    /// <summary>
    /// UI(���� �� ĭ)�� ������ ���� �� UI�� ����ִ� �Լ�
    /// </summary>
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }
}
