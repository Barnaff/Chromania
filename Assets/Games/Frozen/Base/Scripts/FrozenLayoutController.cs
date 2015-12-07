using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine.UI;

public delegate void FrozenAddBlockScoreDelegate(int scoreToAdd);

public class FrozenLayoutController : MonoBehaviour {

	#region Public Properties

	/// <summary>
	/// The blocks container.
	/// </summary>
	public GameObject BlocksContainer;

	/// <summary>
	/// The game blocks.
	/// </summary>
	public List<LayoutPair> GameBlocks;

	/// <summary>
	/// The name of the layout data XML file.
	/// </summary>
	public string LayoutDataXMLFileName;

	/// <summary>
	/// The on add score.
	/// </summary>
	public FrozenAddBlockScoreDelegate OnAddScore;

	public Text LayoutIdLabel;

	#endregion


	#region Private Properties

	private Vector2 _screenDimensions;

	private List<LevelLayout> _layoutsList;

	private List<LevelLayout> _usedLayouts = new List<LevelLayout>();

	private int _level;

	#endregion


	#region Initialize

	// Use this for initialization
	void Start () {
	
		_screenDimensions = CalculateScreenSizeInWorldCoords();

		LoadLayouts();

		GetNewLayout();

	}

	#endregion


	#region Public

	public void GetNewLayout(int level = 0, Action completionAction = null)
	{
		_level = level;
		StartCoroutine(DestoryLastBlocks(()=>
		                                 {
			LevelLayout layout = GetRandomLayout();
		//	LayoutIdLabel.text = "Layout: " + layout.ID.ToString();

			BuildLayout(layout);

			if (completionAction != null)
			{
				completionAction();
			}
		}));

		

	}

	public void BuildLayout(LevelLayout levelLayout)
	{
		BlocksContainer = new GameObject();
		BlocksContainer.name = "Blocks Container";

		string layoutString = levelLayout.Layout;
		string[] blocksArray = layoutString.Split(","[0]);
		int x = -1;
		int y = 0;

		foreach(string blockString in blocksArray)
		{
			if (x >= levelLayout.BaseWidth -1)
			{
				y++;
				x = -1;
			}
			x++;
			GameObject blockPrefab = GetBlockForId(int.Parse(blockString));
			if (blockPrefab != null)
			{
				GameObject block = Instantiate(blockPrefab) as GameObject;
				block.transform.SetParent(BlocksContainer.transform);
				float blockWidth = _screenDimensions.x / levelLayout.BaseWidth;
				float originalWidth = block.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
				float sizeScale = blockWidth / originalWidth;
				block.transform.localScale = new Vector3(sizeScale,sizeScale,sizeScale);
				float blockHeight = block.GetComponent<SpriteRenderer>().sprite.bounds.size.y * sizeScale;
				block.transform.localPosition = new Vector3(x * blockWidth + (blockWidth * 0.5f), (-y - 1) * blockHeight + (blockHeight * 0.5f), 0);
				block.GetComponent<BlockController>().OnBlockAddScore += OnBlockAddScore;
			}
		}

	
		BlocksContainer.transform.position = new Vector3(-_screenDimensions.x * 0.5f, _screenDimensions.y * 0.5f, 0);

		iTween.MoveFrom(BlocksContainer, iTween.Hash("time", 0.5f, "y", BlocksContainer.transform.position.y + _screenDimensions.y * 0.5f));
	}

	public void ResetBlocksContainer()
	{
		Vector3 position  = new Vector3(-_screenDimensions.x * 0.5f, _screenDimensions.y * 0.5f, 0);

		iTween.MoveTo(BlocksContainer, iTween.Hash("time", 0.8f, "y", position.y));
	}

	public void MoveDown()
	{
		if (BlocksContainer != null)
		{
			Vector3 position = BlocksContainer.transform.position;
			position.y -= 10.0f;
			BlocksContainer.transform.position = position;
		}
	}

	public GameObject GetLowesBlock()
	{
		GameObject lowesBlock = null;
		if (BlocksContainer != null)
		{
			for (int i=0; i< BlocksContainer.transform.childCount; i++)
			{
				Transform blockTransform = BlocksContainer.transform.GetChild(i);
				if (blockTransform != null)
				{
					if (lowesBlock == null)
					{
						lowesBlock = blockTransform.gameObject;
					}
					else
					{
						if (lowesBlock.transform.position.y > blockTransform.position.y)
						{
							lowesBlock = blockTransform.gameObject;
						}
					}
				}
			}
		}
		return lowesBlock;
	}
			
	#endregion


	#region Private

	private void OnBlockAddScore(int score)
	{
		if (OnAddScore != null)
		{
			OnAddScore(score);
		}
	}

	private Vector2 CalculateScreenSizeInWorldCoords ()  {
		Camera cam = Camera.main;
		var p1 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));  
		var p2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
		var p3 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
		
		float width  = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;
		
		Vector2 dimensions = new Vector2(width,height);
		
		return dimensions;
	}

	private GameObject GetBlockForId(int blockId)
	{
		foreach (LayoutPair pair in GameBlocks)
		{
			if (pair.ID == blockId)
			{
				return pair.Block;
			}
		}
		return null;
	}

	private void LoadLayouts()
	{
		TextAsset textAsset = (TextAsset) Resources.Load(LayoutDataXMLFileName);  

		_layoutsList = new List<LevelLayout>();
		
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( textAsset.text );
		
		XmlNode layouts = xmldoc["Layouts"];
		
		foreach (XmlNode layoutNode in layouts.ChildNodes)
		{
			LevelLayout layout = new LevelLayout();
			
			layout.ID = int.Parse(layoutNode.Attributes.GetNamedItem("id").Value);
			layout.MinLevel = int.Parse(layoutNode.Attributes.GetNamedItem("minLevel").Value);
			layout.BaseWidth = int.Parse(layoutNode.Attributes.GetNamedItem("baseWidth").Value);
			layout.Layout = layoutNode.InnerText;

			_layoutsList.Add(layout);
		}
	}

	private LevelLayout GetRandomLayout()
	{
		List<LevelLayout> validLayouts = new List<LevelLayout>();

		foreach (LevelLayout layout in _layoutsList)
		{
			if (layout.MinLevel <= _level)
			{
				validLayouts.Add(layout);
			}
		}

		if (validLayouts.Count <= 0)
		{
			_layoutsList.AddRange(_usedLayouts);
			return GetRandomLayout();
		}

		LevelLayout selectedLayout = validLayouts[UnityEngine.Random.Range(0,validLayouts.Count)];
		_usedLayouts.Add(selectedLayout);
		_layoutsList.Remove(selectedLayout);
		return selectedLayout;
	}

	private IEnumerator DestoryLastBlocks(Action completionAction)
	{
		if (BlocksContainer != null)
		{
			List<GameObject> blocksToKill = new List<GameObject>();

			for (int i=0; i< BlocksContainer.transform.childCount; i++)
			{
				Transform blockTransform = BlocksContainer.transform.GetChild(i);
				blocksToKill.Add(blockTransform.gameObject);
//				if (blockTransform != null)
//				{
//					BlockController blockController = blockTransform.gameObject.GetComponent<BlockController>() as BlockController;
//					if (blockController != null)
//					{
//						yield return new WaitForEndOfFrame();
//						OnBlockAddScore(3);
//						blockController.KillBlock();
//					}
//				}
			}

			foreach (GameObject blockToKill in blocksToKill)
			{
				if (blockToKill != null)
				{
					BlockController blockController = blockToKill.GetComponent<BlockController>() as BlockController;
					if (blockController != null)
					{
						yield return new WaitForEndOfFrame();
						OnBlockAddScore(3);
						blockController.KillBlock();
					}
				}
			}

			yield return new WaitForSeconds(Time.fixedDeltaTime * BlocksContainer.transform.childCount);
			
			Destroy(BlocksContainer);
			BlocksContainer = null;
			
			yield return new WaitForSeconds(0.1f);
		}
		completionAction();
	}

	#endregion
}

[Serializable]
public class LayoutPair 
{
	public int ID;
	public GameObject Block;
}

public class LevelLayout
{
	public int ID;
	public int MinLevel;
	public string Layout;
	public int BaseWidth;
}
