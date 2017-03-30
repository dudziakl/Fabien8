using UnityEngine;
using System.Collections;
#if UNITY_EDITOR && UNITY_5_4_OR_NEWER
using UnityEditor;
#endif

namespace AC
{

	public static class ACDebug
	{

		private static string hr = "\n\n -> AC debug logger";
		#if UNITY_EDITOR
			#if UNITY_5_4_OR_NEWER
				private static LogType oldLogType;
				private static StackTraceLogType oldStackTraceLogType;
			#elif UNITY_5
				private static StackTraceLogType oldStackTraceLogType;
			#endif
		#endif


		public static void Log (object message, UnityEngine.Object context = null)
		{
			if (CanDisplay (true))
			{
				BackupStackTrace (LogType.Log);
				Debug.Log (message + hr, context);
				RestoreStackTrace ();
			}
		}


		public static void LogWarning (object message, UnityEngine.Object context = null)
		{
			if (CanDisplay ())
			{
				BackupStackTrace (LogType.Warning);
				Debug.LogWarning (message + hr, context);
				RestoreStackTrace ();
			}
		}


		public static void LogError (object message)
		{
			if (CanDisplay ())
			{
				BackupStackTrace (LogType.Error);
				Debug.LogError (message + hr);
				RestoreStackTrace ();
			}
		}


		private static void BackupStackTrace (LogType logType)
		{
			#if UNITY_EDITOR
				#if UNITY_5_4_OR_NEWER
					oldLogType = logType;
					oldStackTraceLogType = PlayerSettings.GetStackTraceLogType (logType);
					PlayerSettings.SetStackTraceLogType (logType, StackTraceLogType.None);
				#elif UNITY_5
					oldStackTraceLogType = Application.stackTraceLogType;
					Application.stackTraceLogType = StackTraceLogType.None;
				#endif
			#endif
		}


		private static void RestoreStackTrace ()
		{
			#if UNITY_EDITOR
				#if UNITY_5_4_OR_NEWER
					PlayerSettings.SetStackTraceLogType (oldLogType, oldStackTraceLogType);
				#elif UNITY_5
					Application.stackTraceLogType = oldStackTraceLogType;
				#endif
			#endif
		}


		private static bool CanDisplay (bool isInfo = false)
		{
			if (KickStarter.settingsManager)
			{
				switch (KickStarter.settingsManager.showDebugLogs)
				{
				case ShowDebugLogs.Always :
					return true;

				case ShowDebugLogs.Never :
					return false;

				case ShowDebugLogs.OnlyWarningsOrErrors :
					if (!isInfo)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			return true;
		}

	}

}