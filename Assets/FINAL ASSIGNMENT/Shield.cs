using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] ParticleSystem particle;
    [SerializeField] float targetScaleFactor;
    [SerializeField] float duration;
    [SerializeField] Vector3 originalScale;
    Coroutine currentCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            particle.Play();
            transform.localScale = originalScale;

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(currentCoroutine == null) currentCoroutine = StartCoroutine(ShieldExpand());
            particle.Stop();
        }
    }


    IEnumerator ShieldExpand()
    {
        float elapsed = 0;
        Vector3 targetScale = originalScale * targetScaleFactor;
        while (true)
        {
            if(elapsed >= duration) { 
                transform.localScale = Vector3.zero;
                currentCoroutine = null;
                material.SetFloat("_ExpansionAlpha", 1);
                yield break; 
            }
            float t = elapsed / duration; // get % of duration completed

           

            float scaleFactor = t * t; // quadratic easing
            material.SetFloat("_ExpansionAlpha", 1 -scaleFactor * (3f - 2f * t));

            transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleFactor);

            yield return null;
            elapsed += Time.deltaTime;
        }
    }
}
