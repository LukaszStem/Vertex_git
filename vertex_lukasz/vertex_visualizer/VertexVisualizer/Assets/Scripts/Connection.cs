using UnityEngine;
using System.Collections;

public class Connection : MonoBehaviour
{
    public float weight;
    public float radialScale;

    private float minValue;
    private float maxValue;
    
    public void CreateConneciton(float weight, Vector3 start, Vector3 end)
    {
        this.weight = weight;
        float xPos = (start.x + end.x) / 2;
        float yPos = (start.y + end.y) / 2;
        float zPos = (start.z + end.z) / 2;
        Vector3 scale = new Vector3(3f, Vector3.Distance(start, end) / 2, 3f);

        this.transform.localScale = scale;
        this.transform.localPosition = new Vector3(xPos, yPos, zPos);

        this.transform.up = end - start;
    }


}
