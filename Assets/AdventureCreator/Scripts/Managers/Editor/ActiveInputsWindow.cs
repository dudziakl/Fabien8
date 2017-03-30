using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace AC
{
	
	public class ActiveInputsWindow : EditorWindow
	{
		
		private SettingsManager settingsManager;
		
		[MenuItem ("Adventure Creator/Editors/Active Inputs Editor", false, 0)]
		public static void Init ()
		{
			ActiveInputsWindow window = (ActiveInputsWindow) EditorWindow.GetWindow (typeof (ActiveInputsWindow));
			UnityVersionHandler.SetWindowTitle (window, "Active Inputs");
			window.position = new Rect (300, 200, 450, 400);
		}
		
		
		private void OnEnable ()
		{
			if (AdvGame.GetReferences () && AdvGame.GetReferences ().settingsManager)
			{
				settingsManager = AdvGame.GetReferences ().settingsManager;
			}
		}
		
		
		private void OnGUI ()
		{
			if (settingsManager == null)
			{
				EditorGUILayout.HelpBox ("A Settings Manager must be assigned before this window can display correctly.", MessageType.Warning);
				return;
			}

			settingsManager.activeInputs = ShowActiveInputsGUI (settingsManager.activeInputs);

			UnityVersionHandler.CustomSetDirty (settingsManager);
		}
		
		
		private List<ActiveInput> ShowActiveInputsGUI (List<ActiveInput> activeInputs)
		{
			EditorGUILayout.HelpBox ("Active Inputs are used to trigger ActionList assets when an input key is pressed under certain gameplay conditions.", MessageType.Info);
			
			for (int i=0; i<activeInputs.Count; i++)
			{
				EditorGUILayout.BeginVertical (CustomStyles.thinBox);

				string defaultName = "ActiveInput_" + activeInputs[i].inputName;
				if (activeInputs[i].inputName == "") defaultName = "ActiveInput_" + i.ToString ();

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Input #" + i.ToString (), EditorStyles.boldLabel);
				if (GUILayout.Button ("-", GUILayout.Width (20f)))
				{
					activeInputs.RemoveAt (i);
					return activeInputs;
				}
				EditorGUILayout.EndHorizontal ();
				activeInputs[i].inputName = EditorGUILayout.TextField ("Input button:", activeInputs[i].inputName);
				activeInputs[i].gameState = (GameState) EditorGUILayout.EnumPopup ("Available when game is:", activeInputs[i].gameState);
				activeInputs[i].actionListAsset = ActionListAssetMenu.AssetGUI ("ActionList when triggered:", activeInputs[i].actionListAsset, "", defaultName);

				EditorGUILayout.EndVertical ();
			}

			if (activeInputs.Count > 0)
			{
				EditorGUILayout.Space ();
			}

			if (GUILayout.Button ("Create new Active Input"))
			{
				activeInputs.Add (new ActiveInput ());
			}
			
			return activeInputs;
		}
		
		
	}
	
}
