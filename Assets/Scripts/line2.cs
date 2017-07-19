using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line2 : MonoBehaviour {

    public GameObject player;
    public GameObject Explosion;
    public GameObject Pointer;
    public float initialVelocity = 10.0f;
    public float timeResolution = 0.02f;
    public float MaxTime = 10.0f;
    public LayerMask layerMask = -1;
    bool NG = false;
    bool NG2 = false;
    bool havePointer = false;
    Vector3 Point;

    private GameObject ExplosionInstance;
    private GameObject PointerInstance;

    private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!havePointer) {
            PointerInstance = Instantiate(Pointer, Point,Quaternion.Euler(-90,0,0));
            havePointer = true;
        }

        PointerInstance.transform.position = Point;
        if (NG)
        {
            GetComponent<Renderer>().material.color = Color.red;
            PointerInstance.SetActive(false);

        }
        else
        {
            GetComponent<Renderer>().material.color = Color.green;
            PointerInstance.SetActive(true);
        }

        Vector3 velocityVector = transform.forward * initialVelocity;

        lineRenderer.SetVertexCount( ( int ) ( MaxTime / timeResolution ) );

        int index = 0;

        Vector3 currentPosition = transform.position;

        for (float t = 0.0f; t < MaxTime; t += timeResolution) {
            lineRenderer.SetPosition(index, currentPosition);

            RaycastHit hit;

            if (Physics.Raycast(currentPosition, velocityVector, out hit, velocityVector.magnitude, layerMask)) {

                lineRenderer.SetVertexCount(index + 2);

                lineRenderer.SetPosition(index + 1, hit.point);
                if ( Input.GetMouseButtonDown( 0 ) && NG == false ) {
                    player.transform.position = hit.point;
                }
                Point = hit.point;
                Point.y = hit.point.y + 0.01f;
                NG2 = false;
                break;
            } else {
                NG2 = true;
            }
            currentPosition += velocityVector * timeResolution;
            velocityVector += Physics.gravity * timeResolution;
            index++;
        }



        Ray ray = new Ray(Point, Vector3.down);
        RaycastHit hit2;
        if (Physics.Raycast(ray, out hit2))
        {
            Debug.Log(hit2.point);
            Debug.DrawLine(Point, hit2.point, Color.red);
            if (hit2.distance > 1f || hit2.collider.tag == "unstand" || NG2)
            {
                NG = true;
            } else
            {
                NG = false;
            }
        }
    }
}
