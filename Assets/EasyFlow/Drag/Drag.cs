using UnityEngine;
using System.Collections;

namespace NAsoft_EasyFlow
{
    [RequireComponent(typeof(EasyFlow))]
    public class Drag : MonoBehaviour
    {
        public EasyFlow easyflow;
        protected Vector2 lastPos = Vector2.zero;
        protected bool isBeginDrag = false;

        public void OnDisable()
        {
            Destroy(this);
        }

        protected void _OnMouseDown(Vector2 pos)
        {
            isBeginDrag = true;
            easyflow._OnMouseDown();
            lastPos = pos;
        }

        protected void _OnMouseDrag(Vector2 pos)
        {
            if (isBeginDrag == false)
                return;

            Vector2 delta = lastPos - pos;
            delta.y = -delta.y; // Reverse y-axis : World axis <-> Screen axis
            easyflow._OnMouseDrag(delta);
            lastPos = pos;
        }

        protected void _OnMouseUp(Vector2 pos)
        {
            if (isBeginDrag == false)
                return;

            isBeginDrag = false;
            easyflow._OnMouseUp();
            lastPos = Vector2.zero;
        }
    }
}