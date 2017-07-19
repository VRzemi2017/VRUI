using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line2 : MonoBehaviour {

    public GameObject player;
    public GameObject Pointer;
    public float Angle = 45.0f;

    public float initialVelocity = 10.0f;
    public float timeResolution = 0.02f;
    public float MaxTime = 10.0f;

    public LayerMask layerMask = -1;

    bool NG = false;
    bool NG2 = false;
    bool NG3 = false;
    bool havePointer = false;

    Vector3 Point;
    Vector3 defaultPos;
    Quaternion defaultRotation;
    public float rayDistance;

    private GameObject ExplosionInstance;
    private GameObject PointerInstance;

    private LineRenderer lineRenderer;


    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();

        defaultPos = Pointer.transform.localPosition;
        defaultRotation = Pointer.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {

        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>( );
        var device = SteamVR_Controller.Input( ( int )trackedObject.index);

        if (!havePointer) {
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
        currentPosition.y = transform.position.y - 0.25f;

        for (float t = 0.0f; t < MaxTime; t += timeResolution) {
            lineRenderer.SetPosition(index, currentPosition);

            RaycastHit hit;

            if (Physics.Raycast(currentPosition, velocityVector, out hit, velocityVector.magnitude, layerMask)) {

                lineRenderer.SetVertexCount(index + 2);

                lineRenderer.SetPosition(index + 1, hit.point);

                if ( device.GetTouchDown( SteamVR_Controller.ButtonMask.Trigger ) && NG == false ) {
                    player.transform.position = hit.point;
                }
                Point = hit.point;

                PointerInstance.transform.rotation = Quaternion.LookRotation( hit.normal );
                if ( Vector3.Angle( hit.normal, Vector3.up ) >= Angle ) {
                    NG3 = true;
                } else {
                    NG3 = false;
                }

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
        if (Physics.Raycast(ray, out hit2)) {
            Debug.Log(hit2.point);
            Debug.DrawLine(Point, hit2.point, Color.red);
            if (hit2.distance > 1f || hit2.collider.tag == "unstand" || NG2 || NG3) {
                NG = true;
            } else {
                NG = false;
            }
        }


    }
}
