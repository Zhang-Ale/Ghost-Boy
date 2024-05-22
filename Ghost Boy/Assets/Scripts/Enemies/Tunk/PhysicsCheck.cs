using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public Vector2 bottomOffset;
    public Vector2 leftOffset, rightOffset; 
    public float checkRadius;
    public LayerMask groundLayer;

    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    void Update()
    {
        Check(); 
    }

    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer); 

        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);


        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
    }
}
