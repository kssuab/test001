using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAsoft_EasyFlow
{
#if UGUI_USED

    [RequireComponent(typeof(RectTransform))]
    public partial class EasyFlow_UGUI : EasyFlow
    {
        protected override void CreateCover()
        {
            GameObject coverObject = new GameObject();
            coverObject.transform.SetParent(panel);

            coverObject.AddComponent<RectTransform>();

            Canvas canvas = coverObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;

            coverObject.AddComponent<CanvasRenderer>();
            coverObject.AddComponent<UnityEngine.UI.RawImage>();

            Cover_UGUI newCover = coverObject.AddComponent<Cover_UGUI>();
            coverList.Add(newCover);
        }

        protected override void InitPanelDepth()
        {
            Canvas canvas = panel.GetComponent<Canvas>();
            canvas.sortingOrder = (int)saveData.panelDepth;
        }
    }

#elif !UGUI_USED
    public partial class EasyFlow_UGUI : EasyFlow
	{
	}
#endif
}