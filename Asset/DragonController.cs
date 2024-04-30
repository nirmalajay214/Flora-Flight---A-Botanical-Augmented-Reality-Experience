using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
   [SerializeField] private float speed;

   private FixedJoystick[] fixedJoysticks;
   private GameObject[] flowers;
   private Rigidbody rigidBody;
   private Animator animator;
   private Animator animationAfterEat;
   private float distance;
   VegetationOnMesh vegetation;
   public GameObject closestFlowerObject { get; private set; }
   public GameObject closestFlowerRef { get; private set; }
   public Vector3 HighestVertexClosestFlower { get; private set; }
   public List<GameObject> gameOList = new List<GameObject>();
   public ScoreManager scoremanager;
   public bool isClicked;
   public int count = 0;
   public bool onPlant = false;
    private void OnEnable(){
        fixedJoysticks = FindObjectsOfType<FixedJoystick>();
        Debug.Log("Joystick prop - "+ fixedJoysticks);
        rigidBody = gameObject.GetComponent<Rigidbody>();  
    }

    public void OnButtonClick(){
        float closestDistance = Mathf.Infinity;
        GameObject dragon = GameObject.FindGameObjectWithTag("RealDragon");
        animator = dragon.GetComponent<Animator>();
        animator.StopPlayback();
        animator.SetTrigger("PlantEat");
        Vector3 dragonPosition = dragon.transform.position;
        // Debug.Log("Button Clicked");
        flowers = GameObject.FindGameObjectsWithTag("PlantsOnMesh");
        if(flowers != null){
           foreach(GameObject flower in flowers){
                Vector3 flowerPosition = flower.transform.position;
                distance = Vector3.Distance(dragonPosition, flowerPosition);
                if (distance < closestDistance){
                    closestDistance = distance;
                    closestFlowerObject = flower;
                    closestFlowerRef = flower;
                    // To get the highest vertex of the closest flower (assuming Y is the vertical axis)
                    Vector3[] vertices = flower.GetComponent<MeshFilter>().mesh.vertices;
                    float highestY = float.MinValue;

                    foreach (Vector3 vertex in vertices){
                        if (vertex.y > highestY){
                            highestY = vertex.y;
                        }
                    }
                    HighestVertexClosestFlower = flowerPosition + new Vector3(0, highestY, 0);
                }
           }
        }
        
        isClicked = true;
        // Debug.Log(closestFlowerObject.transform.position);
        // Debug.Log(HighestVertexClosestFlower);
        
        // dragon.transform.position = Vector3.MoveTowards(dragon.transform.position, HighestVertexClosestFlower, distance*speed );
    }

    public void Update(){
        if(isClicked){
            GameObject dragon = GameObject.FindGameObjectWithTag("RealDragon");
            dragon.transform.position += (HighestVertexClosestFlower - dragon.transform.position).normalized*speed*Time.deltaTime;
            if (Vector3.Distance(dragon.transform.position, HighestVertexClosestFlower) < 0.001f){
                isClicked = false;
            }
            // Swap the position of the cylinder.
                // animationAfterEat = dragon.GetComponent<Animator>();
                // bool triggerValue = animationAfterEat.GetBool("");


                // if(onPlant){
                //     isClicked = false;
                //     Destroy(closestFlowerObject);
                //     count+=1;
                //     Debug.Log(count);
                //     scoremanager = GameObject.FindGameObjectWithTag("ScoreCanvas").GetComponent<ScoreManager>();
                //     scoremanager.scoreUpdate(count);
                //     onPlant = false;
                // }


                // if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Glide 0")){
                //     isClicked = false;
                //     Destroy(closestFlowerObject);
                //     count+=1;
                //     Debug.Log(count);
                //     scoremanager = GameObject.FindGameObjectWithTag("ScoreCanvas").GetComponent<ScoreManager>();
                //     scoremanager.scoreUpdate(count);
                // }
        }
    }

    public void destroyPlant(){
        Destroy(closestFlowerObject);
        count+=1;
        Debug.Log(count);
        scoremanager = GameObject.FindGameObjectWithTag("ScoreCanvas").GetComponent<ScoreManager>();
        scoremanager.scoreUpdate(count);
    }

   private void FixedUpdate(){
        float xVal = fixedJoysticks[1].Horizontal;
        float zVal = fixedJoysticks[1].Vertical;
        float yVal = fixedJoysticks[0].Vertical;
        Vector3 movement = new Vector3(xVal, yVal, zVal);
        rigidBody.velocity = movement * speed;
        

        // Vector3 movementUpDown = new Vector3(0, yVal, 0);
        // rigidBody.velocity = movementUpDown * speed;

        if(xVal != 0 && zVal != 0){
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(xVal, zVal)*Mathf.Rad2Deg, transform.eulerAngles.z);
        }
    }
}
