using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeonBrave
{
    public class SplinePoints : MonoBehaviour
    {
        [SerializeField] private List<SplinePoint> _splinePoints;

        public List<SplinePoint> SplinePointList
        {
            get
            {
               return _splinePoints;
            }
        }

        private void OnDrawGizmos()
        {

            for (int i = 0; i < _splinePoints.Count; i++) 
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_splinePoints[i].PointTransform.position, .15f);

                Transform p0 = _splinePoints[i].PointTransform;
                Transform p1;

                if (i == _splinePoints.Count - 1) 
                {
                    p1 = _splinePoints[0].PointTransform; 
                }
                else
                {
                    p1 = _splinePoints[i + 1].PointTransform;
                }

                Gizmos.color = Color.white;

                if (i < _splinePoints.Count - 1)
                {
                    Gizmos.DrawLine(_splinePoints[i].PointTransform.position, _splinePoints[i + 1].PointTransform.position);
                }
                else
                {
                    Gizmos.DrawLine(_splinePoints[i].PointTransform.position, _splinePoints[0].PointTransform.position); 
                }

                Vector3 lastPoint = p0.position;
                for (float t = 0.05f; t <= 1; t += 0.05f)
                {
                    Gizmos.color = Color.green;
                    Vector3 point = CalculateBezierPoint(t, p0.position, p0.position + p0.forward, p1.position - p1.forward, p1.position);
                    Gizmos.DrawLine(lastPoint, point);
                    lastPoint = point;
                }
            }
        }

        
       public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

    }
}