using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Enigma))]
public class EnigmaEditor : Editor
{
    SerializedProperty rewardProp;
    SerializedProperty objProp;
    SerializedProperty disableObjectProp;
    SerializedProperty obstacleProp;
    SerializedProperty orderProp;
    SerializedProperty positionProp;
    SerializedProperty activationProp;
    SerializedProperty spotsProp;
    //SerializedProperty checkOrderProp;
    //SerializedProperty orderProp;

    private void OnEnable()
    {
        rewardProp = serializedObject.FindProperty("reward");
        objProp = serializedObject.FindProperty("objReward");
        disableObjectProp = serializedObject.FindProperty("disableObstacle");
        obstacleProp = serializedObject.FindProperty("obstacle");
        orderProp = serializedObject.FindProperty("order");
        positionProp = serializedObject.FindProperty("position");
        activationProp = serializedObject.FindProperty("activation");
        spotsProp = serializedObject.FindProperty("spots");
        //checkOrderProp = serializedObject.FindProperty("checkOrder");
        //orderProp = serializedObject.FindProperty("order");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Enigma enigma = target as Enigma;

        EditorGUILayout.LabelField("Enigma Settings", EditorStyles.boldLabel);

        


        EditorGUILayout.PropertyField(rewardProp);
        if (rewardProp.boolValue)
        {
            EditorGUILayout.PropertyField(objProp);
        }

        EditorGUILayout.PropertyField(disableObjectProp);
        if (disableObjectProp.boolValue)
        {
            EditorGUILayout.PropertyField(obstacleProp);
            enigma.targetPosition = EditorGUILayout.Vector3Field("target Position", enigma.targetPosition);
            enigma.animSpeed = EditorGUILayout.IntField("animation Speed", enigma.animSpeed);
        }

        EditorGUILayout.PropertyField(orderProp);
        if (orderProp.boolValue)
        {
            enigma.elementsToOrder = EditorGUILayout.IntField("Elements To Order", enigma.elementsToOrder);
        }

        EditorGUILayout.PropertyField(positionProp);
        if (positionProp.boolValue)
        {
            EditorGUILayout.PropertyField(spotsProp);
        }

        EditorGUILayout.PropertyField(activationProp);
        if(activationProp.boolValue)
        {
            enigma.numbersOfElement = EditorGUILayout.IntField("Numbers Of Activation", enigma.numbersOfElement);
        }

        /*if (combinationProp.boolValue)
        {
            EditorGUILayout.PropertyField(checkOrderProp);

            if (checkOrderProp.boolValue)
            {
                EditorGUILayout.PropertyField(orderProp);
            }
            else
            {
                EditorGUILayout.PropertyField(orderProp);
            }
        }*/

        /*if (GUILayout.Button("Check Winner"))
        {
            // Add your winner checking logic here.
        }*/

        serializedObject.ApplyModifiedProperties();
    }
}