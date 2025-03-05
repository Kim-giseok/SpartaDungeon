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

    public float noHungerHealthDecay; // hunger가 0일때 사용할 값 (value > 0)
    public event Action onTakeDamage; // Damage 받을 때 호출할 Action

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
        Debug.Log("플레이어가 죽었다.");
    }

    /// <summary>
    /// 데미지 받을 때 필요한 로직 작성 (health 감소, 데미지 Action 호출)
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}
