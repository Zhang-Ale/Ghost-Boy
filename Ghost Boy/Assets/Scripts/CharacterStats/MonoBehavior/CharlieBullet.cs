using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharlieBullet : MonoBehaviour
{
    Transform parent;
    public Transform hitBox; 
    public float pV = 20;
    public float aMin = 3;
    public float aMax = 5;
    public float alpha = 0;
    public float omega = 180;
    public float sign = 1;
    public float a;
    PlayerAttack PA;
    public LayerMask enemyLayers;
    bool hitEnemy;
    private ParticleSystem ps;
    private Transform collidedObject;

    void Start()
    {
        parent = transform.parent.transform;
        a = Random.Range(aMin, aMax); 
        if(Random.Range(0, 100) > 50)
        {
            sign = -1; 
        }
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        PA = GameObject.Find("Player_prefab").GetComponent<PlayerAttack>(); 
        Vector2 direction = (hitBox.transform.position - parent.position).normalized;
        float distance = (hitBox.transform.position - parent.position).magnitude;

        if (!hitEnemy)
        {
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
        else
        {
            parent.position = collidedObject.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayers) != 0)
        {
            hitEnemy = true;
            collidedObject = collision.transform;
            StartCoroutine(ChangeParticleColorAndFade());
        }
    }

    private IEnumerator ChangeParticleColorAndFade()
    {
        ParticleSystem.MainModule mainModule = ps.main;
        Color initialColor = mainModule.startColor.color;
        Color targetColor = Color.white;
        float duration = 1f; // Duration to change color to white
        float fadeDuration = 1f; // Duration to fade out
        float elapsedTime = 0f;

        // Gradually change the color to white
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            mainModule.startColor = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        // Reset elapsed time for fading
        elapsedTime = 0f;

        // Gradually fade out the particles
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            Color fadedColor = Color.Lerp(targetColor, Color.clear, t);
            mainModule.startColor = fadedColor;
            yield return null;
        }

        // Ensure the particles are completely faded out
        mainModule.startColor = Color.clear;
    }
}
