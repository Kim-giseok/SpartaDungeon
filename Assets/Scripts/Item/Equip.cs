using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    Action<float> buf;

    public BufType bType;
    public float amount;

    public void ApplyEBuf(PlayerCondition condition)
    {
        switch(bType)
        {
            case BufType.SPEED:
                buf = condition.ChangeSpeed;
                break;
        }

        buf?.Invoke(amount);
    }

    public void DeflyEBuf()
    {
        buf?.Invoke(-amount);
    }
}
