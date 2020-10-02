using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sample : MonoBehaviour
{
    public bool showGUI = false;
    public List<GameObject> list;
    private GameObject prevSelectObj = null;
    private NAsoft_EasyFlow.EasyFlow currentEasyflow = null;

    public void Awake()
    {
        OnButton(0);
        moveButtonIndex = Random.Range(0, currentEasyflow.coverList.Count);
    }

    public void OnButton(int num)
    {
        if (list == null || list.Count <= num)
            return;

        GameObject obj = list[num];
        if (prevSelectObj != null)
        {
            if (obj == prevSelectObj)
                return;
            prevSelectObj.SetActive(false);
        }

        obj.SetActive(true);
        prevSelectObj = obj;

        currentEasyflow = obj.GetComponent<NAsoft_EasyFlow.EasyFlow>();
    }

    private const int width = 95;
    private const int height = 45;
    private static int space = 0;
    private int moveButtonIndex = 0;

    public void OnGUI()
    {
        if (showGUI)
        {
            if (list == null ||
                list.Count == 0)
                return;

            space = (Screen.width - (width * 7) - (5 * 6)) / 2;

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(space);
                for (int i = 0; i < 7; ++i)
                {
                    if (list[i] == prevSelectObj)
                        GUI.enabled = false;
                    if (GUILayout.Button(string.Format("Preset {0:00}", i + 1), GUILayout.Width(width), GUILayout.Height(height)))
                        OnButton(i);
                    if (list[i] == prevSelectObj)
                        GUI.enabled = true;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(space);
                for (int i = 7; i < 14; ++i)
                {
                    if (list[i] == prevSelectObj)
                        GUI.enabled = false;
                    if (GUILayout.Button(string.Format("Preset {0:00}", i + 1), GUILayout.Width(width), GUILayout.Height(height)))
                        OnButton(i);
                    if (list[i] == prevSelectObj)
                        GUI.enabled = true;
                }
            }
            GUILayout.EndHorizontal();
        }

        DrawMoveButton();
    }

    private void DrawMoveButton()
    {
        Rect rt = new Rect(Screen.width * 0.5f - 210.0f, Screen.height - 100.0f, 100.0f, 20.0f);
        if (GUI.Button(rt, "Move to first"))
            currentEasyflow.MoveToFirst();

        rt.x += 105.0f;
        if (GUI.Button(rt, "Prev"))
            currentEasyflow.MovePrev();

        rt.x += 105.0f;
        if (GUI.Button(rt, "Next"))
            currentEasyflow.MoveNext();

        rt.x += 105.0f;
        if (GUI.Button(rt, "Move to last"))
            currentEasyflow.MoveToLast();

        rt.x = Screen.width * 0.5f - 105.0f;
        rt.y += 25.0f;
        if (GUI.Button(rt, "Move to :"))
        {
            currentEasyflow.MoveTo(moveButtonIndex);
            moveButtonIndex = Random.Range(0, currentEasyflow.coverList.Count);
        }

        rt.x += 105.0f;
        string text = moveButtonIndex.ToString();
        text = GUI.TextField(rt, text);
        if (!int.TryParse(text, out moveButtonIndex))
            text = moveButtonIndex.ToString();
    }
}