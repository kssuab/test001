using System.Collections;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    [RequireComponent(typeof(EasyFlow))]
    public class Drag_onScreen : Drag
    {
        protected Rect dragRect;
        private ScreenOrientation screenOrientation;

        public void Awake()
        {
            easyflow = GetComponent<EasyFlow>();

            screenOrientation = Screen.orientation;
            CalcDragRect();
        }

        private void CalcDragRect()
        {
            Rect rect = easyflow.saveData.dragRect;

            float x = Screen.width * rect.x;
            float y = Screen.height * rect.y;
            float w = Screen.width * rect.width;
            float h = Screen.height * rect.height;

            dragRect = new Rect(x, y, w, h);
        }

        public void OnGUI()
        {
            if (Event.current == null)
                return;

            if (screenOrientation != Screen.orientation)
                CalcDragRect();

            if (dragRect.Contains(Event.current.mousePosition) || isBeginDrag)
            {
                switch (Event.current.type)
                {
                    case EventType.MouseDown: _OnMouseDown(Event.current.mousePosition); break;
                    case EventType.MouseDrag: _OnMouseDrag(Event.current.mousePosition); break;
                    case EventType.MouseUp: _OnMouseUp(Event.current.mousePosition); break;
                }
            }
        }
    }
}