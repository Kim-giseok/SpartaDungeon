using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.position = transform.position + Vector3.up * 3f;
    }
}
