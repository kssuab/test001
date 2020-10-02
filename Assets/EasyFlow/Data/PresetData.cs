using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	[Serializable]
	public class PresetData : ScriptableObject
	{
		public Texture2D texture;
#region Cover
		public float coverDistanceF;
		public float coverDistanceLimit;

		public float coverAngle;

		public Vector2 coverSize;
		public Vector2 coverSizeLimit;

		public float coverDistanceZ;
		public float coverDistanceZLimit;
		public COVER_DISTANCE_Z_MODE coverDistanceZMode;
#endregion
		
#region Texture
		public List<Texture> textureList;
		public TEXTURE_MODE textureMode;
#endregion
		
#region Drag
		public float dragPower;
		public float dragPowerLimit;
		
		public DRAG_MODE dragMode;
		public Rect dragRect;
		
		public BOOL isInverseDrag;
		public BOOL isDragOnAxis;
		public BOOL isEffectAfterDrag;
		public BOOL isMoveToNearCover;
#endregion
		
#region Effect After Drag
		public float effectAfterDragTime;
		public float effectAfterDragTimeLimit;
		public AnimationCurve effectAfterDragCurve;
#endregion
		
#region Position
		public float positionRate;
		public float positionRateLimit;
		
		public float positionInfluenceRange;
		public float positionInfluenceRangeLimit;
		
		public AnimationCurve positionCurve;
#endregion
		
#region Rotate
		public BOOL isLookatCenter;
		public BOOL isRotateOnAxis;
		
		public float rotateRate;
		public float rotateRateLimit;
		
		public float rotateInfluenceRange;
		public float rotateInfluenceRangeLimit;
		
		public AnimationCurve rotateCurve;
#endregion
		
#region Scale
		public float scaleRate;
		public Vector2 scaleRateLimit;	// min ~ max
		
		public float scaleInfluenceRange;
		public float scaleInfluenceRangeLimit;
		
		public AnimationCurve scaleCurve;
#endregion
		
#region Alpha
		public float alphaRate;
		public float alphaRateLimit;	// min ~ max
		
		public float alphaInfluenceRange;
		public float alphaInfluenceRangeLimit;
		
		public AnimationCurve alphaCurve;
#endregion
		
#region Depth
		public float panelDepth;
		public Vector2 panelDepthLimit;	// min ~ max
		
		public Vector2 coverDepth;	// min ~ max
		public Vector2 coverDepthLimit;	// min ~ max
		
		public float depthInfluenceRange;
		public float depthInfluenceRangeLimit;
		
		public AnimationCurve depthCurve;
#endregion
		
#region Other
		public BOOL isPanelMove;
		public float beginFlowIndex;
#endregion

		public static void Save(SaveData saveData)
		{
#if UNITY_EDITOR
			PresetData presetData = ScriptableObject.CreateInstance<PresetData>();
			presetData.LoadFromSaveData(saveData);

			string filePath = string.Format("{0}/Preset/PresetData_{1}_{2}.asset",
			                                PathEF.assetPath, saveData.GetHashCode(), UnityEngine.Random.Range(int.MinValue, int.MaxValue));
			UnityEditor.AssetDatabase.CreateAsset(presetData, filePath);
			UnityEditor.AssetDatabase.SaveAssets();
			UnityEditor.AssetDatabase.Refresh();
#endif
		}

		private void LoadFromSaveData(SaveData saveData)
		{
#region Cover
			coverSize = saveData.coverSize;
			coverSizeLimit = saveData.coverSizeLimit;
			
			coverAngle = saveData.coverAngle;

			coverDistanceF = saveData.coverDistanceF;
			coverDistanceLimit = saveData.coverDistanceLimit;
			
			coverDistanceZ = saveData.coverDistanceZ;
			coverDistanceZLimit = saveData.coverDistanceZLimit;
			coverDistanceZMode = saveData.coverDistanceZMode;
#endregion

#region Texture
			textureList = saveData.textureList.GetRange(0, saveData.textureList.Count);
			textureMode = saveData.textureMode;
#endregion

#region Drag
			dragPower = saveData.dragPower;
			dragPowerLimit = saveData.dragPowerLimit;
			
			dragMode = saveData.dragMode;
			dragRect = saveData.dragRect;
			
			isInverseDrag = saveData.isInverseDrag;
			isDragOnAxis = saveData.isDragOnAxis;
#endregion
			
#region Effect After Drag
			isEffectAfterDrag = saveData.isEffectAfterDrag;
			isMoveToNearCover = saveData.isMoveToNearCover;
			
			effectAfterDragTime = saveData.effectAfterDragTime;
			effectAfterDragTimeLimit = saveData.effectAfterDragTimeLimit;
			
			effectAfterDragCurve = saveData.effectAfterDragCurve;
#endregion
			
#region Position
			positionRate = saveData.positionRate;
			positionRateLimit = saveData.positionRateLimit;
			
			positionInfluenceRange = saveData.positionInfluenceRange;
			positionInfluenceRangeLimit = saveData.positionInfluenceRangeLimit;
			
			positionCurve = saveData.positionCurve;
#endregion
			
#region Rotate
			isLookatCenter = saveData.isLookatCenter;
			isRotateOnAxis = saveData.isRotateOnAxis;
			
			rotateRate = saveData.rotateRate;
			rotateRateLimit = saveData.rotateRateLimit;
			
			rotateInfluenceRange = saveData.rotateInfluenceRange;
			rotateInfluenceRangeLimit = saveData.rotateInfluenceRangeLimit;
			
			rotateCurve = saveData.rotateCurve;
#endregion
			
#region Scale
			scaleRate = saveData.scaleRate;
			scaleRateLimit = saveData.scaleRateLimit;
			
			scaleInfluenceRange = saveData.scaleInfluenceRange;
			scaleInfluenceRangeLimit = saveData.scaleInfluenceRangeLimit;
			
			scaleCurve = saveData.scaleCurve;
#endregion

#region Alpha
			alphaRate = saveData.alphaRate;
			alphaRateLimit = saveData.alphaRateLimit;
			
			alphaInfluenceRange = saveData.alphaInfluenceRange;
			alphaInfluenceRangeLimit = saveData.alphaInfluenceRangeLimit;
			
			alphaCurve = saveData.alphaCurve;
#endregion
			
#region Depth
			panelDepth = saveData.panelDepth;
			panelDepthLimit = saveData.panelDepthLimit;
			
			coverDepth = saveData.coverDepth;
			coverDepthLimit = saveData.coverDepthLimit;
			
			depthInfluenceRange = saveData.depthInfluenceRange;
			depthInfluenceRangeLimit = saveData.depthInfluenceRangeLimit;
			
			depthCurve = saveData.depthCurve;
#endregion
			
#region Other
			isPanelMove = saveData.isPanelMove;
			beginFlowIndex = saveData.beginFlowIndex;
#endregion
		}

		public void LoadToSaveData(ref SaveData saveData)
		{
#region Cover
			saveData.coverSize = coverSize;
			saveData.coverSizeLimit = coverSizeLimit;
			
			saveData.coverAngle = coverAngle;

			saveData.coverDistanceF = coverDistanceF;
			saveData.coverDistanceLimit = coverDistanceLimit;
			
			saveData.coverDistanceZ = coverDistanceZ;
			saveData.coverDistanceZLimit = coverDistanceZLimit;
			saveData.coverDistanceZMode = coverDistanceZMode;
#endregion

#region Texture
			saveData.textureList = textureList.GetRange(0, textureList.Count);
			saveData.textureMode = textureMode;
#endregion
			
#region Drag
			saveData.dragPower = dragPower;
			saveData.dragPowerLimit = dragPowerLimit;
			
			saveData.dragMode = dragMode;
			saveData.dragRect = dragRect;
			
			saveData.isInverseDrag = isInverseDrag;
			saveData.isDragOnAxis = isDragOnAxis;
#endregion
			
#region Effect After Drag
			saveData.isEffectAfterDrag = isEffectAfterDrag;
			saveData.isMoveToNearCover = isMoveToNearCover;
			
			saveData.effectAfterDragTime = effectAfterDragTime;
			saveData.effectAfterDragTimeLimit = effectAfterDragTimeLimit;
			
			saveData.effectAfterDragCurve = effectAfterDragCurve;
#endregion
			
#region Position
			saveData.positionRate = positionRate;
			saveData.positionRateLimit = positionRateLimit;
			
			saveData.positionInfluenceRange = positionInfluenceRange;
			saveData.positionInfluenceRangeLimit = positionInfluenceRangeLimit;
			
			saveData.positionCurve = positionCurve;
#endregion
			
#region Rotate
			saveData.isLookatCenter = isLookatCenter;
			saveData.isRotateOnAxis = isRotateOnAxis;
			
			saveData.rotateRate = rotateRate;
			saveData.rotateRateLimit = rotateRateLimit;
			
			saveData.rotateInfluenceRange = rotateInfluenceRange;
			saveData.rotateInfluenceRangeLimit = rotateInfluenceRangeLimit;
			
			saveData.rotateCurve = rotateCurve;
#endregion
			
#region Scale
			saveData.scaleRate = scaleRate;
			saveData.scaleRateLimit = scaleRateLimit;
			
			saveData.scaleInfluenceRange = scaleInfluenceRange;
			saveData.scaleInfluenceRangeLimit = scaleInfluenceRangeLimit;
			
			saveData.scaleCurve = scaleCurve;
#endregion

#region Alpha
			saveData.alphaRate = alphaRate;
			saveData.alphaRateLimit = alphaRateLimit;
			
			saveData.alphaInfluenceRange = alphaInfluenceRange;
			saveData.alphaInfluenceRangeLimit = alphaInfluenceRangeLimit;
			
			saveData.alphaCurve = alphaCurve;
#endregion
			
#region Depth
			saveData.panelDepth = panelDepth;
			saveData.panelDepthLimit = panelDepthLimit;
			
			saveData.coverDepth = coverDepth;
			saveData.coverDepthLimit = coverDepthLimit;
			
			saveData.depthInfluenceRange = depthInfluenceRange;
			saveData.depthInfluenceRangeLimit = depthInfluenceRangeLimit;
			
			saveData.depthCurve = depthCurve;
#endregion
			
#region Other
			saveData.isPanelMove = isPanelMove;
			saveData.beginFlowIndex = beginFlowIndex;
#endregion
		}
	}
}