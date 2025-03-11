using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    /// <summary>
    /// 오브젝트가 맵 밖으로 나가면 다시 맵 중앙으로 옮겨옵니다.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.position = transform.position + Vector3.up * 5f;
    }
}
