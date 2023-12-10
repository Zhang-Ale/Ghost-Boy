using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharlieBullet : MonoBehaviour
{
    Transform parent;
    [SerializeField] Transform hitBox; 
    public float pV = 20;
    public float aMin = 3;
    public float aMax = 5;
    public float alpha = 0;
    public float omega = 180;
    public float sign = 1;
    public float a;

    void Start()
    {
        parent = transform.parent.transform;
        a = Random.Range(aMin, aMax); 
        if(Random.Range(0, 100) > 50)
        {
            sign = -1; 
        }
    }
    
    void Update()
    {
        hitBox = GameObject.Find("Player_prefab").transform.GetChild(3).transform;
        Vector2 direction = (hitBox.transform.position - parent.position).normalized;
            float distance = (hitBox.transform.position - parent.position).magnitude;

            if (distance < pV * Time.deltaTime)
            {
                parent.position = hitBox.transform.position;
            }
            else
            {
                parent.Translate(pV * Time.deltaTime * direction);
            }

            direction = Quaternion.Euler(0, 0, sign * 90) * direction;
            transform.localPosition = Mathf.Min(a, distance / 2) * Mathf.Sin(alpha) * direction;
            alpha += omega * Time.deltaTime * Mathf.PI / 180; 
    }
}
