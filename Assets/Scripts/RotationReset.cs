using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationReset : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
