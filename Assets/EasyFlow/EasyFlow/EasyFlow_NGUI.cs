using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class EasyFlow_NGUI : EasyFlow
	{
		protected override void CreateCover()
		{
#if NGUI_USED
			UITexture uitexture = NGUITools.AddWidget<UITexture>(panel.gameObject);
			uitexture.mainTexture = EasyFlow.whiteTexture;
			
			Cover_NGUI newCover = uitexture.gameObject.AddComponent<Cover_NGUI>();
			coverList.Add(newCover);
#endif
		}

		protected override void InitPanelDepth()
		{
#if NGUI_USED
			UIPanel uipanel = panel.GetComponent<UIPanel>();
			uipanel.depth = (int)saveData.panelDepth;
#endif
		}
	}
}