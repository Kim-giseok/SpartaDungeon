using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFlatform : MonoBehaviour, IInteractable
{
    public float jumpPower;

    public string GetInteractPrompt()
    {
        return "점프대\n밟으면 위로 뛰어오릅니다.";
    }

    public void OnInteract()
    {
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.transform.position.y - transform.position.y > 0.2f)
        {
            var rigi = collision.gameObject.GetComponent<Rigidbody>();
            rigi.velocity = new Vector3(rigi.velocity.x, 0, rigi.velocity.z);
            rigi.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
