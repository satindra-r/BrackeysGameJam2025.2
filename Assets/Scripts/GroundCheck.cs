using System;
using UnityEngine;

using BrackeysGameJam;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Movement movement;

    private void OnCollisionEnter2D(Collision2D other)
    {
        movement.OnGroundEnter();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        movement.OnGroundExit();
    }
}
