using UnityEngine;
using System.Collections;

namespace DI.SimpleNote
{
	[SimpleNote]
	public class SimpleNoteExample : MonoBehaviour {

		[HelpBox("This is simple Note")]
		public int speed;
		[HelpBox("\nThis is simple Note with Info type\n", MessageType.Info)]
		public float height;
		[HelpBox("\nThis is simple note with Error Type\n", MessageType.Error)]
		public string isThisError;
	}
}
