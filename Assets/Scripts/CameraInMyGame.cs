using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInMyGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Camera m_Camera = GetComponent<Camera>();
       m_Camera.transform.position.Set(5, 5,5);
       m_Camera.transform.rotation.Set(90, 0, 0, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
