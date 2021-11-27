using UnityEngine;
using System.Collections;

public class animatingScript : MonoBehaviour {

    float timer = 0;
	// Use this for initialization
	void Start () {

        //transform.GetComponent<BoxCollider>().isTrigger = true;
        
	}
	
	// Update is called once per frame
	void Update () {

        
        transform.localScale += new Vector3(3*Time.deltaTime, 0, 3*Time.deltaTime);
        timer += Time.deltaTime;
        if(timer > 1)
        {
            //Destroy(gameObject);
        }
	}
}
