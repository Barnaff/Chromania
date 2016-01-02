using UnityEngine;

/* See the "http://avoex.com/?p=30" for the full license governing this code. */

namespace AvoEx
{
    public static class UtilEvent
    {
        public static bool IsMouseOn(Rect rect)
        {
            return rect.Contains(Event.current.mousePosition);
        }

        public static bool IsClicked(Rect rect)
        {
            return Event.current.type == EventType.MouseDown && IsMouseOn(rect);
        }

		public static bool IsEnterPressed()
		{
			if (Event.current.isKey && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
				return true;
			return false;
		}

		public static bool IsInputUnsignedNumber()
		{
			if (Event.current.isKey)
			{
				char inputChar = Event.current.character;
				if (inputChar < '0' || inputChar > '9')
				{
					return false;
				}
			}
			return true;
		}
    }
}