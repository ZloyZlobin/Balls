using UnityEngine;
using Balls.Game;

namespace Balls.UI
{
    public class IngameUI : MonoBehaviour
    {
        private void OnGUI()
        {
            GUI.BeginGroup(new Rect(10f, Screen.height - 180f, 150f, 150f));
            GUILayout.BeginVertical();
            GUILayout.Label("In game: " + StatisticSystem.InGame);
            GUILayout.Label("Started: " + StatisticSystem.Started);
            GUILayout.Label("Missed: " + StatisticSystem.Missed);
            GUILayout.Label("Canceled: " + StatisticSystem.Canceled);
            GUILayout.Label("Collisions: " + StatisticSystem.Collisions);
            if (GUILayout.Button("Reset"))
                StatisticSystem.Reset();
            GUILayout.EndVertical();
            GUI.EndGroup();
        }
    }
}
