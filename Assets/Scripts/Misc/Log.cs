using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Misc
{
	public static class Log
	{
		/// <summary>
		///     <para>Logs a message to the Unity Console.</para>
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("UNITY_EDITOR")]
		public static void print(string message)
		{
			Debug.Log(message);
		}

		[Conditional("UNITY_EDITOR")]
		public static void printWarning(string message)
		{
			Debug.LogWarning(message);
		}

		[Conditional("UNITY_EDITOR")]
		public static void printError(string message)
		{
			Debug.LogError(message);
		}
	}
}