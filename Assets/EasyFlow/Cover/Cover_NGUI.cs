using UnityEngine;
using System.Collections;

namespace NAsoft_EasyFlow
{
#if NGUI_USED
	[RequireComponent (typeof(UIWidget))]
#endif
	public class Cover_NGUI : Cover
	{
#if NGUI_USED
		private UIWidget m_widget;
		private UIWidget widget
		{
			get
			{
				if (m_widget == null)
				{
					m_widget = GetComponent<UIWidget>();
				}
				return m_widget;
			}
			set
			{
				m_widget = value;
			}
		}

		public override void SetTexture(Texture texture)
		{
			widget.mainTexture = texture;
			NGUITools.SetDirty(this);
		}

		public override void SetSize(Vector2 size)
		{
			widget.SetDimensions((int)size.x, (int)size.y);
			NGUITools.SetDirty(this);
		}
		
		public override void UpdateAlpha(Vector3 flowPosition)
		{
			widget.alpha = CalcAlpha(flowPosition);
			NGUITools.SetDirty(this);
		}

		public override void UpdateScale(Vector3 flowPosition)
		{
			transform.localScale = CalcScale(flowPosition) * Vector3.one;
		}

		public override void UpdateDepth(Vector3 flowPosition)
		{
			widget.depth = CalcDepth(flowPosition);
			NGUITools.SetDirty(this);
		}
#endif
	}
}