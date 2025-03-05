using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFlatform : MonoBehaviour
{
    public float jumpPower;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.transform.position.y > transform.position.y)
        {
            var rigi = collision.gameObject.GetComponent<Rigidbody>();
            rigi.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
