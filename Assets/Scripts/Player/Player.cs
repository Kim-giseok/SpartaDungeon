using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player�� ���õ� ����� ��Ƶδ� ��.
// �̰��� ���� ��ɿ� ���� ����.
public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    private void Awake()
    {
        // �̱���Ŵ����� Player�� ������ �� �ְ� �����͸� �ѱ��.
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
