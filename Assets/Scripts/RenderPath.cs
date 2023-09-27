using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPath
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LineRenderer RenderALine(Vector3 a, Vector3 b) {
        GameObject myLine = new GameObject();
         myLine.transform.position = a;
         myLine.AddComponent<LineRenderer>();
         LineRenderer lr = myLine.GetComponent<LineRenderer>();
        // lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        // //lr.SetColors(color, color);
        // lr.startWidth = 2f; 
        // lr.endWidth = 2f; 
        // lr.SetPosition(0, a);
        // lr.SetPosition(1, b);
        //GameObject.Destroy(myLine, duration);

         lr.gameObject.SetActive(true);
        lr.positionCount = 2; 
         lr.SetPosition(0, a);
        lr.SetPosition(1, b);
        lr.startWidth = 0.15f; 
        lr.endWidth = 0.15f; 
        //lr.transform.Rotate(new Vector3(0, 1f, 0), Space.World); 

        return lr; 
    }
}
