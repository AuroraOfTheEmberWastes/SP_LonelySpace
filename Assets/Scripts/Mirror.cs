using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Transform point2;
    public Transform point3;
    public List<GameObject> mirroredObjects;

    public GameObject mirroredRepresentation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 toPoint2 = GetVector(point2.position);
        Vector3 toPoint3 = GetVector(point3.position);

        Vector3 normal = Vector3.Normalize(Vector3.Cross(toPoint2, toPoint3));

        foreach(GameObject duck in mirroredObjects)
        {
            //despite the name, this vec tor goes away from this point
            Vector3 incident = duck.transform.position - transform.position;
            float dot = Vector3.Dot(Vector3.Normalize(incident), normal);

            Vector3 toMirror = -normal * dot * incident.magnitude;

            Instantiate(mirroredRepresentation, duck.transform.position + (2 * toMirror), Quaternion.identity);
        }

	}

    // Update is called once per frame
    void Update()
    {
        
    }


    private Vector3 GetVector(Vector3 otherPoint)
    {
        Vector3 toPoint = otherPoint - transform.position;
        return toPoint;
    }
}
