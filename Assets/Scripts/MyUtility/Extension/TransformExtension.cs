using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public class ExTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public ExTransform(Transform transform)
        {
            position = transform.localPosition;
            rotation = transform.localRotation;
            scale = transform.localScale;
        }

    }
}
