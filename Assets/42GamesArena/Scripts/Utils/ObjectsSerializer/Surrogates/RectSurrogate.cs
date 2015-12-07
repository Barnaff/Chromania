using System.Runtime.Serialization;
using UnityEngine;

sealed class RectSurrogate : ISerializationSurrogate {
	
	public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context) {
		
		Rect rect = (Rect) obj;

		info.AddValue("center", rect.center);
		info.AddValue("h", rect.height);
		info.AddValue("w", rect.width);
		info.AddValue("position", rect.position);

	}
	
	public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) 
	{
		Rect rect = (Rect) obj;

		rect.center = (Vector2)info.GetValue("center", typeof(Vector2));
		rect.position = (Vector2)info.GetValue("position", typeof(Vector2));
		rect.width = (float)info.GetValue("w", typeof(float));
		rect.height = (float)info.GetValue("h", typeof(float));

		obj = rect;
		return obj; 
	}
}
