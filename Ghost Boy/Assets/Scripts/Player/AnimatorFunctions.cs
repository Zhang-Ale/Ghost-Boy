using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem particleSystem1;
    [SerializeField] private int particleSystemAmount;
    
    void Start()
    {
        if(audioSource == null)
        {
            //audioSource = GetComponent<AudioSource>();
            //audioSource = Player.Instance.audioSource;
        }
    }

    public void PlaySound(AudioClip whichSound)
    {
        //GameManagerScript.Instance.audioSource.PlayDeadSound(whichSound);  
    }
    public void EmitParticles1()
    {
        particleSystem1.Emit(particleSystemAmount);
    }
    public void ScreenShake(float power)
    {
        //Player.Instance.cameraEffect.Shake(power, 1f);
    }
}
