using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class EasyFlow : MonoBehaviour
	{
		static Texture2D m_whiteTexture;
		public static Texture2D whiteTexture
		{
			get
			{
				if (m_whiteTexture == null)
                    m_whiteTexture = (Texture2D)Resources.Load("White", typeof(Texture2D));
				return m_whiteTexture;
			}
		}

		public void InitTexture()
		{
			switch (saveData.textureMode)
			{
			case TEXTURE_MODE.Once:		ChangeTextureOnce(saveData.textureList);	break;
			case TEXTURE_MODE.Loop:		ChangeTextureLoop(saveData.textureList);	break;
			case TEXTURE_MODE.Random:	ChangeTextureRandom(saveData.textureList);	break;
			}
		}

		private void ChangeTextureOnce(List<Texture> textureList)
		{
			int textureCount = textureList == null ? 0 : textureList.Count;
			int coverCount = coverList.Count;
			
			if (textureCount == coverCount)
			{
				for (int i=0; i<textureCount; ++i)
					coverList[i].SetTexture(textureList[i]);
			}
			else if (textureCount > coverCount)
			{
				for (int i=0; i<coverCount; ++i)
					coverList[i].SetTexture(textureList[i]);
			}
			else if (textureCount < coverCount)
			{
				for (int i=0; i<textureCount; ++i)
					coverList[i].SetTexture(textureList[i]);
				for (int i=textureCount; i<coverCount; ++i)
					coverList[i].SetTexture(whiteTexture);
			}
		}
		
		private void ChangeTextureLoop(List<Texture> textureList)
		{
			int textureCount = textureList == null ? 0 : textureList.Count;
			int coverCount = coverList.Count;
			
			if (textureCount == 0)
			{
				for (int i=0; i<coverCount; ++i)
					coverList[i].SetTexture(whiteTexture);
			}
			else if (textureCount == coverCount)
			{
				for (int i=0; i<textureCount; ++i)
					coverList[i].SetTexture(textureList[i]);
			}
			else if (textureCount > coverCount)
			{
				for (int i=0; i<coverCount; ++i)
					coverList[i].SetTexture(textureList[i]);
			}
			else if (textureCount < coverCount)
			{
				int textureIndex = 0;
				for (int i=0; i<coverCount; ++i)
				{
					textureIndex = i % textureCount;
					coverList[i].SetTexture(textureList[textureIndex]);
				}
			}
		}
		
		private void ChangeTextureRandom(List<Texture> textureList)
		{
			int textureCount = textureList == null ? 0 : textureList.Count;
			int textureIndex = 0;
			
			if (textureCount == 0)
			{
				foreach (Cover cover in coverList)
					cover.SetTexture(whiteTexture);
			}
			else if (textureCount == 1)
			{
				foreach (Cover cover in coverList)
				{
					cover.SetTexture(textureList[0]);
				}
			}
			else
			{
				foreach (Cover cover in coverList)
				{
					textureIndex = Random.Range(0, textureCount);
					cover.SetTexture(textureList[textureIndex]);
				}
			}
		}
	}
}