using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSplineMove : MonoBehaviour
{
    [SerializeField] List<GameObject> points;
    [SerializeField] float sphereSize = 1.0f;
    [SerializeField] int division = 100;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float moveSphereSize = 1.0f;
    [SerializeField] float moveOffset = 0.1f;
    [SerializeField] int sphereNum = 5;

    Vector3 CatmullRomSplineInterp(Vector3 p_mi1, Vector3 p_0, Vector3 p_1, Vector3 p_2, float t)
    {
        Vector3 a4 = p_0;
        Vector3 a3 = (p_1 - p_mi1) / 2.0f;
        Vector3 a1 = (p_2 - p_0) / 2.0f - 2.0f*p_1 + a3 + 2.0f*a4;
        Vector3 a2 = 3.0f*p_1 - (p_2 - p_0) / 2.0f - 2.0f*a3 - 3.0f*a4;

        return a1*t*t*t + a2*t*t + a3*t + a4;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var point in points)
        {
            Gizmos.DrawSphere(point.transform.position, sphereSize);
        }

        Gizmos.color = Color.red;
        for(int i=0; i< points.Count-3; i++)
        {
            Vector3 prevPos = points[i+1].transform.position;
            for (int j = 1; j <= division; j++)
            {
                float t = j * 1.0f / division;

                Vector3 pos = CatmullRomSplineInterp(points[i].transform.position, points[i + 1].transform.position, points[i + 2].transform.position, points[i + 3].transform.position, t);

                Gizmos.DrawLine(pos, prevPos);
                prevPos = pos;
            }
        }


        for(int i = 0; i < sphereNum; i++)
        {
            int pointsCnt = points.Count - 3;
            float currentTime = (Time.realtimeSinceStartup * speed + i * moveOffset) % pointsCnt;
            //float currentTime = Time.realtimeSinceStartup % pointsCnt;
            int idx = Mathf.FloorToInt(currentTime) + 1;
            Vector3 position = CatmullRomSplineInterp(points[idx - 1].transform.position, points[idx].transform.position, points[idx + 1].transform.position, points[idx + 2].transform.position, currentTime % 1.0f);
            Gizmos.DrawSphere(position, moveSphereSize);
        }

    }
}
