using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendThrust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();
        renderer.SetBlendShapeWeight(0, 50 * Input.GetAxis("Throttle"));
        
    }
}
