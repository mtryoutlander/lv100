using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    private enum LedgeDetectorState { top, bottom};
    [SerializeField]private float radius;
    [SerializeField]private LayerMask ledge;
    [SerializeField]private PlayerMovementControler player;
    [SerializeField] private LedgeDetectorState ledgeHight;
    private bool touchingWall;


    private void Update()
    {
        switch (ledgeHight)
        {
            case LedgeDetectorState.top:
                player.topLedge = Physics2D.OverlapCircle(transform.position, radius, ledge);
                break;
            case LedgeDetectorState.bottom:
                player.bottomLedge = Physics2D.OverlapCircle(transform.position, radius, ledge);
                break;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
