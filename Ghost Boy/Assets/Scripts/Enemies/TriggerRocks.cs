using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRocks : Enemy
{
    public int _rockLife = 1;
    public Transform _fallingPoint;
    public Transform _fallingPoint2;
    public GameObject _fallingRock;
    Rigidbody2D rb;
    public bool _fall = false;
    TriggerRocks trig;
    SpriteRenderer render;
    private float minimum = 0.0f;
    private float maximum = 1f;
    private float duration = 5.0f;
    private float startTime;
    public Collider2D coll;
    bool isFlipped;

    private void Start()
    {
        //rb = _fallingRock.GetComponent<Rigidbody2D>();
        //rb.bodyType = RigidbodyType2D.Kinematic;
        coll = GetComponentInChildren<BoxCollider2D>();
        render = this.GetComponent<SpriteRenderer>();
        rb = _fallingRock.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        trig = GameObject.FindGameObjectWithTag("TriggerRock").GetComponent<TriggerRocks>();
        startTime = Time.time;
        _life = 30f;
        damageType = DamageTypes.rock;
    }
    void Check()
    {
        if (render.flipX == false)
        {
            isFlipped = false;
        }
        if (render.flipX == true)
        {
            isFlipped = true;
        }
    }

    public void DestroyRock()
    {
        Check();
        _rockLife -= 1;
        Debug.Log("Hit");
        if (trig._rockLife == 0)
        {
            Debug.Log("Pls");
            float t = (Time.time - startTime) / duration;
            render.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(minimum, maximum, t));
            coll.isTrigger = true;
            
            Destroy(this.gameObject);
            _fall = true;
            StartCoroutine(RockSolid());
        }
    }

    IEnumerator RockSolid()
    {
        if (_fall == true)
        {
            if (isFlipped == false)
            {
                Instantiate(_fallingRock, new Vector2(_fallingPoint.position.x, _fallingPoint.position.y), Quaternion.identity);
                //rb.bodyType = RigidbodyType2D.Dynamic;
                yield return new WaitForSeconds(1f);
                //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                rb.isKinematic = true;
            }
            if (isFlipped == true)
            {
                Instantiate(_fallingRock, new Vector2(_fallingPoint2.position.x, _fallingPoint2.position.y), Quaternion.identity);
                //rb.bodyType = RigidbodyType2D.Dynamic;
                yield return new WaitForSeconds(1f);
                //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                rb.isKinematic = true;
            }
        }
    }
}
