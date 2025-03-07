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
        return flyReady ? "발사대\n[E]키로 발사" : "발사된 발사대\n잠시 후 원래자리로 돌아갑니다.";
    }

    public void OnInteract()
    {
        if (!flyReady) return;
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
