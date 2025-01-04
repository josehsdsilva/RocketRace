using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledOnStart : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
