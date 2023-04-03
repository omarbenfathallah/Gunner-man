using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;


namespace TerrainStratumPainter
{
	/// <summary>
	/// TerrainStratumPainter v1.0
	/// </summary>
	public class TerrainStratumPainter : EditorWindow
	{
		// Add to the menu.
		[MenuItem ("Tools/Terrain Stratum Painter")]
		public static void  ShowWindow()
		{
			var t = GetWindowWithRect<TerrainStratumPainter>(new Rect(0,0,320,400),true,"Terrain Stratum Painter");

			// Find terrain.
			t.SetActiveTerrain();
			// Set maximumheight.
			t.SetMaximumHeight();
		}



		private struct StratumInfoTable
		{
			public SplatPrototype splat;
			public float value;

			public StratumInfoTable(SplatPrototype _splat, float _value)
			{
				splat = _splat;
				value = _value;
			}
		}



		#region variables
		private Terrain target;
		private Vector2 scroll;

		private Dictionary<int,StratumInfoTable> table = new Dictionary<int,StratumInfoTable>();

		private Rect scrollRect = new Rect(0,20,165,360);

		private Rect viewRect;
		private Rect baseRect = new Rect(17,0,128,80);

		private Rect buttonRect = new Rect(20,380,120,20);

		private Rect texRect = new Rect(0,15,64,64);
		private Rect heightRect = new Rect(66,15,60,64);

		private float maxHeight;


		// from ver 1.1
		private Rect AreaLabelRect = new Rect(180,20,120,20);

		private Rect minmaxhRect = new Rect(180,160,120,10);
		private Rect minmaxvRect = new Rect(180,40,120,10);

		private Rect boxRect = new Rect(180,40,120,120);
		private Rect boxdrawRect = new Rect(180,40,120,120);
		private Color boxdrawColor = new Color(0f,1f,0f,0.3f);

		private Vector2 rotatePivot = new Vector2(180,40);

		private float[] minmaxData = new float[]{0f,1f,0f,1f};

		private Rect undoRect = new Rect(150,380,120,20);
		private float[,,] alphaBuffer = new float[,,]{};
		#endregion



		#region functions
		/// <summary>set active terrain.</summary>
		private void SetActiveTerrain()
		{
			if(Terrain.activeTerrain==null)
			{
				target = FindObjectOfType<Terrain>();
				if(target==null)
					return;
				if(target.terrainData==null)
					target = null;
			}
			else
			{
				target = Terrain.activeTerrain;
				if(target==null)
					return;
				if(target.terrainData==null)
					target = null;
			}
		}

		/// <summary>set maximum height</summary>
		private void SetMaximumHeight()
		{
			var td = target.terrainData;
			var heights = td.GetHeights(0,0,td.heightmapResolution,td.heightmapResolution);
			maxHeight = float.MinValue;
			foreach (var h in heights)
			{
				if(h>maxHeight)
					maxHeight = h;
			}
			maxHeight = td.heightmapResolution*maxHeight*(td.size.y/td.heightmapResolution);
		}

		/// <summary>Painting process.</summary>
		private void DoPaint()
		{
			var td = target.terrainData;
			var ar = target.terrainData.alphamapResolution;
			var hr = target.terrainData.heightmapResolution;
			var alpha = td.GetAlphamaps(0,0,ar,ar);
			var height = td.GetHeights(0,0,hr,hr);

			var table2 = table.OrderByDescending(xx=>xx.Value.value);

			var view = viewRect.height-80;

			// set undo buffer.
			alphaBuffer = (float[,,])alpha.Clone();

			// paint all alphamaps.
			for (int y=0;y<ar;y++)
			{
				// target area check.
				var ny = (float)(ar-y)/ar;
				if(ny < minmaxData[2] || ny > minmaxData[3])
					continue;

				for (int x=0;x<ar;x++)
				{
					// target area check.
					var nx = (float)x/ar;
					if(nx < minmaxData[0] || nx > minmaxData[1])
						continue;

					var hx = Mathf.RoundToInt((float)x/ar*hr);
					var hy = Mathf.RoundToInt((float)y/ar*hr);
					var h = height[hy,hx];
					var oldH = -1f;
					bool hasChanged = false;

					foreach (var t2 in table2)
					{
						var newH = (maxHeight-t2.Value.value/view*maxHeight)/hr;
						if(h>oldH && h<=newH)
						{
							hasChanged = true;
							alpha[y,x,t2.Key] = 1f;
						}
						else
							alpha[y,x,t2.Key] = 0f;
						oldH = newH;
					}
					if(!hasChanged)
						alpha[y,x,table2.Last().Key] = 1f;
				}
			}

			td.SetAlphamaps(0,0,alpha);

			EditorUtility.DisplayDialog("Terrain Stratum Painter","Painting has been finished.","ok");
		}
		#endregion



		#region GUI
		/// <summary>show window.</summary>
		private void OnGUI()
		{
			EditorGUIUtility.labelWidth = 80;
			target = EditorGUILayout.ObjectField("Source : ",target,typeof(Terrain),true) as Terrain;

			// check terrain.
			if(target==null)
				return;
			var t = target;
			if(t.terrainData == null)
			{
				EditorGUI.LabelField(scrollRect,"not found terrain data.");
				return;
			}

			// get splat types.
			var splat = t.terrainData.splatPrototypes;

			// no textures.
			if(splat.Length==0)
			{
				GUILayout.Label("no textures found.");
				return;
			}

			// set table.
			if(splat.Length!=table.Count)
			{
				table.Clear();
				float max = (splat.Length-1)*80;
				for(var i=splat.Length-1;i>=0;i--)
					table.Add(i,new StratumInfoTable(splat[i],max-i*80));
			}

			// set view rect.
			viewRect = new Rect(0,0,145,table.Count*80);

			// begin scroll area.
			scroll = GUI.BeginScrollView(scrollRect,scroll,viewRect,false,true);
			{
				// begin sub-window area.
				BeginWindows();
				{
					// show window, each of splat types.
					for (int i=0;i<splat.Length;i++)
					{
						var ta = table[i];
						var view = viewRect.height-80;
						var r = baseRect;
						r.y = ta.value;

						r = GUI.Window(i,r,(x)=>{

							// show texture.
							GUI.DrawTexture(texRect,splat[x].texture,ScaleMode.StretchToFill,false);

							// show height.
							GUI.Label(heightRect,"Height\n"+(maxHeight-r.y/view*maxHeight).ToString("F2"));

							// set window draggable.
							GUI.DragWindow(new Rect(0,0,128,80));
						},splat[i].texture.name);



						ta.value = Mathf.Clamp(r.y,0,view);
						table[i] = ta;
					}
				}
				EndWindows();

				// show texture bar.
				var splat2 = table.OrderByDescending(x=>x.Value.value);
				var lastSplat = splat2.Last();
				var br = viewRect;
				br.width = 15;
				br.y = br.height;
				foreach (var s in splat2)
				{
					if(lastSplat.Key!=s.Key)
					{
						br.height = br.y-s.Value.value;
						br.y = s.Value.value;
					}
					else
					{
						br.height = br.y;
						br.y = 0;
					}

					GUI.DrawTexture(br,s.Value.splat.texture,ScaleMode.StretchToFill,false);
				}
			}
			GUI.EndScrollView();

			if(GUI.Button(buttonRect,"do painting"))
				DoPaint();


			// from ver 1.1
			if(alphaBuffer.Length>0)
			{
				if(GUI.Button(undoRect,"Undo"))
				{
					t.terrainData.SetAlphamaps(0,0,alphaBuffer);
					alphaBuffer = new float[,,]{};
				}
			}

			GUI.Label(AreaLabelRect,"Target Area");

			GUI.Box(boxRect,"");

			boxdrawRect.xMin = Mathf.Lerp(boxRect.xMin,boxRect.xMax,minmaxData[0]);
			boxdrawRect.xMax = Mathf.Lerp(boxRect.xMin,boxRect.xMax,minmaxData[1]);
			boxdrawRect.yMin = Mathf.Lerp(boxRect.yMin,boxRect.yMax,minmaxData[2]);
			boxdrawRect.yMax = Mathf.Lerp(boxRect.yMin,boxRect.yMax,minmaxData[3]);
			EditorGUI.DrawRect(boxdrawRect,boxdrawColor);

			EditorGUI.MinMaxSlider(minmaxhRect,ref minmaxData[0],ref minmaxData[1],0f,1f);
			GUIUtility.RotateAroundPivot(90f, rotatePivot);
			EditorGUI.MinMaxSlider(minmaxvRect,ref minmaxData[2],ref minmaxData[3],0f,1f);
		}
		#endregion
	}
}