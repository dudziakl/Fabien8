﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionDialogOptionRename.cs"
 * 
 *	This action changes the text of dialogue options.
 * 
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionDialogOptionRename : Action
	{
		
		public int optionID;
		public string newLabel;
		public int lineID;

		public int constantID;
		public Conversation linkedConversation;
		
		
		public ActionDialogOptionRename ()
		{
			this.isDisplayed = true;
			category = ActionCategory.Dialogue;
			title = "Rename option";
			description = "Renames the label of a dialogue option.";
		}


		override public void AssignValues ()
		{
			linkedConversation = AssignFile <Conversation> (constantID, linkedConversation);
		}

		
		override public float Run ()
		{
			if (linkedConversation)
			{
				linkedConversation.RenameOption (optionID, newLabel, lineID);
			}
			
			return 0f;
		}
		
		
		#if UNITY_EDITOR
		
		public override void ShowGUI ()
		{
			linkedConversation = (Conversation) EditorGUILayout.ObjectField ("Conversation:", linkedConversation, typeof (Conversation), true);

			if (linkedConversation)
			{
				linkedConversation.Upgrade ();
			}

			constantID = FieldToID <Conversation> (linkedConversation, constantID);
			linkedConversation = IDToField <Conversation> (linkedConversation, constantID, true);

			if (linkedConversation)
			{
				optionID = ShowOptionGUI (linkedConversation.options, optionID);

				newLabel = EditorGUILayout.TextField ("New label text:", newLabel);
			}

			AfterRunningOption ();
		}


		private int ShowOptionGUI (List<ButtonDialog> options, int optionID)
		{
			// Create a string List of the field's names (for the PopUp box)
			List<string> labelList = new List<string>();
			
			int i = 0;
			int tempNumber = -1;

			if (options.Count > 0)
			{
				foreach (ButtonDialog option in options)
				{
					string label = option.ID.ToString () + ": " + option.label;
					if (option.label == "")
					{
						label += "(Untitled option)";
					}
					labelList.Add (label);
					
					if (option.ID == optionID)
					{
						tempNumber = i;
					}
					
					i ++;
				}
				
				if (tempNumber == -1)
				{
					// Wasn't found (variable was deleted?), so revert to zero
					if (optionID != 0)
						ACDebug.LogWarning ("Previously chosen option no longer exists!");
					tempNumber = 0;
					optionID = 0;
				}

				tempNumber = EditorGUILayout.Popup (tempNumber, labelList.ToArray());
				optionID = options [tempNumber].ID;
			}
			else
			{
				EditorGUILayout.HelpBox ("No options exist!", MessageType.Info);
				optionID = 0;
				tempNumber = 0;
			}
			
			return optionID;
		}


		override public void AssignConstantIDs (bool saveScriptsToo)
		{
			if (saveScriptsToo)
			{
				AddSaveScript <RememberConversation> (linkedConversation);
			}
			AssignConstantID <Conversation> (linkedConversation, constantID, 0);
		}
		
		#endif
		
	}

}