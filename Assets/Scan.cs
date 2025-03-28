using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan : MonoBehaviour
{
    [SerializeField] Material scanMaterial;
    [SerializeField] float endDistance = 3000;
    [SerializeField] float startDistance;
    [SerializeField] float currentDistance;
    [SerializeField] float scanDuration = 5;

    [SerializeField] float scanThickness = 50;
   
    private static readonly int minLine = Shader.PropertyToID("_MinDist");
    private static readonly int maxLine = Shader.PropertyToID("_MaxDist");
    Coroutine scan;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(scan == null) scan = StartCoroutine(StartScan());
        }
    }


    IEnumerator StartScan()
    {
        currentDistance = startDistance;
        scanMaterial.SetFloat(minLine, startDistance - scanThickness);
        scanMaterial.SetFloat(maxLine, startDistance);
        float speed = (endDistance - startDistance) / scanDuration;
        while (currentDistance <= endDistance+scanThickness)
        {
            scanMaterial.SetFloat(minLine, scanMaterial.GetFloat(minLine) + (speed * Time.deltaTime*0.85f) );
            scanMaterial.SetFloat(maxLine, scanMaterial.GetFloat(maxLine) + (speed *Time.deltaTime) );
            currentDistance += speed * Time.deltaTime;
            yield return null;
        }

        scanMaterial.SetFloat(maxLine, endDistance);
        scanMaterial.SetFloat(minLine, endDistance); // set to same value- effectively disables it.
        scan = null;

    }
    
}
