using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

[CreateAssetMenu(menuName ="Animation duration list")]
public class AnimationDurationList : ScriptableObject
{
    [SerializeField]
    public List<AnimationData> animations;
#if UNITY_EDITOR
    [HideInInspector]
    public AnimatorController animatorController;
    public void RefreshList()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty serializedProperty = serializedObject.FindProperty("animations");
        animations.Clear();
        serializedObject.Update();
        for (int i = 0; i < animatorController.layers[0].stateMachine.states.Length; i++)
        {
            AnimatorState state = animatorController.layers[0].stateMachine.states[i].state;
            if (state.motion == null)
            {
                serializedProperty.InsertArrayElementAtIndex(i);
                AnimationData tmp2 = new AnimationData(state.name, 0);
                serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue = tmp2.name;
                serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("duration").floatValue = tmp2.duration;
                continue;
            }
            serializedProperty.InsertArrayElementAtIndex(i);
            serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue = state.name;
            serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("duration").floatValue = state.motion.averageDuration;
        }
        serializedObject.ApplyModifiedProperties();
    }
#endif
}
