using UnityEngine;
using System.Collections;
using UnityEditor;

//Custom inspector its not really that important


[CustomEditor(typeof(DayNightController))]
public class DayNightControllerEditor : Editor {

	private SerializedProperty m_Property;
	private SerializedProperty m_Property_stars;
	private SerializedObject m_Object;


	void OnEnable() {
		m_Object = new SerializedObject(target);
	}


	public override void OnInspectorGUI(){

		DayNightController myTarget = (DayNightController)target;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Day Night Speed Multiplier : ");
		myTarget.daySpeedMultiplier = EditorGUILayout.FloatField (myTarget.daySpeedMultiplier);
		EditorGUILayout.EndHorizontal ();


		if (!myTarget.sunLight) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.HelpBox ("Please assign this variable",MessageType.Warning);
			EditorGUILayout.EndHorizontal ();
		}


		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Sun Light : ");
		myTarget.sunLight = (Light)EditorGUILayout.ObjectField (myTarget.sunLight, typeof(Light), true);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Control Intensity : ");
		myTarget.controlIntensity = EditorGUILayout.Toggle (myTarget.controlIntensity);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.HelpBox ("What time the cycle should start ?",MessageType.Info);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.LabelField ("Start Time : ");
		myTarget.startTime = EditorGUILayout.Slider(myTarget.startTime,0.0f,24.0f);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Current Time : " + myTarget.timeString);
		EditorGUILayout.EndHorizontal ();

	
		EditorGUILayout.BeginHorizontal ();
		m_Property = m_Object.FindProperty("cloudSpheres");
		EditorGUILayout.PropertyField(m_Property, new GUIContent("Clouds Sphere"), true);
		m_Object.ApplyModifiedProperties();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Cloud Rotation Speed : ");
		myTarget.cloudRotationSpeed = EditorGUILayout.FloatField (myTarget.cloudRotationSpeed);
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal ();
		m_Property_stars = m_Object.FindProperty("starSpheres");
		EditorGUILayout.PropertyField(m_Property_stars, new GUIContent("Stars Sphere"), true);
		m_Object.ApplyModifiedProperties();
		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Star Rotation Speed : ");
		myTarget.starRotationSpeed = EditorGUILayout.FloatField (myTarget.starRotationSpeed);
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Stars Twinkle Frequency : ");
		myTarget.twinkleFrequency = EditorGUILayout.FloatField (myTarget.twinkleFrequency);
		EditorGUILayout.EndHorizontal();

		if (!myTarget.cameraToFollow) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.HelpBox ("If not assigned then the main camera will be used",MessageType.Warning);
			EditorGUILayout.EndHorizontal ();
		}


		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Camera to follow : ");
		myTarget.cameraToFollow = (Camera)EditorGUILayout.ObjectField (myTarget.cameraToFollow, typeof(Camera), true);
		EditorGUILayout.EndHorizontal ();


		if(GUI.changed)
		{
			EditorUtility.SetDirty( target );
		}

	}


}
