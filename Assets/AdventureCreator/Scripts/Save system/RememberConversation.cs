/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"RememberConversation.cs"
 * 
 *	This script is attached to conversation objects in the scene
 *	with DialogOption states we wish to save.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AC
{

	/**
	 * Attach this script to Conversation objects in the scene with DialogOption states you wish to save.
	 */
	[AddComponentMenu("Adventure Creator/Save system/Remember Conversation")]
	#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0)
	[HelpURL("http://www.adventurecreator.org/scripting-guide/class_a_c_1_1_remember_conversation.html")]
	#endif
	public class RememberConversation : Remember
	{

		/**
		 * <summary>Serialises appropriate GameObject values into a string.</summary>
		 * <returns>The data, serialised as a string</returns>
		 */
		public override string SaveData ()
		{
			ConversationData conversationData = new ConversationData();
			conversationData.objectID = constantID;

			if (GetComponent <Conversation>())
			{
				Conversation conversation = GetComponent <Conversation>();

				List<bool> optionStates = new List<bool>();
				List<bool> optionLocks = new List<bool>();
				List<bool> optionChosens = new List<bool>();
				List<string> optionLabels = new List<string>();
				List<int> optionLineIDs = new List<int>();

				foreach (ButtonDialog _option in conversation.options)
				{
					optionStates.Add (_option.isOn);
					optionLocks.Add (_option.isLocked);
					optionChosens.Add (_option.hasBeenChosen);
					optionLabels.Add (_option.label);
					optionLineIDs.Add (_option.lineID);
				}

				conversationData._optionStates = ArrayToString <bool> (optionStates.ToArray ());
				conversationData._optionLocks = ArrayToString <bool> (optionLocks.ToArray ());
				conversationData._optionChosens = ArrayToString <bool> (optionChosens.ToArray ());
				conversationData._optionLabels = ArrayToString <string> (optionLabels.ToArray ());
				conversationData._optionLineIDs = ArrayToString <int> (optionLineIDs.ToArray ());

				conversationData.lastOption = conversation.lastOption;
			}

			return Serializer.SaveScriptData <ConversationData> (conversationData);
		}


		/**
		 * <summary>Deserialises a string of data, and restores the GameObject to its previous state.</summary>
		 * <param name = "stringData">The data, serialised as a string</param>
		 */
		public override void LoadData (string stringData)
		{
			ConversationData data = Serializer.LoadScriptData <ConversationData> (stringData);
			if (data == null) return;

			if (GetComponent <Conversation>())
			{
				Conversation conversation = GetComponent <Conversation>();

				bool[] optionStates = StringToBoolArray (data._optionStates);
				bool[] optionLocks = StringToBoolArray (data._optionLocks);
				bool[] optionChosens = StringToBoolArray (data._optionChosens);
				string[] optionLabels = StringToStringArray (data._optionLabels);
				int[] optionLineIDs = StringToIntArray (data._optionLineIDs);

				for (int i=0; i<conversation.options.Count; i++)
				{
					if (optionStates != null && optionStates.Length > i)
					{
						conversation.options[i].isOn = optionStates[i];
					}

					if (optionLocks != null && optionLocks.Length > i)
					{
						conversation.options[i].isLocked = optionLocks[i];
					}

					if (optionChosens != null && optionChosens.Length > i)
					{
						conversation.options[i].hasBeenChosen = optionChosens[i];
					}

					if (optionLabels != null && optionLabels.Length > i)
					{
						conversation.options[i].label = optionLabels[i];
					}

					if (optionLineIDs != null && optionLineIDs.Length > i)
					{
						conversation.options[i].lineID = optionLineIDs[i];
					}
				}

				conversation.lastOption = data.lastOption;
			}
		}

	}


	/**
	 * A data container used by the RememberConversation script.
	 */
	[System.Serializable]
	public class ConversationData : RememberData
	{

		/** The enabled state of each DialogOption */
		public string _optionStates;
		/** The locked state of each DialogOption */
		public string _optionLocks;
		/** The 'already chosen' state of each DialogOption */
		public string _optionChosens;
		/** The index of the last-chosen option */
		public int lastOption;
		/** The labels of each DialogOption */
		public string _optionLabels;
		/** The line IDs of each DialogOption */
		public string _optionLineIDs;

		/**
		 * The default Constructor.
		 */
		public ConversationData () { }
	}

}