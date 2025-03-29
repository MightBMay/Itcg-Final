using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Arrow : MonoBehaviour
{
    [SerializeField] ParticleSystem head;
    [SerializeField] ParticleSystem ring;
    [SerializeField] ParticleSystem shaft;
    [SerializeField] VisualEffect trail;

    [SerializeField] float chargeDuration;
    Coroutine currentCoroutine;
    //[SerializeField]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(currentCoroutine == null) { currentCoroutine = StartCoroutine(DrawArrow()); }
        }
    }

    IEnumerator DrawArrow()
    {
        head.Play(); // start head particles
        shaft.Play(); // start shaft particles

        yield return new WaitForSeconds(chargeDuration);// wait for arrow to be drawn back

        ring.Play(); // play ring particle
        trail.SendEvent("START"); // start emitting trails
        yield return new WaitForSeconds(0.250f); // wait a little bit, then stop head particles.
      
        head.Stop();
       
        yield return new WaitUntil(()=> Input.GetMouseButtonUp(0)); // when you release the arrow, stop all particles.

        ring.Stop();
        shaft.Stop();
        trail.SendEvent("STOP");

        currentCoroutine = null; // used to make sure this code is only running once at any given time.

    }
}
