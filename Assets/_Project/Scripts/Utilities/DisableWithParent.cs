using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithParent : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
