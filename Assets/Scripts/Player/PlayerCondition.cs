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

    public float noHungerHealthDecay; // hunger�� 0�϶� ����� �� (value > 0)
    public event Action onTakeDamage; // Damage ���� �� ȣ���� Action

    private void Update()
    {
        if (health.curValue <= 0f)
        {
            Die();
        }
        else
            health.curValue -= Time.deltaTime;
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
}
