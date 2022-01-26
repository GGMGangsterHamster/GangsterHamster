using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Objects.Bezier
{
    public class BezierObject : MonoBehaviour
    {
        public Vector3 startPoint = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 endPoint = new Vector3(-2.0f, 0.0f, 0.0f);
        public Vector3 startTangent = new Vector3(0.0f, -2.0f, 0.0f);
        public Vector3 endTangent = new Vector3(-2.0f, 2.0f, 0.0f);

        /// <summary>
        /// t 에 해당하는 지점을 반환함
        /// </summary>
        public Vector3 CoordAtT(float t)
        {
            Vector3 p1 = Vector3.Lerp(startPoint, startTangent, t);
            Vector3 p2 = Vector3.Lerp(startTangent, endTangent, t);
            Vector3 p3 = Vector3.Lerp(endTangent, endPoint, t);

            Vector3 q1 = Vector3.Lerp(p1, p2, t);
            Vector3 q2 = Vector3.Lerp(p2, p3, t);

            return Vector3.Lerp(q1, q2, t);
        }
    }



    #region Draws bezier curve to scene
    #if UNITY_EDITOR
    [CustomEditor(typeof(BezierObject))]
    public class BezierTestEditor : Editor
    {
        private void OnSceneViewGUI(SceneView sv)
        {
            BezierObject be = target as BezierObject;
            be.startPoint = Handles.PositionHandle(be.startPoint, Quaternion.identity);
            be.endPoint = Handles.PositionHandle(be.endPoint, Quaternion.identity);
            be.startTangent = Handles.PositionHandle(be.startTangent, Quaternion.identity);
            be.endTangent = Handles.PositionHandle(be.endTangent, Quaternion.identity);

            Handles.DrawBezier(be.startPoint, be.endPoint, be.startTangent, be.endTangent, Color.white, null, 2f);
            Handles.DrawLine(be.startPoint, be.startTangent, 2.0f);
            Handles.DrawLine(be.endPoint, be.endTangent, 2.0f);
        }

        void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneViewGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneViewGUI;
        }
    }
    #endif
    #endregion

}