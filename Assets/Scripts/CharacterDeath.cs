using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeath : MonoBehaviour
{
    void Destroy()
    {
        Destroy(gameObject);
    }
}
