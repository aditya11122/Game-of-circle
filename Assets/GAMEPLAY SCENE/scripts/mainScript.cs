using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class mainScript : MonoBehaviour
{
    public bool DrawGizmos = true;
    Vector3 finalPosition = Vector3.zero;
    Vector3 currentPosition =  new Vector3(0, .3f, 20);
    public GameObject sphere;
    public GameObject sphereContainer;
    public List<GameObject> trails;
    public GameObject newObject;
    public List<GameObject> blockStorage;
    public GameObject block;
    public GameObject target;
    public GameObject targetContainer = null;
    int j = 0;
    GameObject torch;
    bool settingUp;
    int k;
    public Button newBlockButton;
    public Slider rotateSlider;
    public Slider scaleSlider;
    Vector3 mouseWorldPosition;
    Vector3 tempScale;
    bool oneTime = true;
    public List<Vector3> PossibleTargetPositions;
    public GameObject bat, batContainer;
    public GameObject ball, ballContainer;
    public bool ballRace = false;
    float ballSpeed;
    Transform batDefaultTransform;
    void Awake()
    {
        tempScale = block.transform.lossyScale;
    }

    // Use this for initialization
    void Start()
    {
        //creating instance of bat
        ballContainer = (GameObject)Instantiate(ball);
        
        batContainer = (GameObject)Instantiate(bat);
        batDefaultTransform = batContainer.transform;
        //batContainer = (GameObject)Instantiate(bat, bat.transform.position, bat.transform.rotation);


        newObject.transform.position = new Vector3(0, .3f, 10);
        for (int h= 0; h<60; ++h)
        {
            GameObject tempObject = new GameObject() ;
            tempObject.transform.position = new Vector3(0, .3f, 23);
            if (h == 0)
            {
                 
                PossibleTargetPositions.Add(tempObject.transform.position);
                Destroy(tempObject);
            }
            if (h > 0)
            {

                tempObject.transform.RotateAround(newObject.transform.position, Vector3.up, -6*h);
                PossibleTargetPositions.Add(tempObject.transform.position);
                Destroy(tempObject);
            }

        }


        torch = (GameObject)Instantiate(block, currentPosition, Quaternion.identity);
        
        //drawCircle();
        createBlocks();
        //createSpecialCircle();
        //Vector3 currentPosition = new Vector3(0, .3f, 20);
        //Vector3 finalPosition = new Vector3(-10, .3f, 10);
        //torch = (GameObject)Instantiate(sphere, currentPosition, Quaternion.identity);
        Button btn = newBlockButton.GetComponent<Button>();
        btn.onClick.AddListener(createBlocks);
        Slider rsld = rotateSlider.GetComponent<Slider>();
        rsld.onValueChanged.AddListener(distortRotation);
        Slider ssld = scaleSlider.GetComponent<Slider>();
        ssld.onValueChanged.AddListener(distortScale);

    }


    IEnumerator waitTime()
    {
        k += 1;
        Debug.Log("entered the enumerator!!");
        yield return new WaitForSeconds(2.0f);
    }

    void normalWaitTime()
    {
        float t = 0;
        while(t<2)
        {
            Debug.Log("normal function delay entered!!");
            t += .5f * Time.deltaTime;
        }
        k += 1;
    }
    void createSpecialCircle()
    {
        Vector3 currentPosition = new Vector3(0, .3f, 20);
        Vector3 finalPosition = new Vector3(-10, .3f, 10);
        GameObject torch = (GameObject)Instantiate(sphere, currentPosition, Quaternion.identity);
        for (int j = 0; j < 23; ++j)
        {
            torch.transform.position = Vector3.Slerp(currentPosition, finalPosition, (1 + j) / 22);

         }
    }

    

        void drawCircle()
            {
            for (int j = 0; j < 90; ++j)
            {
                

                if (j == 0)
                {
                    sphereContainer = (GameObject)Instantiate(sphere, currentPosition, Quaternion.identity);
                    //finalTempPosition = sphereContainer.transform.position;
                }


                else if (j > 0)
                {
                    sphereContainer = (GameObject)Instantiate(sphere, currentPosition, Quaternion.identity);
                    sphereContainer.transform.RotateAround(newObject.transform.position, Vector3.up, -4 * j);

                    //finalTempPosition = sphereContainer.transform.position;
                }
                trails.Add(sphereContainer);
            }
        }

    
    void OnDrawGizmos()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 currentPosition = Vector3.zero;

        if (DrawGizmos != true)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < 4; ++i)
        {

            if (i == 0)
            {
                currentPosition = new Vector3(0, 1, 20);
                finalPosition = new Vector3(10, 1, 10);
            }
            else if (i == 1)
            {
                currentPosition = finalPosition;
                finalPosition = new Vector3(0, 1, 0);
            }
            else if (i == 2)
            {
                currentPosition = finalPosition;
                finalPosition = new Vector3(-10, 1, 10);
            }
            else if (i == 3)
            {
                currentPosition = finalPosition;
                finalPosition = new Vector3(0, 1, 20);
            }
            Vector3 finalTempPosition = Vector3.zero;
            Vector3 currentTempPosition;
            for (int j = 0; j < 90; ++j)
            {

                if (finalTempPosition != Vector3.zero)
                {
                    currentTempPosition = finalTempPosition;
                }
                else
                    currentTempPosition = currentPosition;

                finalTempPosition = Vector3.Slerp(currentTempPosition, finalPosition, ((1 + j) / (90)));
                Gizmos.DrawLine(currentTempPosition, finalTempPosition);
            }
            if (i == 3)
            {
                return;
            }
        }
    }

    bool batSwinging = false;
    int swingCount = 0;
    GameObject chosenOne = null;
    Quaternion batRotation;
    float totalRotationDisplacement;
    Vector3 defaultPointPosition = Vector3.zero;
    bool batSelected;
    // Update is called once per frame
    void Update()
    {

        /* -----------------------RayCast functionality-------------------------------------*/
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(Camera.main.transform.position, (hit.point-Camera.main.transform.position), Color.blue);


            
                //float deltaRotation;
                //Vector3 defaultPointPosition = Vector3.zero;
                

            


            if ((hit.transform.tag == "block"))
            {
                if(Input.GetMouseButtonDown(0))
                {



                    if (chosenOne != null)
                    {
                        chosenOne.transform.tag = "block";
                        chosenOne = hit.transform.gameObject;
                        hit.transform.tag = "chosen one";
                    }
                    else
                    {
                        chosenOne = hit.transform.gameObject;
                        hit.transform.tag = "chosen one";
                    }

                    
                    //hit.transform.gameObject.transform.position = new Vector3(Input.GetAxis("Mouse X"), hit.transform.gameObject.transform.position.y, Input.GetAxis("Mouse Z"));
                    
                }
                
            }

            if (Input.GetMouseButton(0))
            {

                /*  bat selection cose */
                if (hit.transform.tag == "bat")
                {

                    if (defaultPointPosition == Vector3.zero)
                    {
                        batSelected = true;
                        defaultPointPosition = hit.point;
                    }

                }

                if (batSelected == true)
                { 
                    if ((hit.point != defaultPointPosition))
                    {
                        float y_diff = hit.point.z - defaultPointPosition.z;
                        batContainer.transform.RotateAround(new Vector3(-.849f, 0, 23.57f), Vector3.up, -y_diff*7);
                        defaultPointPosition = hit.point;
                    }

                }
                

                /*-------------------------*/
                

                if ((hit.transform.tag == "chosen one"))
                {
                    
                    if (oneTime == true)
                    {
                        mouseWorldPosition = hit.point;
                        oneTime = false;
                    }

                }

                if ((oneTime == false))
                {
                    //if (hit.point!= mouseWorldPosition)
                    //{
                    //float x_difference = .12f * (hit.point.x - mouseWorldPosition.x);
                    //float z_difference = .12f * (hit.point.z - mouseWorldPosition.z);
                    //Debug.Log("x_difference: " + x_difference);
                    //Debug.Log("z_difference: " + z_difference);

                    chosenOne.transform.position = new Vector3(hit.point.x, chosenOne.transform.position.y, hit.point.z);
                        //chosenOne.transform.Translate(hit.point);
                        //chosenOne.transform.position = new Vector3(chosenOne.transform.position.x - x_difference, chosenOne.transform.position.x - x_difference, chosenOne.transform.position.z - z_difference);
                       // mouseWorldPosition = hit.point;
                        //Vector3 difference = 
                    //}
                }
           }
            
            if(Input.GetMouseButtonUp(0))
            {
                if(batSelected == true)
                {
                    //totalRotationDisplacement = batContainer.transform.eulerAngles.y - batDefaultRotation.y;
                    //totalRotationDisplacement = ( batDefaultTransform.eulerAngles.y) - ( batContainer.transform.eulerAngles.y);
                    totalRotationDisplacement = 360 -batContainer.transform.eulerAngles.y;
                    batSwinging = true;
                    batSelected = false;
                    ballSpeed = .3f * totalRotationDisplacement;
                    defaultPointPosition = Vector3.zero;
                }

                oneTime = true;
            }

        }

        /*-------------------------------------------------------------------------------------*/

        if(batSwinging == true)
        {
            float partOfTotalRotation = totalRotationDisplacement / 10;
            if (swingCount < 10)
            {
                
                 batContainer.transform.RotateAround(new Vector3(-.849f, 0, 23.57f), Vector3.up, partOfTotalRotation);
                swingCount += 1;
            }
            if(swingCount == 10)
            {
                batSwinging = false;
                batContainer.transform.localRotation = batDefaultTransform.localRotation;
                batContainer.transform.position = batDefaultTransform.position;
                swingCount = 0;
                ballRace = true;
                
            }

        }

        if(ballRace == true)
        {
            ballContainer.transform.Translate(-Vector3.forward * ballSpeed * Time.deltaTime);
        }


        if(Input.GetKeyDown(KeyCode.Space))
        {
            settingUp = true;
            k = 0;
        }

        if((settingUp == true))
        {
            torch.transform.RotateAround(newObject.transform.position, Vector3.up, -4 );
            GameObject tempObject = (GameObject)Instantiate(sphere, torch.transform.position, Quaternion.identity);
            trails.Add(tempObject);
            //StartCoroutine(waitTime());
            normalWaitTime();
            if(k >= 90)
            {
                settingUp = false;
                Destroy(torch);
            }

        }

        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            
            if(targetContainer == null)
            {
                int r = Random.Range(0, 60);
                targetContainer = (GameObject)Instantiate(target, PossibleTargetPositions[r], Quaternion.identity);
                targetContainer.transform.LookAt(newObject.transform);
            }
            if(targetContainer != null)
            {

                Destroy(targetContainer);
                int r = Random.Range(0, 60);
                targetContainer = (GameObject)Instantiate(target, PossibleTargetPositions[r], Quaternion.identity);
                targetContainer.transform.LookAt(newObject.transform);
            }
        }
        
    }

    void createBlocks()
    {
        GameObject tempStorer;
        tempStorer = (GameObject)Instantiate(block, new Vector3(-2, .2f, 8), Quaternion.identity);
        tempStorer.tag = "block";
        blockStorage.Add(tempStorer);

    }

    void distortRotation(float angle)
    {
        chosenOne.transform.eulerAngles = new Vector3 (0, angle, 0);
    }

    void distortScale(float magnitude)
    {

        
        chosenOne.transform.localScale = new Vector3(tempScale.x, tempScale.y, magnitude * tempScale.z);
    }

}

