using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffects : MonoBehaviour
{
    private float lengh, startpos;
    public GameObject cam;
    public float parallaxEffects;

    void Start()
    {
        startpos = transform.position.x;
        lengh = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float dist = (cam.transform.position.x * parallaxEffects);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}
