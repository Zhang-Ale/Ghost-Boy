using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D col; 
    public bool manual; 
    public Vector2 bottomOffset;
    public Vector2 leftOffset, rightOffset; 
    public float checkRadius;
    public LayerMask groundLayer;

    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            rightOffset = new Vector2((col.bounds.size.x + col.offset.x) / 2, col.bounds.size.y/2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y); 
        }
    }

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
        Gizmos.DrawWireSphere((Vector2)transform.localPosition + bottomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.localPosition + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.localPosition + rightOffset, checkRadius);
    }
}
