using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;   // 아이템 데이터

    public UIInventory inventory;

    public Image icon;
    public TextMeshProUGUI quatityText;  // 수량표시 Text
    public TextMeshProUGUI IndexText;  // 수량표시 Text
    private Outline outline;             // 선택시 Outline 표시위한 컴포넌트

    int index;                    // 몇 번째 Slot인지 index 할당
    public int Index
    {
        get => index;
        set
        {
            index = value;
            IndexText.text = index.ToString();
        }
    }
    public bool equipped;                // 장착여부
    public int quantity;                 // 수량데이터

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// UI(슬롯 한 칸) 업데이트를 위한 함수
    /// 아이템데이터에서 필요한 정보를 각 UI에 표시
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
    /// UI(슬롯 한 칸)에 정보가 없을 때 UI를 비워주는 함수
    /// </summary>
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }
}
