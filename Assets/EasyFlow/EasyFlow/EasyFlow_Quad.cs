using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NAsoft_EasyFlow
{
	public partial class EasyFlow_Quad : EasyFlow
	{
		protected override void CreateCover()
		{
            GameObject coverPrefab = (GameObject)Resources.Load("Prefab/Prefab_Quad", typeof(GameObject));
			GameObject coverObject = GameObject.Instantiate(coverPrefab) as GameObject;
            coverObject.transform.parent = panel;
            Material mat = (Material)Resources.Load("Prefab/Material_Quad", typeof(Material));
            coverObject.GetComponent<Renderer>().sharedMaterial = (Material)Material.Instantiate(mat);
			
			Cover_Quad newCover = coverObject.GetComponent<Cover_Quad>();
			coverList.Add(newCover);
		}

		protected override void InitPanelDepth()
		{
			//panel.SetSiblingIndex((int)saveData.panelDepth);
			//Vector3 pos = panel.localPosition;
			//pos.z = saveData.panelDepth;
			//panel.localPosition = pos;
		}
	}
}