using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// Encoding GIF example.
	/// </summary>
	public class EncodeExample : ExampleBase
    {
		public List<Texture2D> Frames;
		public AnimatedImage AnimatedImage;

		public void Start()
		{
			var frames = Frames.Select(f => new GifFrame(f, 0.1f)).ToList();
			var gif = new Gif(frames);
			var bytes = gif.Encode();

			#if UNITY_EDITOR

			var path = UnityEditor.EditorUtility.SaveFilePanel("Save", "", "EncodeExample", "gif");

			#elif UNITY_WEBGL

			var path = "";
			Debug.LogWarning("Saving files is not supported on WebGL.");

			#else

            var path = Path.Combine(Application.persistentDataPath, "EncodeExample.gif");

			#endif

			if (path == "") return;

			File.WriteAllBytes(path, bytes);
			Debug.Log($"Saved to: {path}");
			AnimatedImage.Play(gif);
		}
	}
}