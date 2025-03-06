using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFlatform : MonoBehaviour, IInteractable
{
    [Header("Move Info")]
    public float distance; //전진할 거리입니다. 해당 거리만큼 전진한 후, 다시 돌아옵니다.
    public Vector3 direction; // 전진할 방향입니다.
    public float speed; //이동속도입니다.

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
        float movement = Mathf.PingPong(speed * Time.time, distance);
        transform.position = startP + direction * movement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out player) && collision.gameObject.transform.position.y - transform.position.y > 0.2f)
            player.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (player != null && collision.gameObject == player.gameObject)
            player.transform.parent = null;
    }

    public string GetInteractPrompt()
    {
        return "이동발판\n위에 올라타면 옮겨줍니다.";
    }

    public void OnInteract()
    {
    }
}
