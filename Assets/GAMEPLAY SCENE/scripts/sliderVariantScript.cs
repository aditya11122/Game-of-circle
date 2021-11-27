using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class sliderVariantScript : MonoBehaviour
{
    
    
    public GameObject defaultGamePlane;
    public AudioClip menuBackGroundMusic;
    public AudioClip gameViewMusic;
    public bool DrawGizmos;
    Vector3 finalPosition;
    Vector3 currentPosition;
    public GameObject sphere;
    public GameObject sphereContainer;
    public List<GameObject> trails;
    public GameObject newObject;
    public List<GameObject> blockStorage;
    public GameObject block;
    public GameObject target;
    public GameObject targetContainer;
    int j ;
    public GameObject torch;
    bool settingUp;
    int k;

    Vector3 mouseWorldPosition;
    Vector3 tempScale;
    bool oneTime;
    public List<Vector3> PossibleTargetPositions;
    public GameObject bat;
    public GameObject batContainer;
    public GameObject ball, ballContainer;
    public static bool ballRace;
    public static float ballSpeed;
    float rotationLimit, scaleLimit;
    public Button newBockButton;
    Transform batDefaultTransform;
    Vector3 onScreenMouseRotationPosition, onScreenMouseScalePosition, previousMousePosition;
    bool rotationFlag, scaleFlag;
    public static Vector3 initialBallPosition;
    public GameObject arrow, arrowContainer;
    public static bool newTarget;
    public Material blockSelectionMaterial, defaultBlockMaterial;
    public float tapTime;// total tapping time.
    public static bool ballOutOfRange;
    public  Text lostBallText;
    public Vector3 TargetPos;
    public static GameObject colliderBlock;
    //public static GameObject normalFlag;

    public float waitStateTimer;
    bool batSwinging;
    int swingCount;
    GameObject chosenOne;
    //Quaternion batRotation;
    public static float totalRotationDisplacement;
    Vector3 defaultPointPosition;
    bool batSelected;
    int flag;// flag to check if the tap for scale or rotationhas been done
    Vector3 previousScale, previousRotation, newScale, startMousePressPosition;// variables for rotation and scaling of objects
    Vector3 hitPointAndBlockPosDiff;
    public static Vector3 rayCastPointOfCollision;
    public static float fixedDistance, updatedDistance;
    public static string ballState;
    public static Vector3 hardCodedOutDirection;
    public Text score, highScoreText;
    public static int scoreData, highScore;
    public bool hardCodeFillerFlag;
    public string GAME_PLAY_STATUS;
    public Button playButton, instructionButton, highScoreButton;
    public bool fadeAwayGo;
    public GameObject gamePlayUI;
    public GameObject mainUI;
    public Text outOfEdgeText;
    //public Camera secondCamera;
    //public Camera mainCamera;
    void Awake()
    {
        Debug.Log("distance: " + Vector3.Distance(new Vector3(0, .3f, 20), new Vector3(0, .3f, 10)));
        if (GAME_PLAY_STATUS == "playMode")
        {
            highScore = 0;
            waitStateTimer = 0;
            outOfEdgeText.enabled = false;
            scoreData = 0;
            hardCodedOutDirection = Vector3.zero;
            //secondCamera.enabled = false;
            colliderBlock = new GameObject();
            ballState = "firstStrike";
            //normalFlag = new GameObject();
            defaultPointPosition = Vector3.zero;
            chosenOne = null;
            swingCount = 0;
            batSwinging = false;
            ballOutOfRange = false;
            tapTime = 0;
            newTarget = true;
            scaleLimit = 5;
            rotationLimit = 5;
            ballRace = false;
            oneTime = true;
            settingUp = true;
            j = 0;
            k = 0;
            targetContainer = null;
            currentPosition = new Vector3(0, .3f, 20);
            DrawGizmos = true;
            finalPosition = Vector3.zero;
            tempScale = block.transform.lossyScale;
        }
    }



    // Use this for initialization
    void Start()
    {
        if (GAME_PLAY_STATUS == "menuMode")
        {
            

        }

        
        //gamePlayUI.SetActive(false);
        //GAME_PLAY_STATUS = "menu_mode";

        // normalFlag = (GameObject)Instantiate(sphere, Vector3.zero, Quaternion.identity);
        //creating instance of bat
        if (GAME_PLAY_STATUS == "playMode")
        {
            highScore = PlayerPrefs.GetInt("highScore");
            
            ballContainer = (GameObject)Instantiate(ball);
            initialBallPosition = ballContainer.transform.position;
            batContainer = (GameObject)Instantiate(bat);
            batDefaultTransform = batContainer.transform;
            //batContainer = (GameObject)Instantiate(bat, bat.transform.position, bat.transform.rotation);


            newObject.transform.position = new Vector3(0, .3f, 10);
            for (int h = 0; h < 60; ++h)
            {
                GameObject tempObject = new GameObject();
                tempObject.transform.position = new Vector3(0, .3f, 23);
                if (h == 0)
                {

                    PossibleTargetPositions.Add(tempObject.transform.position);
                    Destroy(tempObject);
                }
                if (h > 0)
                {

                    tempObject.transform.RotateAround(newObject.transform.position, Vector3.up, -6 * h);
                    PossibleTargetPositions.Add(tempObject.transform.position);
                    Destroy(tempObject);
                }

            }


            torch = (GameObject)Instantiate(block, currentPosition, Quaternion.identity);
            torch.layer = 2;

            createBlocks();
            
        }


    }

    


    // function that finds out the normal to any object passed to it.
    /*public static Vector3 normalCalculator(GameObject anyObject, Vector3 collisionPoint)
    {

        Vector3 normalDirection = Vector3.zero;
        //float angle = anyObject.transform.eulerAngles.y;
        //Transform sampleTransform = anyObject.transform;
        //sampleTransform.position = sampleTransform.right*2;
        //arrowContainer = (GameObject)Instantiate(arrow, collisionPoint, Quaternion.Euler(anyObject.transform.eulerAngles));
        normalDirection = (anyObject.transform.right);
        normalDirection += new Vector3(0, 0, 2);
        //Debug.Log("normal direction: " + normalDirection);
        return normalDirection;

    }*/




    /*void normalWaitTime()
    {
        float t = 0;
        while (t < 2)
        {
            Debug.Log("normal function delay entered!!");
            t += .5f * Time.deltaTime;
        }
        k += 1;
    }*/




   /* void drawCircle()
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
    */


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

    public void deSelectAll()
    {
        if (chosenOne != null)
        {
            chosenOne.transform.GetComponent<Renderer>().material = defaultBlockMaterial;
            foreach (var d in chosenOne.GetComponentsInChildren<Renderer>())
            {
                d.material = defaultBlockMaterial;
            }
            chosenOne.transform.tag = "block";
            chosenOne = null;
        }
    }



    
    
    // Update is called once per frame
    void Update()
    {
        if(GAME_PLAY_STATUS == "waitState")
        {
            waitStateTimer += Time.deltaTime;
            if(waitStateTimer>= 1)
            {
                waitStateTimer = 0;
                GAME_PLAY_STATUS = "playMode";
                initialiseNewTarget();
            }
        }

        if(GAME_PLAY_STATUS == "fadingAwayMainUI")
        {
            
            mainUI.GetComponent<CanvasGroup>().alpha -=  Time.deltaTime;
            mainUI.transform.localScale+= new Vector3(.5f* Time.deltaTime, .5f* Time.deltaTime, 0);

            if(mainUI.GetComponent<CanvasGroup>().alpha<= 0)
            {
                defaultGamePlane.SetActive(true);
                
                mainUI.SetActive(false);
                //gamePlayUI.SetActive(true);
                GAME_PLAY_STATUS = "playMode";
                gamePlayUI.SetActive(true);
                Awake();
                Start();
                //drawCircle();
            }
        }

        if (GAME_PLAY_STATUS == "menuMode")
        {
           

            

        }
        //GamePlay State
        if (GAME_PLAY_STATUS == "playMode")
        {
            
            /*if (ballOutOfRange == true)
            {
                ballOutOfRangeSituation();
                

                //ballOutOfRange = false;

            }
            if (ballOutOfRange == true)
            {
                initialiseNewTarget();
            }*/

            if (ballScript.onCollisionBallPosition != Vector3.zero)
            {
                //normalFlag.transform.position = ballScript.onCollisionBallPosition;
                ballScript.onCollisionBallPosition = Vector3.zero;
            }


            /* -----------------------RayCast functionality-------------------------------------*/

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit ballHit;

            if (ballScript.outDirection == Vector3.zero)
            {
                //Debug.Log("Level 1 check");
                if (Physics.Raycast(ballContainer.transform.position, -ballContainer.transform.forward, out ballHit))
                {
                    //Debug.Log("Level 2 check");
                    if (ballHit.transform.tag == "side")
                    {


                        Debug.DrawRay(ballContainer.transform.position, -10 * ballContainer.transform.forward, Color.green, .3f, true);
                        rayCastPointOfCollision = ballHit.point;
                        //Debug.Log("rayCast detects the side object!!");
                        //Debug.Log("Hit point: " + ballHit.point);
                        //ballHit.transform.gameObject.transform.parent.GetComponent<Renderer>().material = ballScript.colliderBlockMaterial;
                        colliderBlock = ballHit.transform.gameObject.transform.parent.gameObject;
                        ballScript.normalOfObject = ballHit.transform.forward;
                        if (hardCodedOutDirection == Vector3.zero && ballRace == true && rayCastPointOfCollision!= Vector3.zero)
                        {
                            hardCodedOutDirection = Vector3.Reflect(-(ballContainer.transform.position - rayCastPointOfCollision), ballScript.normalOfObject);
                        }
                        ballContainer.GetComponent<ballScript>().eventualBallDirection = (ballContainer.transform.position - rayCastPointOfCollision);
                    }
                    else
                    {
                        colliderBlock = null;
                        ballContainer.GetComponent<ballScript>().eventualBallDirection = -10 * ballContainer.transform.forward;

                        //rayCastPointOfCollision = Vector3.zero;
                        //ballScript.outDirection = Vector3.zero;
                    }
                }
            }
            else
            {
                if (Physics.Raycast(ballContainer.transform.position, ballScript.outDirection, out ballHit))
                {
                    if (ballHit.transform.tag == "side")
                    {
                        Debug.DrawRay(ballContainer.transform.position, 15 * ballScript.outDirection, Color.green, .3f, true);
                        rayCastPointOfCollision = ballHit.point;
                        //Debug.Log("rayCast detects the  side of subsequent object!!");
                        //Debug.Log("Hit point: " + ballHit.point);
                        //ballHit.transform.gameObject.transform.parent.GetComponent<Renderer>().material = ballScript.colliderBlockMaterial;
                        colliderBlock = ballHit.transform.gameObject.transform.parent.gameObject;
                        ballScript.normalOfObject = ballHit.transform.forward;
                        if (hardCodedOutDirection == Vector3.zero && ballRace == true && rayCastPointOfCollision != Vector3.zero)
                        {
                            hardCodedOutDirection = Vector3.Reflect(-(ballContainer.transform.position - rayCastPointOfCollision), ballScript.normalOfObject);
                        }
                        ballContainer.GetComponent<ballScript>().eventualBallDirection = (ballContainer.transform.position - rayCastPointOfCollision);
                    }
                    else
                    {
                        colliderBlock = null;
                        //rayCastPointOfCollision = 
                        ballContainer.GetComponent<ballScript>().eventualBallDirection = 10 * ballContainer.transform.forward;
                        //ballScript.normalOfObject = Vector3.zero;
                        //rayCastPointOfCollision = Vector3.zero;
                        //ballScript.outDirection = Vector3.zero;
                        //rayCastPointOfCollision = -ballContainer.transform.forward;
                    }
                }
            }



            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(Camera.main.transform.position, (hit.point - Camera.main.transform.position), Color.blue);

                //float deltaRotation;
                //Vector3 defaultPointPosition = Vector3.zero;

                // scale and rotation of block code starts here
                if (hit.transform.tag == "plane")
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (chosenOne != null)
                        {
                            startMousePressPosition = Input.mousePosition;
                            previousMousePosition = Input.mousePosition;

                            if ((previousMousePosition.x < Screen.width / 10))// for rotation
                            {
                                //Debug.Log("Rotation mouse down loop");
                                flag = 1;
                            }
                            else if (previousMousePosition.x > 9 * (Screen.width / 10))//for scale
                            {
                                //Debug.Log("Scale mouse down loop");
                                //onScreenMouseScalePosition = Input.mousePosition;
                                flag = 2;

                                previousScale = chosenOne.transform.localScale;
                                newScale = previousScale;
                            }
                        }
                    }

                    if (Input.GetMouseButton(0))
                    {

                        Vector3 mousePositionDelta = Input.mousePosition - previousMousePosition;

                        //float x_difference = Input.mousePosition.x -  onScreenMouseRotationPosition.x ;
                        //Debug.Log("magnitude: " + mousePositionDelta.magnitude);
                        if ((Input.mousePosition - startMousePressPosition).magnitude >= 3 && (chosenOne != null))
                        {
                            if (flag == 1)// rotation
                            {

                                chosenOne.transform.eulerAngles += new Vector3(0, 3 * mousePositionDelta.y, 0);
                            }
                            else if (flag == 2)
                            {
                                //Vector3 newScale = previousScale;
                                newScale.z = newScale.z + .2f * mousePositionDelta.y;
                                newScale.z = Mathf.Clamp(newScale.z, 2.5f, 10);
                                //previousScale.z = Mathf.Clamp(.4f * mousePositionDelta.x, 2.5f, 10f);
                                chosenOne.transform.localScale = newScale;

                            }
                        }
                        previousMousePosition = Input.mousePosition;
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        Vector3 deltaMouse = Input.mousePosition - startMousePressPosition;
                        //Debug.Log("mouse delta: " + deltaMouse.magnitude);
                        flag = 0;
                        if (deltaMouse.magnitude < 3)
                        {
                            previousMousePosition = Vector3.zero;
                            deSelectAll();
                        }
                    }
                }
                // scale and rotation code end here







                if (hit.transform.tag == "bat")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        deSelectAll();
                        if (defaultPointPosition == Vector3.zero)
                        {
                            batSelected = true;
                            defaultPointPosition = hit.point;
                        }
                    }
                }


                if ((hit.transform.tag == "pickAndDrop"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {

                        if (chosenOne != null)
                        {
                            //chosenOne.transform.GetComponent<Renderer>().material = defaultBlockMaterial;
                            chosenOne.transform.tag = "block";
                        }
                        chosenOneFiller(hit.transform.parent.gameObject);
                    }
                }

                if (Input.GetMouseButton(0))
                {


                    /*  bat selection code */


                    if (batSelected == true)
                    {
                        //defaultPointPosition = hit.point;

                        if ((hit.point != defaultPointPosition))
                        {
                            float y_diff = ( hit.point.z - defaultPointPosition.z);
                            //y_diff = Mathf.Clamp(y_diff, defaultPointPosition.z, )
                            //y_diff = Mathf.Clamp(y_diff, 0, 60);
                            batContainer.transform.RotateAround(new Vector3(-.849f, 0, 23.57f), Vector3.up,  -7 * y_diff);
                            
                            defaultPointPosition = hit.point;
                        }

                    }



                    /*-------------------------*/


                    if ((hit.transform.tag == "pickAndDrop"))
                    {
                        if (chosenOne != null)
                        {
                            if (oneTime == true)
                            {

                                hitPointAndBlockPosDiff = chosenOne.transform.position - hit.point;
                                oneTime = false;
                            }
                        }
                    }


                    if ((oneTime == false))
                    {
                        // chosenOne.transform.position - hit.point;
                        //if (hit.point!= mouseWorldPosition)
                        //{
                        //float x_difference = .12f * (hit.point.x - mouseWorldPosition.x);
                        //float z_difference = .12f * (hit.point.z - mouseWorldPosition.z);
                        //Debug.Log("x_difference: " + x_difference);
                        //Debug.Log("z_difference: " + z_difference);
                        //if (hit.transform.gameObject != chosenOne)
                        //{ 
                        //if(mouseWorldPosition!= hit.point)
                        //{
                        //hitPointAndBlockPosDiff = hit.point - mouseWorldPosition;
                        if (chosenOne != null)
                        {
                            chosenOne.transform.position = new Vector3(hit.point.x + hitPointAndBlockPosDiff.x, .35f, hit.point.z + hitPointAndBlockPosDiff.z);
                        }
                        //}
                        //hitPointAndBlockPosDiff = chosenOne.transform.position - hit.point;


                        //}
                        //mouseWorldPosition = hit.point;
                        //chosenOne.transform.Translate(hit.point);
                        //chosenOne.transform.position = new Vector3(chosenOne.transform.position.x - x_difference, chosenOne.transform.position.x - x_difference, chosenOne.transform.position.z - z_difference);
                        // mouseWorldPosition = hit.point;
                        //Vector3 difference = 
                        //}
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {

                    if (batSelected == true)
                    {
                        //chosenOne.transform.GetComponent<Renderer>().material = defaultBlockMaterial;
                        //chosenOne = null;
                        //totalRotationDisplacement = batContainer.transform.eulerAngles.y - batDefaultRotation.y;
                        //totalRotationDisplacement = ( batDefaultTransform.eulerAngles.y) - ( batContainer.transform.eulerAngles.y);
                        totalRotationDisplacement =  360-batContainer.transform.eulerAngles.y;
                        batSwinging = true;
                        batSelected = false;
                        ballSpeed =  totalRotationDisplacement;
                        defaultPointPosition = Vector3.zero;
                    }

                    oneTime = true;
                }

            }

            /*-------------------------------------------------------------------------------------*/

            if (batSwinging == true)
            {
                float partOfTotalRotation = totalRotationDisplacement / 10;
                if (swingCount < 10)
                {

                    batContainer.transform.RotateAround(new Vector3(-.849f, 0, 23.57f), Vector3.up, partOfTotalRotation);
                    swingCount += 1;
                }
                if (swingCount == 10)
                {
                    batSwinging = false;
                    batContainer.transform.localRotation = batDefaultTransform.localRotation;
                    batContainer.transform.position = batDefaultTransform.position;
                    swingCount = 0;

                    ballRace = true;

                }

            }

            if (targetContainer && ballContainer)
            {
                if (Vector3.Distance(targetContainer.transform.position, Vector3.zero) < Vector3.Distance(ballContainer.transform.position, Vector3.zero) && Vector3.Distance(ballContainer.transform.position, targetContainer.transform.position) < 12)
                {
                    //Debug.Log("YAHEE YAPPA did you see that??!!??");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                settingUp = true;
                k = 0;
            }

            if ((settingUp == true))
            {
                torch.transform.RotateAround(newObject.transform.position, Vector3.up, -4);
                GameObject tempObject = (GameObject)Instantiate(sphere, torch.transform.position, Quaternion.identity);
                trails.Add(tempObject);
                k += 1;
                //StartCoroutine(waitTime());
                //normalWaitTime();
                if (k >= 90)
                {
                    settingUp = false;
                    Destroy(torch);
                    ballContainer.transform.position = ball.transform.position;
                    //settingUp = false;
                }

            }


            //new target initialisation
            if (Input.GetKeyDown(KeyCode.LeftAlt) || newTarget == true)
            {
                initialiseNewTarget();

            }


            //if ball goes away out of range

            Vector3 ballUpdatedCheckPosition = Camera.main.WorldToScreenPoint(ballContainer.transform.position);
            if (ballUpdatedCheckPosition.x < 0 || ballUpdatedCheckPosition.x > Screen.width || ballUpdatedCheckPosition.y < 0 || ballUpdatedCheckPosition.y > Screen.height)
            {

                lostBallText.text = "YOU MISSED THE TARGET and YOU LOST THE BALL!!";
                GAME_PLAY_STATUS = "waitState";
                //ballOutOfRange = true;
            }
            //to check if the ball has gotten out of the circle without hitting the target

            
            Vector3 ballWorldPos = ballContainer.transform.position;
            if (ballSpeed == 0 && ballRace == true)
            {
                if (ballWorldPos.x > -11 || ballWorldPos.x < 10 || ballWorldPos.z > -1 || ballWorldPos.z < 20)
                {
                    lostBallText.text = "FOR GOD'S SAKE PUT YOUR SCRAWNY ARMS TO SOME USE!!";

                }
                else if (ballWorldPos.x < -17 || ballWorldPos.x > 17 || ballWorldPos.z < -4 || ballWorldPos.z > 25)
                {
                    lostBallText.text = "WHERE DO YOU THINK YOU WERE GOING WITH IT!?!";
                }
                GAME_PLAY_STATUS = "waitState";
                //ballOutOfRange = true;
            }


            //checking if the block is out of the circle range
            if (chosenOne != null)
            {
                bool outOfEdgeFlag = false;
                Transform[] children = new Transform[5];
                for (int r = 0; r < chosenOne.transform.childCount; ++r)
                {
                    children[r] = chosenOne.transform.GetChild(r);
                }
                foreach (var f in children)
                {
                    if (Vector3.Distance(new Vector3(0, .3f, 10), f.position) > 10)
                    {
                        outOfEdgeFlag = true;
                    }

                    if (outOfEdgeFlag == true)
                    {
                        if (outOfEdgeText.enabled == false)
                        {
                            outOfEdgeText.enabled = true;
                            batContainer.layer = 2;
                        }
                    }
                    else
                    {

                        if (outOfEdgeText.enabled == true)
                        {
                            outOfEdgeText.enabled = false;
                            batContainer.layer = 0;
                        }
                    }
                }
            }


            if(lostBallText.text!= "")
            {
                ballOutOfRangeSituation();
            }


        }
         
        
        








        /*----------------------*/





        /*-------------------*/

    }

    

    public void ballOutOfRangeSituation()
    {

        scoreData = 0;
    }

    public void createBlocks()
    {
        GameObject tempStorer;
        tempStorer = (GameObject)Instantiate(block, new Vector3(-2, .2f, 8), Quaternion.identity);
        tempStorer.tag = "block";
        tempStorer.GetComponent<Renderer>().material = defaultBlockMaterial;
        foreach (var d in tempStorer.GetComponentsInChildren<Renderer>())
        {
            d.material = defaultBlockMaterial;
        }
        blockStorage.Add(tempStorer);

    }

    public void chosenOneFiller(GameObject selectedObject)
    {
        
        chosenOne = selectedObject;
        chosenOne.transform.GetComponent<Renderer>().material = blockSelectionMaterial;
        foreach(var d in chosenOne.GetComponentsInChildren<Renderer>())
        {
            d.material = blockSelectionMaterial;
        }
        chosenOne.transform.tag = "chosen one";

        
    }

    public void initialiseNewTarget()
    {
        if(scoreData> highScore)
        {
            highScore = scoreData;
            highScoreText.text = "highScore: " + scoreData;
            PlayerPrefs.SetInt("highScore", highScore);
            PlayerPrefs.Save();
        }
        score.text = "Score: " + scoreData;
        highScoreText.text = "HighScore: " + highScore;
        lostBallText.text = "";
        ballRace = false;
        ballSpeed = 0;
        ballOutOfRange = false;
        ballScript.outDirection = Vector3.zero;
        newTarget = false;
        ballContainer.transform.position = initialBallPosition;
        //GameObject[] residualBlocks = GameObject.FindGameObjectsWithTag("block");
        foreach (var s in blockStorage)
        {
            Destroy(s);
        }
        
        if (targetContainer != null)
        {

            Destroy(targetContainer);
            
        }
        int r;
        do
        {
            r = Random.Range(0, 60);
        } while ((Vector3.Distance(batContainer.transform.position, PossibleTargetPositions[r]) < 3));

        targetContainer = (GameObject)Instantiate(target, PossibleTargetPositions[r], Quaternion.identity);
        targetContainer.transform.LookAt(newObject.transform);
        TargetPos = targetContainer.transform.position;
    }

    public void reset()
    {
        if (targetContainer != null)
        {
            Destroy(targetContainer);
        }

        for(int i =0; i< trails.Count; ++i)
        {
            Destroy(trails[i]);
        }

        if (blockStorage.Count != 0)
        {
            for (int t = 0; t < blockStorage.Count; ++t)
            {
                Destroy(blockStorage[t]);
            }
        }

        if(batContainer!= null)
        {
            Destroy(batContainer);
        }

        if(ballContainer!= null)
        {
            Destroy(ballContainer);
        }

        

        //GameObject[] allObjects = FindObjectsOfType((GameObject));
        
    }

    public void playButtonFunctionality()
    {
        shootingStars.starsSwitch = true;
        GAME_PLAY_STATUS = "fadingAwayMainUI";
        //transform.GetComponent<AudioSource>().
        Debug.Log("Play button was clicked");
        
    }
    public void instructionButtonFunctionality()
    {
        Debug.Log("instruction button was clicked");
    }
    public void exitFunctionality()
    {

        reset();
        gamePlayUI.SetActive(false);
        mainUI.SetActive(true);
        mainUI.GetComponent<CanvasGroup>().alpha = 1;
        mainUI.transform.localScale = new Vector3(1, 1, 1);
        GAME_PLAY_STATUS = "menu_mode";
        defaultGamePlane.SetActive(false);
        
        shootingStars.starsSwitch = false;
    }
    //void distortRotation(float angle)
    //{
    //    chosenOne.transform.eulerAngles = new Vector3(0, angle, 0);
    //}

    //void distortScale(float magnitude)
    //{
    //   chosenOne.transform.localScale = new Vector3(tempScale.x, tempScale.y, magnitude * tempScale.z);
    //}

}





