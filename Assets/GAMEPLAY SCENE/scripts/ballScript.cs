using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ballScript : MonoBehaviour
{

    public Vector3 CollisionBallPosition;
    public Vector3 ballDirection;
    float angleOfIncidence = 0;
    public static Vector3 normalOfObject;
    Vector3 directionAfterDeflection = Vector3.zero;
    public Vector3 eventualBallDirection = Vector3.zero;
    public GameObject sampleSphere;
    GameObject tempSphere1, tempSphere2, tempSphere3;
    public static Vector3 onCollisionBallPosition = Vector3.zero;
    public static Material colliderBlockMaterial;

    public Plane trialPlane1;
    public Plane trialPlane2;
    public GameObject baloonyContainer;
    public GameObject animatorObject;
    public GameObject dummyAnimatorObject;
    //public Text ballScriptText;
    //public  string referenceScoreText = "";
    public static Vector3 outDirection;
    public int blockStrikeCount;
    float deaccelerationOfBall;

    // Use this for initialization
    void Start()
    {
        deaccelerationOfBall = .1f;
        blockStrikeCount = 0;
        outDirection = Vector3.zero;
        //referenceScoreText = "Score: ";
        // eventualBallDirection = (Vector3.forward * Time.deltaTime);

    }

   




    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collider name: " + other.gameObject.name);


        if(other.gameObject.name == "bat(Clone)")
        {
            if (sliderVariantScript.ballRace == true)
            {
                //sliderVariantScript.lostBallText.text = "WATCH WHERE YOU ARE HITTING";
                sliderVariantScript.ballOutOfRange = true;
            }
       }





        if (other.gameObject.tag == "target")
        {
            sliderVariantScript.newTarget = true;
            eventualBallDirection = Vector3.zero;
            //sliderVariantScript.ballSpeed = 0;
            sliderVariantScript.totalRotationDisplacement = 0;
            sliderVariantScript.scoreData +=  ( 10 * (blockStrikeCount));
        }

        


    }


    // Update is called once per frame
    void Update()
    {











        if (sliderVariantScript.ballRace == true)
        {

            if (outDirection == Vector3.zero)
            {
                if (sliderVariantScript.colliderBlock != null)
                {
                    if (sliderVariantScript.colliderBlock.GetComponent<Renderer>().material = colliderBlockMaterial)
                    {
                        Debug.Log("Entered this special condition");
                    }
                }
                transform.Translate(-transform.forward * Time.deltaTime * sliderVariantScript.ballSpeed);
                //directionAfterDeflection = eventualBallDirection;
                //Debug.Log("direction after deflection: " + directionAfterDeflection);
                //angleOfIncidence = 0;
                //sliderVariantScript.ballRace = false;
            }
            else
            {

                //transform.Translate(-transform.forward * Time.deltaTime * sliderVariantScript.ballSpeed);
                transform.Translate(outDirection.normalized * Time.deltaTime * sliderVariantScript.ballSpeed);
            }
        }
        else
        {
            if (blockStrikeCount != 0)
            {
                blockStrikeCount = 0;
            }
        }
        if (sliderVariantScript.rayCastPointOfCollision != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, sliderVariantScript.rayCastPointOfCollision) < 1.5f  &&  sliderVariantScript.colliderBlock!= null)
            {

                
                //outDirection = Vector3.Reflect(-(transform.position - sliderVariantScript.rayCastPointOfCollision), normalOfObject);
                sliderVariantScript.colliderBlock.GetComponent<Renderer>().material = colliderBlockMaterial;
                foreach (var d in sliderVariantScript.colliderBlock.GetComponentsInChildren<Renderer>())
                {
                    d.material = colliderBlockMaterial;
                }
                transform.position = sliderVariantScript.rayCastPointOfCollision;
                blockStrikeCount += 1;
                //transform.LookAt(-outDirection);
                outDirection = sliderVariantScript.hardCodedOutDirection;
                sliderVariantScript.hardCodedOutDirection = Vector3.zero;
                //Debug.Log("out direction: " + 5 * outDirection);
                sliderVariantScript.rayCastPointOfCollision = Vector3.zero;
                
                //Debug.Log("block strike count in one event: " + blockStrikeCount);
                //Debug.Log("collider name check: " + other.gameObject.name);
                //eventualBallDirection.Normalize();
                //sliderVariantScript.ballRace = false;
            }
        }

        if (sliderVariantScript.ballSpeed != 0)
        {
            sliderVariantScript.ballSpeed -= deaccelerationOfBall;
            if (sliderVariantScript.ballSpeed <= 0)
            {
                sliderVariantScript.ballSpeed = 0;

            }
        }


    }


}
