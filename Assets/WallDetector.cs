using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private enum GroundDetection { wall, ground };
    [SerializeField] private float radius;
    [SerializeField] private LayerMask ground;
    [SerializeField] private PlayerMovementControler player;
    [SerializeField] private GroundDetection groundDetection;
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void Update()
    {
        switch (groundDetection)
        {
            case GroundDetection.wall:
                player.wall = Physics2D.OverlapCircle(transform.position, radius, ground);
                break;
            case GroundDetection.ground:
                player.ground = Physics2D.OverlapCircle(transform.position, radius, ground);
                break;
        }
        
    }
}
