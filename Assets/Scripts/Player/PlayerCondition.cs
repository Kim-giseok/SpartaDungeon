using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

// UI�� ������ �� �ִ� PlayerCondition
// �ܺο��� �ɷ�ġ ���� ����� �̰��� ���ؼ� ȣ��. ���������� UI ������Ʈ ����.
public class PlayerCondition : MonoBehaviour, IDamagable
{

    public UICondition uiCondition;

    Condition health => uiCondition.health;
    int invincible;

    Condition stamina => uiCondition.stamina;
    public bool IsRun => CharacterManager.Instance.Player.controller.IsRun && !grogy;
    bool grogy = false;

    int dJumpCount; //�������� ���� Ƚ��
    public bool DJumpable => dJumpCount >= 1;

    public event Action onTakeDamage; // Damage ���� �� ȣ���� Action

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
        Debug.Log("�÷��̾ �׾���.");
    }

    /// <summary>
    /// ������ ���� �� �ʿ��� ���� �ۼ� (health ����, ������ Action ȣ��)
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakePhysicalDamage(int damageAmount)
    {
        if (invincible > 0) return;
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }

    /// <summary>
    /// �÷��̾��� ���ǵ带 ��ȭ��ŵ�ϴ�.
    /// </summary>
    /// <param name="speedAmount">��ȭ��ų ��ġ�Դϴ�. ����� ����, ������ �����մϴ�.</param>
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
    /// �������� ������ġ���� �����մϴ�. �������� ��� �� ������� ������ �ڷ�ƾ ������ ���� ���⼭ ȣ���մϴ�.
    /// </summary>
    /// <param name="data">�������� ������ �ִ� �����Դϴ�.</param>
    public void ApplyBuf(ItemData data)
    {
        foreach (var consumable in data.consumables)
        {
            StartCoroutine(consumable.ApplyBuf(this));
        }
    }
}
