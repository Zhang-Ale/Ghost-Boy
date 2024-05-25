using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D col; 
    public bool manual; 
    public Vector2 bottomOffset, leftOffset, rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;

    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        if (manual)
        {
            rightOffset = new Vector2((col.bounds.size.x) / 2 + col.offset.x, col.bounds.size.y);
            leftOffset = new Vector2(-(col.bounds.size.x) / 2 + col.offset.x, col.bounds.size.y); 
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
