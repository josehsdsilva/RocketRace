
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnStart : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool activeOnStart;

    private void Awake()
    {
        gameObject.SetActive(activeOnStart);
    }
}
