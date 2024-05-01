using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestFlower : MonoBehaviour
{
    public GameObject[] flowers; // An array of flower objects to search through

    public GameObject closestFlowerObject { get; private set; } // Reference to the closest flower object
    public Vector3 HighestVertexClosestFlower { get; private set; } // Highest vertex of the closest flower
    public GameObject closestFlowerRef { get; private set; } // Reference to the closest flower


    
    private void Update()
    {   
        // VegetationOnMesh vegetationScript = (ARMesh).GetComponent<VegetationOnMesh>();
        // if (vegetationScript != null){
        //     Debug.Log("vegetationScript has something");
        //     flowers = vegetationScript.GetGroundPrefabsArray();
        // }
        FindClosestFlowerObject();
    }

    private void FindClosestFlowerObject()
    {
        if (flowers.Length == 0)
        {
            closestFlowerObject = null; // No flowers in the scene
            return;
        }

        float closestDistance = Mathf.Infinity;
        Vector3 dragonPosition = transform.position; // Assuming this script is on the dragon

        foreach (GameObject flower in flowers)
        {
            Vector3 flowerPosition = flower.transform.position;
            float distance = Vector3.Distance(dragonPosition, flowerPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFlowerObject = flower;
                closestFlowerRef = flower;

                // To get the highest vertex of the closest flower (assuming Y is the vertical axis)
                Vector3[] vertices = flower.GetComponent<MeshFilter>().mesh.vertices;
                float highestY = float.MinValue;

                foreach (Vector3 vertex in vertices)
                {
                    if (vertex.y > highestY)
                    {
                        highestY = vertex.y;
                    }
                }

                HighestVertexClosestFlower = flowerPosition + new Vector3(0, highestY, 0);
            }
}
}
}