using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

// UI를 참조할 수 있는 PlayerCondition
// 외부에서 능력치 변경 기능은 이곳을 통해서 호출. 내부적으로 UI 업데이트 수행.
public class PlayerCondition : MonoBehaviour, IDamagable
{

    public UICondition uiCondition;

    Condition health => uiCondition.health;
    int invincible;

    Condition stamina => uiCondition.stamina;
    public bool IsRun => CharacterManager.Instance.Player.controller.IsRun && !grogy;
    bool grogy = false;

    int dJumpCount; //더블점프 가능 횟수
    public bool DJumpable => dJumpCount >= 1;

    public event Action onTakeDamage; // Damage 받을 때 호출할 Action

    private void Update()
    {
        if (stamina.curValue <= 0)
            grogy = true;
        if (stamina.curValue >= stamina.maxValue)
            grogy = false;

        if (IsRun)
            stamina.Subtract(stamina.passiveValue * Time.deltaTime);
        else
            stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (health.curValue <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    /// <summary>
    /// 데미지 받을 때 필요한 로직 작성 (health 감소, 데미지 Action 호출)
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakePhysicalDamage(int damageAmount)
    {
        if (invincible > 0) return;
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }

    /// <summary>
    /// 플레이어의 스피드를 변화시킵니다.
    /// </summary>
    /// <param name="speedAmount">변화시킬 수치입니다. 양수면 증가, 음수면 감소합니다.</param>
    public void ChangeSpeed(float speedAmount)
    {
        CharacterManager.Instance.Player.controller.moveSpeed += speedAmount;
    }

    public void ChangeDJumpCount(float djumpCount)
    {
        dJumpCount += (int)djumpCount;
    }

    public void ChangeInvincible(float invin)
    {
        invincible += (int)invin;
    }

    /// <summary>
    /// 아이템의 버프수치들을 적용합니다. 아이템은 사용 후 사라지기 때문에 코루틴 유지를 위해 여기서 호출합니다.
    /// </summary>
    /// <param name="data">아이템이 가지고 있는 정보입니다.</param>
    public void ApplyBuf(ItemData data)
    {
        foreach (var consumable in data.consumables)
        {
            StartCoroutine(consumable.ApplyBuf(this));
        }
    }
}
