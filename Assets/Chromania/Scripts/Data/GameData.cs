using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

[XmlRoot("GameData")]
public class GameData : ScriptableObject  {

	public string Version;

	[XmlArray("Chromiez")]
	[XmlArrayItem("Chromie")]
	public List<ChromieDataObject> ChromiezData;


	public void Save(string path)
	{
		var serializer = new XmlSerializer(typeof(GameData));
        var encoding = Encoding.GetEncoding("UTF-8");

        using (StreamWriter stream = new StreamWriter(path, false, encoding))
        {
            serializer.Serialize(stream, this);
        }
	}
	
	public static GameData Load(string path)
	{
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
		using(FileStream stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as GameData;
		}
	}

	public static GameData LoadResource(string fileName)
	{
		TextAsset textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
		if(textAsset == null)
		{
			Debug.LogError("Could not load text asset " + fileName);
		}
		else
		{
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
			using (MemoryStream stream = new MemoryStream(textAsset.bytes))
			{
				return serializer.Deserialize(stream) as GameData;
			}
		}
		Debug.LogError("Could not load game data!");
		return null;
	}
	
	//Loads the xml directly from the given string. Useful in combination with www.text.
	public static GameData LoadFromText(string text) 
	{
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
		return serializer.Deserialize(new StringReader(text)) as GameData;
	}


	public ChromieDataObject GetChromie(eChromieType colorType)
	{
		foreach (ChromieDataObject chromieDataItem in ChromiezData)
		{
			if (chromieDataItem.ChromieColor == colorType)
			{
				return chromieDataItem;
			}
		}
		return null;
	}
}
