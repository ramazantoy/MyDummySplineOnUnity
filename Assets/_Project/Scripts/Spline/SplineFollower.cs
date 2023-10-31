using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeonBrave
{
    public class SplineFollower : MonoBehaviour
    {
        public SplinePoints spline;
        public float speed = 1.0f;

        [SerializeField] private bool _canMove = false;

        private Vector3 previousPosition;

        [SerializeField] [Range(0, 1)] private float _T;

        private void Start()
        {
            if (spline && spline.SplinePointList.Count > 0)
            {
                previousPosition = transform.position;
            }
        }

        private void Update()
        {
            FollowSpline();
        }

        private void FollowSpline()
        {
            if (!_canMove || spline == null || spline.SplinePointList.Count < 2) return;

            _T += Time.deltaTime * speed;
            if (_T > 1.0f)
            {
                _T -= 1.0f;
            }

            MoveToTValue(_T);
        }

        private void MoveToTValue(float tValue)
        {
            int segment = Mathf.FloorToInt(tValue * spline.SplinePointList.Count);
            Transform p0 = spline.SplinePointList[segment % spline.SplinePointList.Count].PointTransform;
            Transform p1 = spline.SplinePointList[(segment + 1) % spline.SplinePointList.Count].PointTransform;

            float localT = tValue * spline.SplinePointList.Count - segment;
            Vector3 position = spline.CalculateBezierPoint(localT, p0.position, p0.position + p0.forward,
                p1.position - p1.forward, p1.position);

            Vector3 direction = (position - previousPosition).normalized;
            if (direction != Vector3.zero)
            {
                transform.forward = direction;
            }

            transform.position = position;
            previousPosition = position;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (spline && spline.SplinePointList.Count > 0)
            {
                if (_T >= 1)
                {
                    _T = 0;
                }

                MoveToTValue(_T);
            }
        }
#endif
    }
}