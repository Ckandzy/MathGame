using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public enum EnumMoveMode
    {
        StraightLine = 0,
        InnerCycloid,
        OuterCycloid,
    }
    public EnumMoveMode moveMode = EnumMoveMode.StraightLine;
    public float moveSpeed = 1f;
    public Transform center;

    public float lastDegree;

    // Start is called before the first frame update
    void Start()
    {
        //lastDegree = Mathf.Acos((transform.position - center.position).x / (transform.position - center.position).magnitude);
        lastDegree = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float speed = moveSpeed * Time.deltaTime;
        float radius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
        switch (moveMode)
        {
            case EnumMoveMode.StraightLine:
                {
                    transform.localPosition += new Vector3(speed, 0, 0);
                    // degree per second = 360 / (2*PI*r/speed)
                    transform.Rotate(0, 0, -(360 / (2 * Mathf.PI * radius / moveSpeed)) * Time.deltaTime, Space.Self);
                }break;
            case EnumMoveMode.InnerCycloid:
                {
                    float rCenter = (center.position - transform.position).magnitude;
                    float rEdge = rCenter + radius;
                    float perimeterEdge = 2 * rEdge * Mathf.PI;
                    //Debug.Log(rCenter + "," + rEdge + "," + perimeterEdge + "," + 2 * rEdge * radius);
                    transform.localPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (lastDegree + 360 / (perimeterEdge / speed))) * rCenter,
                        Mathf.Sin(Mathf.Deg2Rad * (lastDegree + 360 / (perimeterEdge / speed))) * rCenter,
                        0);
                    lastDegree = (lastDegree + 360 / (perimeterEdge / speed)) % 360;

                    transform.Rotate(0, 0, -(360 / (perimeterEdge / speed) * rCenter / rEdge), Space.Self);
                }break;
            case EnumMoveMode.OuterCycloid:
                {

                }break;
        }
    }
}
