using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ExText))]

public class CustomExText : Editor
{
    ExText exText;
    Dictionary<eMultLanguageTag, string> newStrDic = new Dictionary<eMultLanguageTag, string>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        exText = target as ExText;
        Dictionary<eMultLanguageTag, string> strDic = MultLanguageUtility.GetExTextStrDic(exText.textUID);


        foreach (eMultLanguageTag tag in Enum.GetValues(typeof(eMultLanguageTag)))
        {
            string tagName = Enum.GetName(typeof(eMultLanguageTag), tag);
            string textValue = string.Empty;

            if (newStrDic.ContainsKey(tag))
            {
                textValue = newStrDic[tag];
            }
            else if (strDic != null && strDic.ContainsKey(tag))
            {
                textValue = strDic[tag];
            }
            newStrDic[tag] = EditorGUILayout.TextField(tagName, textValue);
        }

        if (GUILayout.Button("±£¥Ê∂‡”Ô—‘"))
        {
            foreach (KeyValuePair<eMultLanguageTag, string> item in newStrDic)
            {
                MultLanguageUtility.SetExTextStr(exText.textUID, item.Key, item.Value);
            }
            MultLanguageUtility.SaveMultLanguage();
            exText.text = MultLanguageUtility.GetExTextStr(exText.textUID);
        }
    }
}
