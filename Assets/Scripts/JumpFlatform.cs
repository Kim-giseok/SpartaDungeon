using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFlatform : MonoBehaviour
{
    public float jumpPower;

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.transform.position.y > transform.position.y)
        {
            var rigi = collision.gameObject.GetComponent<Rigidbody>();
            rigi.velocity = new Vector3(rigi.velocity.x,0,rigi.velocity.z);
            rigi.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
