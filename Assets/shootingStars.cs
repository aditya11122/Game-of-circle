using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class shootingStars : MonoBehaviour {
    public List<GameObject> starsContainer;
    public List<Vector3> eachStarDirectionContainer;
    public GameObject star;
    public Vector3 pointOfInstantiation;
    public Vector3 randomPosition;
    public float starTravelSpeed = 45;
    [SerializeField]
    public static bool starsSwitch;
	// Use this for initialization
	void Start () {


        pointOfInstantiation = new Vector3(0, -40, 16);
        if (starsSwitch == false)
        {
            InvokeRepeating("starCreation", .5f, .03f);
        }
	}

    void starCreation()
    {

        GameObject tempStarObject = (GameObject)Instantiate(star, pointOfInstantiation, Quaternion.identity);
        starsContainer.Add(tempStarObject);
        randomPosition = Camera.main.ScreenToWorldPoint (new Vector3(Random.Range(0, 724), Random.Range(0, 724),18));
        Vector3 starDirection = (randomPosition - pointOfInstantiation).normalized;
        eachStarDirectionContainer.Add(starDirection);
    }
	
	// Update is called once per frame
	void Update () {
        if (starsSwitch == false)
        {
            if (starsContainer.Count > 0)
            {

                for (int k = 0; k < starsContainer.Count; ++k)
                {

                    starsContainer[k].transform.Translate(eachStarDirectionContainer[k] * Time.deltaTime * starTravelSpeed);
                    if (starsContainer[k].transform.position.y > 30)
                    {
                        starsContainer[k].transform.position = pointOfInstantiation;
                    }

                }
                if (starsContainer.Count > 150)
                {
                    CancelInvoke();
                }
            }
        }
        
            if(starsSwitch)
        {
            Debug.Log("star switch state: " + starsSwitch);
            if (starsContainer.Count > 0)
            {
                CancelInvoke();
                for (int h = 0; h < starsContainer.Count; ++h)
                {

                    Destroy(starsContainer[h]);
                    starsContainer[h] = null;
                }
                starsContainer.RemoveRange(0, 150);
            }
            
            //transform.GetComponent<shootingStars>().enabled = false;
        }
        

        
        
        //Debug.Log("screen height: " + Camera.main.pixelHeight + "screen width: " + Camera.main.pixelWidth);
	}
}
