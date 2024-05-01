using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Dragon_PlayerMovements : MonoBehaviour
{
    public float speed = 3.0f;
    public float rotationSpeed = 3.0f;
    public float riseSpeed = 0.015f;
    public float distanceUntilInteraction = 0.05f;
    public float distanceOffset = 0.25f;
    public float interactionSpeed = 3f;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 riseInput = Vector2.zero;
    bool isInteracting;

    private FindClosestFlower findClosestFlower;
    private Dragon_PlayerControl playerControls;
    private GameObject closestFlower;

    private GameObject referenceClosestFlower;

    public GameObject RefClosestFlower{
        set { referenceClosestFlower = value; }
        get {return referenceClosestFlower; }
    }

    public static event Action successFullInteraction;
    public static event Action interactionClosed;

    private void Awake(){
        playerControls = GetComponent<Dragon_PlayerControl>();
        findClosestFlower = FindObjectOfType<FindClosestFlower>().GetComponent<FindClosestFlower>();
    }

    private void OnEnable(){
        playerControls.OnMove += HandleMove;
        playerControls.OnRise += HandleRise;
        playerControls.OnInterAct += HandleInteraction;
    }

    private void OnDisable(){
        playerControls.OnMove -= HandleMove;
        playerControls.OnRise -= HandleRise;
        playerControls.OnInterAct -= HandleInteraction;
    }

    private void HandleInteraction(){
        GameObject closestFlower = findClosestFlower.closestFlowerObject;
        Debug.Log("Closestflower =>" + closestFlower);
        StartCoroutine(Interact(closestFlower));
    }

    IEnumerator Interact(GameObject obj){
        isInteracting = true;
        Vector3 highestVertex = findClosestFlower.HighestVertexClosestFlower;
        while(Vector3.Distance(transform.position, highestVertex) > distanceUntilInteraction){
            if(Vector3.Distance(transform.position, highestVertex) < distanceOffset * .125f){
                transform.position = highestVertex;
                break;
            }

            transform.position = Vector3.Slerp(transform.position, highestVertex, interactionSpeed * Time.deltaTime);
            yield return null;
        }

        successFullInteraction?.Invoke();
        yield return new WaitForSeconds(1);
        interactionClosed?.Invoke();
        isInteracting = false;
        Debug.Log("ClosestFlower is "+closestFlower);

        if(findClosestFlower.closestFlowerRef != null){
            findClosestFlower.closestFlowerRef.SetActive(false);
        }

        Vector3 afterInteractionPosition = transform.position + new Vector3(0, .5f, 0);
        while(Vector3.Distance(transform.position,afterInteractionPosition) > distanceUntilInteraction){
            if(Vector3.Distance(transform.position, highestVertex) > (distanceOffset)){
                break;
            }

            transform.position = Vector3.Slerp(transform.position, afterInteractionPosition, interactionSpeed * Time.deltaTime);

            yield return null;
        }

        yield return null;
    }

    private void onDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, distanceOffset);
    }

    private void HandleMove(Vector2 input){
        moveInput = input;
    }

    private void HandleRise(Vector2 input){
        riseInput = input;
    }

    private void Update(){
        if(isInteracting){
            return;
        }

        float rotationAmount = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);

        Vector3 moveVector = transform.forward * moveInput.y * speed * Time.deltaTime;
        moveVector += new Vector3(0, riseInput.y * riseSpeed, 0);
        transform.position += moveVector;
    }
}
