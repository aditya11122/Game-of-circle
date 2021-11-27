using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class blockScript : MonoBehaviour {

    
    public bool overlappingFlag;
    // Use this for initialization
    void Start () {
	
	}

    

    void OnCollisionStay(Collision other)
    {
        Debug.Log("logic entered in Collision function");
        if (other.gameObject.name == "Sphere(Clone)")
        {
            Debug.Log("the block is touching boundary spheres");
            //gameObject.layer = 2;
            

        }
    }
    // Update is called once per frame
    void Update () {
	
        if(overlappingFlag)
        {
            Debug.Log("logic entered in Collision function");
            //outOfEdgeText.text = "Your block is breaking the boundary";
        }
        
	}
}
