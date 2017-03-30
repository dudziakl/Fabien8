using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AC
{

	#if UNITY_WEBGL
	#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8
	#elif UNITY_5 || UNITY_PRO_LICENSE
	[CustomEditor (typeof (BackgroundImage))]
	public class BackgroundImageEditor : Editor
	{
		
		private BackgroundImage _target;
		
		
		private void OnEnable ()
		{
			_target = (BackgroundImage) target;
		}
		
		
		public override void OnInspectorGUI ()
		{
			EditorGUILayout.BeginVertical ("Button");
			EditorGUILayout.LabelField ("When playing a MovieTexture:");
			_target.loopMovie = EditorGUILayout.Toggle ("Loop clip?", _target.loopMovie);
			_target.restartMovieWhenTurnOn = EditorGUILayout.Toggle ("Restart clip each time?", _target.restartMovieWhenTurnOn);
			EditorGUILayout.EndVertical ();
		
			UnityVersionHandler.CustomSetDirty (_target);
		}

	}
	#endif

}