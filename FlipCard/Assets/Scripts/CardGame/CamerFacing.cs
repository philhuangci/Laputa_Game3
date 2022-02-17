using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFacing : MonoBehaviour
{

    private Camera refCamer;
    private bool reverFace = false;
    public static CamerFacing Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (refCamer == null)
            refCamer = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = refCamer.transform.forward;
        this.transform.rotation = refCamer.transform.rotation;
    }
}
