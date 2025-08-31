using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageColorData", menuName = "Game/Stage Color Data")]
public class StageColorData : ScriptableObject
{
    [System.Serializable]
    public class StageColors
    {
        public Color backgroundColor;
        public Color fromTextColor;
        public Color toTextColor;
        public Color circleColor = Color.white;
    }
    public StageColors[] stageColors;
}
