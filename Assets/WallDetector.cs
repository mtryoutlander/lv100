using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask ground;
    [SerializeField] private PlayerMovementControler player;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void Update()
    {
        player.wall = Physics2D.OverlapCircle(transform.position, radius, ground);
        
    }
}
