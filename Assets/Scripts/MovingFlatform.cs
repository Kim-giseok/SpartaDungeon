using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFlatform : MonoBehaviour
{
    [Header("Move Info")]
    public float distance; //������ �Ÿ��Դϴ�. �ش� �Ÿ���ŭ ������ ��, �ٽ� ���ƿɴϴ�.
    public Vector3 direction; // ������ �����Դϴ�.
    public float speed; //�̵��ӵ��Դϴ�.

    Vector3 startP;

    public Player player;

    private void Awake()
    {
        direction = direction.normalized;
        startP = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Mathf.PingPong(speed*Time.time, distance);
        transform.position = startP + direction * movement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out player) && collision.gameObject.transform.position.y > transform.position.y)
            player.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
            player.transform.parent = null;
    }
}
