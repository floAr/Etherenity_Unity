using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnimator : MonoBehaviour {
    public GameObject Upper;
    public GameObject Lower;

    public bool DoRotation;
    public bool DoPulse;

    [Range(0,1)]
    public float RotationSpeed;
    public Vector3 RotationAxis;

    public float PulseDistanceScale = 10000f;
    public float PulseTimeeScale = 500f;

    Coroutine Rotate;
    Coroutine Pulse;


    public ParticleSystem PlexSystem;

    private void Start()
    {
        PlexSystem.Play();
    }

    // Update is called once per frame
    void Update () {
        if (DoPulse && Pulse == null)
            Pulse= StartCoroutine(Cor_Pulse());
        if (!DoPulse && Pulse!=null)
        {
            StopCoroutine(Pulse);
            Pulse = null;
        }


        if (DoRotation && Rotate == null)
            Rotate= StartCoroutine(Cor_Rotate());
        if (!DoRotation && Rotate != null)
        {
            StopCoroutine(Rotate);
            Rotate = null;
        }
    }

    private IEnumerator Cor_Rotate()
    {
        while (true)
        {
            this.transform.Rotate(RotationAxis, RotationSpeed);
            PlexSystem.transform.Rotate(Vector3.up, RotationSpeed/-8f);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Cor_Pulse()
    {
        float count = 0;
        while (true)
        {
            Upper.transform.position = Upper.transform.position + Vector3.up * Mathf.Sin(count / PulseTimeeScale) / PulseDistanceScale;
            Lower.transform.position = Lower.transform.position - Vector3.up * Mathf.Sin(count / PulseTimeeScale) / PulseDistanceScale;
            count += 1;
            yield return new WaitForEndOfFrame();
        }
    }
}
