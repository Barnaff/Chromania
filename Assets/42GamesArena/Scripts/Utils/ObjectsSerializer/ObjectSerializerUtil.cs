using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

public class ObjectSerializerUtil  {

	public static string Serialize(object obj, BinaryFormatter binaryFormatter = null)
	{
		if (binaryFormatter == null)
		{
			binaryFormatter = CreateBinaryFormatter();
		}

		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, obj);
		byte[] listBytes = memoryStream.ToArray();
		return System.Convert.ToBase64String(listBytes); 
	}
	
	public static object Deserialize(string stringData, BinaryFormatter binaryFormatter = null)
	{
		byte[] listByts = System.Convert.FromBase64String(stringData);
		if (binaryFormatter == null)
		{
			binaryFormatter = CreateBinaryFormatter();
		}
		MemoryStream memoryStream = new MemoryStream(listByts);
		return binaryFormatter.Deserialize(memoryStream);
	}

	private static BinaryFormatter CreateBinaryFormatter()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		
		SurrogateSelector surrogateSelector = new SurrogateSelector();
		
		ColorSurrogate colorSurrogate = new ColorSurrogate();
		surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All) , colorSurrogate);
		
		SpriteSurrogate spriteSurrogate = new SpriteSurrogate();
		surrogateSelector.AddSurrogate(typeof(Sprite), new StreamingContext(StreamingContextStates.All) , spriteSurrogate);
		
		RectSurrogate rectSurrogate = new RectSurrogate();
		surrogateSelector.AddSurrogate(typeof(Rect), new StreamingContext(StreamingContextStates.All) , rectSurrogate);
		
		Vector2Surrogate vector2Surrogate = new Vector2Surrogate();
		surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All) , vector2Surrogate);
		
		binaryFormatter.SurrogateSelector = surrogateSelector;

		return binaryFormatter;
	}
}
