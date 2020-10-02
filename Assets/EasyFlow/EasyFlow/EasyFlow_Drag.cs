using UnityEngine;
using System.Collections;

namespace NAsoft_EasyFlow
{
	public partial class EasyFlow : MonoBehaviour
	{
		float moveSpeed = 0.5f;
		
		public void Drag(Vector2 delta)
		{
			Vector3 delta3 = delta;
			if (saveData.isDragOnAxis == BOOL.Yes)
			{
				Vector3 axisDirection = saveData.coverDistance.normalized;
				float dragDistance = delta.magnitude;
				Vector3 deltaDistanceOnAxis = axisDirection * dragDistance * saveData.dragPower;
				Vector3 dragDirection = delta.normalized;
				float dot = Vector3.Dot(dragDirection, axisDirection);
				if (dot >= 0.0f)
					delta3 = deltaDistanceOnAxis;
				else
					delta3 = -deltaDistanceOnAxis;
			}

			Vector3 deltaPos = delta3 * moveSpeed;
			flowPosition += deltaPos;
			UpdateCover();

			if (saveData.isEffectAfterDrag == BOOL.Yes)
				AccureDragDistance(deltaPos);
		}
	}
}