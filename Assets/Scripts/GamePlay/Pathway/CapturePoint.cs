using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    /// <summary>
    /// Raises the trigger enter2d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        EventManager.InvokeEvent("Captured", other.gameObject, null);
    }
}
