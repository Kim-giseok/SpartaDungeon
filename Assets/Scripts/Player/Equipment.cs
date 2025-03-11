using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    PlayerController controller;
    PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
    }

    /// <summary>
    /// 장비를 장착하고 그 능력치를 얻습니다.
    /// </summary>
    /// <param name="data"></param>
    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
        curEquip.ApplyEBuf(condition);
    }

    /// <summary>
    /// 장비를 장착해제하고 그 능력치를 잃습니다.
    /// </summary>
    public void UnEquip()
    {
        if (curEquip != null)
        {
            curEquip.DeflyEBuf();
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}
