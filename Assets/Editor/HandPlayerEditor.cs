using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HandPlayer))]
class HandPlayerEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HandPlayer player = (HandPlayer)target;
        if (GUILayout.Button("Play"))
        {
            player.PlayRecords();
        }
    }
}
