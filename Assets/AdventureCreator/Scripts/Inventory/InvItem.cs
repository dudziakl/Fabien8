/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"InvItem.cs"
 * 
 *	This script is a container class for individual inventory items.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace AC
{

	/**
	 * A container class for an Inventory item.
	 * Items are defined in InventoryManager, and downloaded to the RuntimeInventory component during gameplay.
	 */
	[System.Serializable]
	public class InvItem
	{

		/** The item's Editor name */
		public string label;
		/** A unique identifier */
		public int id;
		/** The item's in-game name, if not label */
		public string altLabel;

		/** If True, the Player carries the item when the game begins */
		public bool carryOnStart;
		/** If True, then a Player prefab that is not the default carries the item when the game begins (if playerSwitching = PlayerSwitching.Allow in SettingsManager) */
		public bool carryOnStartNotDefault;
		/** The ID number of the Player prefab that carries the item when the game begins, if carryOnStartNotDefault = True */
		public int carryOnStartID;

		/** The item's properties */
		public List<InvVar> vars = new List<InvVar>();

		/** If True, then multiple instances of the item can be carried at once */
		public bool canCarryMultiple;
		/** The number of instances being carried, if canCarryMultiple = True */
		public int count;
		/** If True, and canCarryMultiple = True, then multiple instances of the same item will be listed in separate MenuInventoryBox slots */
		public bool useSeparateSlots;
		/** The item's main graphic */
		public Texture2D tex;
		/** The item's 'highlighted' graphic */
		public Texture2D activeTex;
		/** The item's 'selected' graphic (if SettingsManager's selectInventoryDisplay = SelectInventoryDisplay.ShowSelectedGraphic) */
		public Texture2D selectedTex;
		/** A GameObject that can be associated with the item, for the creation of e.g. 3D inventory items (through scripting only) */
		public GameObject linkedPrefab;
		/** A CursorIcon instance that, if assigned, will be used in place of the 'tex' Texture when the item is selected on the cursor */
		public CursorIcon cursorIcon = new CursorIcon ();
		/** The translation ID number of the item's name, as generated by SpeechManager */
		public int lineID = -1;
		/** The ID number of the CursorIcon (in CursorManager's cursorIcons List) to show when hovering over the item if appropriate */
		public int useIconID = 0;
		/** The ID number of the item's InvBin category, as defined in InventoryManager */
		public int binID;
		/** If True, the item is being edited within the InventoryManager GUI */
		public bool isEditing = false;
		/** The index number of the MenuCrafting slot that the item is placed in, when used as a Recipe ingredient */
		public int recipeSlot = -1;
		/** An identifier number of the last Use/Inventory interaction associated with the item */
		public int lastInteractionIndex = 0;

		/** If True, then the item has its own "Use X on Y" syntax when selected */
		public bool overrideUseSyntax = false;
		/** The "Use" in "Use X on Y", if overrideUseSyntax = True */
		public HotspotPrefix hotspotPrefix1 = new HotspotPrefix ("Use");
		/** The "on" on "Use X on Y", if overrideUseSyntax = True */
		public HotspotPrefix hotspotPrefix2 = new HotspotPrefix ("on");

		/** The ActionListAsset to run when the item is used, if multiple interactions are disallowed */
		public ActionListAsset useActionList;
		/** The ActionListAsset to run when the item is examined, if multiple interactions are disallowed */
		public ActionListAsset lookActionList;
		/** A List of all "Use" InvInteraction objects associated with the item */
		public List<InvInteraction> interactions = new List<InvInteraction>();
		/** A List of all "Combine" InvInteraction objects associated with the item */
		public List<ActionListAsset> combineActionList = new List<ActionListAsset>();
		/** A List of InvItem ID numbers associated with the InvInteraction objects in combineActionList */
		public List<int> combineID = new List<int>();
		/** The ActionListAsset to run when using the item on a Hotspot is unhandled */
		public ActionListAsset unhandledActionList;
		/** The ActionListAsset to run when using the item on another InvItem is unhandled */
		public ActionListAsset unhandledCombineActionList;

		private bool canBeAnimated;


		/**
		 * <summary>The default Constructor.</summary>
		 * <param name = "idArray">An array of already-used ID numbers, so that a unique ID number can be assigned</param>
		 */
		public InvItem (int[] idArray)
		{
			count = 0;
			tex = null;
			activeTex = null;
			selectedTex = null;
			cursorIcon = new CursorIcon ();
			id = 0;
			binID = -1;
			recipeSlot = -1;
			useSeparateSlots = false;
			carryOnStartNotDefault = false;
			vars = new List<InvVar>();
			canBeAnimated = false;
			linkedPrefab = null;

			interactions = new List<InvInteraction>();

			combineActionList = new List<ActionListAsset>();
			combineID = new List<int>();

			overrideUseSyntax = false;
			hotspotPrefix1 = new HotspotPrefix ("Use");
			hotspotPrefix2 = new HotspotPrefix ("on");

			// Update id based on array
			foreach (int _id in idArray)
			{
				if (id == _id)
					id ++;
			}

			label = "Inventory item " + (id + 1).ToString ();
			altLabel = "";
		}
		

		/**
		 * <summary>A Constructor that sets all its values by copying another InvItem.</summary>
		 * <param name = "assetItem">The InvItem to copy</param>
		 */
		public InvItem (InvItem assetItem)
		{
			count = assetItem.count;
			tex = assetItem.tex;
			activeTex = assetItem.activeTex;
			selectedTex = assetItem.selectedTex;
			cursorIcon = assetItem.cursorIcon;
			carryOnStart = assetItem.carryOnStart;
			carryOnStartNotDefault = assetItem.carryOnStartNotDefault;
			carryOnStartID = assetItem.carryOnStartID;
			canCarryMultiple = assetItem.canCarryMultiple;
			label = assetItem.label;
			altLabel = assetItem.altLabel;
			id = assetItem.id;
			lineID = assetItem.lineID;
			useIconID = assetItem.useIconID;
			binID = assetItem.binID;
			useSeparateSlots = assetItem.useSeparateSlots;
			isEditing = false;
			recipeSlot = -1;

			overrideUseSyntax = assetItem.overrideUseSyntax;
			hotspotPrefix1 = assetItem.hotspotPrefix1;
			hotspotPrefix2 = assetItem.hotspotPrefix2;
			
			useActionList = assetItem.useActionList;
			lookActionList = assetItem.lookActionList;
			interactions = assetItem.interactions;
			combineActionList = assetItem.combineActionList;
			unhandledActionList = assetItem.unhandledActionList;
			unhandledCombineActionList = assetItem.unhandledCombineActionList;
			combineID = assetItem.combineID;
			vars = assetItem.vars;
			linkedPrefab = assetItem.linkedPrefab;

			canBeAnimated = DetermineCanBeAnimated ();
		}


		/**
		 * <summary>Checks if the item has an InvInteraction combine interaction for a specific InvItem.</summary>
		 * <param name = "invItem">The InvITem to check for</param>
		 * <returns>True if the item has an InvInteraction combine interaction for the InvItem.</returns>
		 */
		public bool DoesHaveInventoryInteraction (InvItem invItem)
		{
			if (invItem != null)
			{
				foreach (int invID in combineID)
				{
					if (invID == invItem.id)
					{
						return true;
					}
				}
			}
			
			return false;
		}


		/**
		 * <summary>Gets the item's display name.</summary>
		 * <param name = "languageNumber">The index of the current language, as set in SpeechManager</param>
		 * <returns>The item's display name</returns>
		 */
		public string GetLabel (int languageNumber)
		{
			if (languageNumber > 0)
			{
				return (KickStarter.runtimeLanguages.GetTranslation (label, lineID, languageNumber));
			}
			else
			{
				if (altLabel != "")
				{
					return altLabel;
				}
				return label;
			}
		}


		public string GetFullLabel (int languageNumber = 0)
		{
			string label = "";

			if (KickStarter.stateHandler.gameState == GameState.DialogOptions && !KickStarter.settingsManager.allowInventoryInteractionsDuringConversations)
			{
				return "";
			}

			if (KickStarter.runtimeInventory.showHoverLabel)
			{
				label = KickStarter.playerInteraction.GetLabelPrefix (null, this, languageNumber);

				if (KickStarter.runtimeInventory.selectedItem == null || this != KickStarter.runtimeInventory.selectedItem)
				{
					label += GetLabel (languageNumber);
				}
				else
				{
					label = GetLabel (languageNumber);
				}
			}

			return (label);	
		}


		/**
		 * <summary>Gets the index number of the next relevant use/combine interaction.</summary>
		 * <param name = "i">The index number to start from</param>
		 * <param name = "numInvInteractions">The number of relevant "combine" interactions</param>
		 */
		public int GetNextInteraction (int i, int numInvInteractions)
		{
			if (i < interactions.Count)
			{
				i ++;

				if (i >= interactions.Count + numInvInteractions)
				{
					return 0;
				}
				else
				{
					return i;
				}
			}
			else if (i == interactions.Count - 1 + numInvInteractions)
			{
				return 0;
			}
			
			return (i+1);
		}
		

		/**
		 * <summary>Gets the index number of the previous relevant use/combine interaction.</summary>
		 * <param name = "i">The index number to start from</param>
		 * <param name = "numInvInteractions">The number of relevant "combine" interactions</param>
		 */
		public int GetPreviousInteraction (int i, int numInvInteractions)
		{
			if (i > interactions.Count && numInvInteractions > 0)
			{
				return (i-1);
			}
			else if (i == 0)
			{
				return GetNumInteractions (numInvInteractions) - 1;
			}
			else if (i <= interactions.Count)
			{
				i --;

				if (i < 0)
				{
					return GetNumInteractions (numInvInteractions) - 1;
				}
				else
				{
					return i;
				}
			}
			
			return (i-1);
		}


		private int GetNumInteractions (int numInvInteractions)
		{
			return (interactions.Count + numInvInteractions);
		}


		/**
		 * <summary>Checks if the item's assigned textures are enough for animated effects to be possible.</summary>
		 * <returns>True if the item's assigned textures are enough for animated effects to be possible.</returns>
		 */
		public bool CanBeAnimated ()
		{
			return canBeAnimated;
		}


		private bool DetermineCanBeAnimated ()
		{
			if (cursorIcon != null && cursorIcon.texture != null && cursorIcon.isAnimated)
			{
				return true;
			}
			if (activeTex != null)
			{
				return true;
			}
			return false;
		}


		/**
		 * <summary>Gets a property of the inventory item.</summary>
		 * <param name = "ID">The ID number of the property to get</param>
		 * <returns>The property of the inventory item</returns>
		 */
		public InvVar GetProperty (int ID)
		{
			if (vars.Count > 0 && ID >= 0)
			{
				foreach (InvVar var in vars)
				{
					if (var.id == ID)
					{
						return var;
					}
				}
			}
			return null;
		}

	}

}