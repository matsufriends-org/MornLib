
using MornUtil;
#if USE_TEXTMESHPRO
using TMPro;
using UnityEngine.EventSystems;
#endif
#if UNITY_EDITOR && USE_TEXTMESHPRO
using UnityEditor;
using TMPro.EditorUtilities;
#endif

namespace MornLib.Mono
{
#if USE_TEXTMESHPRO
    public class MornTMPDropdown : TMP_Dropdown
    {
        public bool IsClickOnMouseRight;
        public bool IsClickOnMouseMiddle;
        public bool IsClickOnMouseLeft;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.IsLeftClick() && IsClickOnMouseLeft) Show();

            if (eventData.IsMiddleClick() && IsClickOnMouseMiddle) Show();

            if (eventData.IsRightClick() && IsClickOnMouseRight) Show();
        }
    }
#endif
#if USE_TEXTMESHPRO && UNITY_EDITOR
    [CustomEditor(typeof(MornTMPDropdown))]
    public class MornTMPDropdownEditor : DropdownEditor
    {
        public override void OnInspectorGUI()
        {
            var dropdown = (MornTMPDropdown)target;
            dropdown.IsClickOnMouseRight = EditorGUILayout.Toggle("IsClickOnMouseRight", dropdown.IsClickOnMouseRight);
            dropdown.IsClickOnMouseMiddle =
                EditorGUILayout.Toggle("IsClickOnMouseMiddle", dropdown.IsClickOnMouseMiddle);
            dropdown.IsClickOnMouseLeft = EditorGUILayout.Toggle("IsClickOnMouseLeft", dropdown.IsClickOnMouseLeft);
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
#endif
}