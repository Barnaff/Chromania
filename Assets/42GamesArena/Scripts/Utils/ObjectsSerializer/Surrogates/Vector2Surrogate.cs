using System.Runtime.Serialization;
using UnityEngine;

sealed class Vector2Surrogate : ISerializationSurrogate {
	
	public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context) 
	{
		Vector2 vector2 = (Vector2) obj;
		info.AddValue("x", vector2.x);
		info.AddValue("y", vector2.y);
	}
	
	public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) 
	{
		Vector2 vector2 = (Vector2)obj;
		vector2.x = (float)info.GetValue("x", typeof(float));
		vector2.y = (float)info.GetValue("y", typeof(float));
		obj = vector2;
		return obj; 
	}
}
