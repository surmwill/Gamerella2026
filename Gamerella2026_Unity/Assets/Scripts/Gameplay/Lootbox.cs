using System;
using UnityEngine;

public class Lootbox : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collided");
    }
}
