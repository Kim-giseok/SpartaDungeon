using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̱��� ����
public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // �Ҵ���� �ʾ��� ��, �ܺο��� CharacterManager.Instance �� �����ϴ� ���
                // ���� ������Ʈ�� ������ְ� CharacterManager ��ũ��Ʈ�� AddComponent
                _instance = new GameObject("CharacerManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    // ���߿� ������ ��츦 ����Ͽ� ����(_player)�� ����(Player)�� ����
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
