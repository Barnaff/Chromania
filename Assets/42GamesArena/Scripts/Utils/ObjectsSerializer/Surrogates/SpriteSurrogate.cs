using System.Runtime.Serialization;
using UnityEngine;

sealed class SpriteSurrogate : ISerializationSurrogate {
	
	public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context) {
		
		Sprite sprite = (Sprite) obj;
		info.AddValue("texture", sprite.texture.GetRawTextureData());
		info.AddValue("name", sprite.name);
		info.AddValue("w", sprite.rect.width);
		info.AddValue("h", sprite.rect.height);
		info.AddValue("tw", sprite.texture.width);
		info.AddValue("th", sprite.texture.height);
		info.AddValue("rect", sprite.rect);
		info.AddValue("px", sprite.pivot.x);
		info.AddValue("py", sprite.pivot.y);
	}
	
	public System.Object SetObjectData(System.Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
		

		byte[] textureByte = (byte[])info.GetValue("texture", typeof(byte[]));
		int textureWidth = (int)info.GetValue("tw", typeof(int));
		int textureHeight = (int)info.GetValue("th", typeof(int));

		Texture2D texture = new Texture2D(textureWidth, textureHeight);
		texture.LoadRawTextureData(textureByte);
		Rect rect = (Rect)info.GetValue("rect", typeof(Rect));
		Vector2 pivot = new Vector2((float)info.GetValue("px", typeof(float)), (float)info.GetValue("py", typeof(float)));

		Sprite sprite = Sprite.Create(texture, rect, pivot);

		sprite.name = (string)info.GetValue("name", typeof(string));

		obj = sprite;
		return obj; 
	}
}
