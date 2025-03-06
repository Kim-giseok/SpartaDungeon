using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyFlatform : MonoBehaviour, IInteractable
{
    Vector3 originP;
    Quaternion originR;

    public float flyPower;
    bool flyReady = true;

    float flyStartTime;
    public float returnTime;

    Rigidbody rigi;

    public string GetInteractPrompt()
    {
        return "발사대\n[E]키로 발사";
    }

    public void OnInteract()
    {
        rigi.isKinematic = false;
        flyReady = false;
        rigi.AddForce(transform.up * flyPower, ForceMode.Impulse);
        flyStartTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        originP = transform.position;
        originR = transform.rotation;
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!flyReady && Time.time - flyStartTime >= returnTime)
        {
            rigi.velocity = Vector3.zero;
            rigi.isKinematic = true;
            flyReady = true;
        }

        if (flyReady)
        {
            transform.position = originP;
            transform.rotation = originR;
        }
    }
}
