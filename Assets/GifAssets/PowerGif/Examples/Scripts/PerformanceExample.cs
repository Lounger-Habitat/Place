using System.IO;
using UnityEngine;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// This example shows how to check encoding/decoding operations performance.
	/// </summary>
	public class PerformanceExample : ExampleBase
    {
		public AnimatedImage AnimatedImage;
        public byte[] Bytes;

        private readonly System.Diagnostics.Stopwatch _stopwatch = new();

        public void OnValidate()
        {
            Bytes = File.ReadAllBytes("Assets/GifAssets/PowerGif/Examples/Samples/Large.gif");
        }

        public void Start()
		{
			_stopwatch.Reset();
			_stopwatch.Start();

			var gif = Gif.Decode(Bytes);

			Debug.Log($"Decoded in {_stopwatch.Elapsed.TotalSeconds:N2}s");

			_stopwatch.Restart();

			gif.Encode();

			Debug.Log($"Encoded in {_stopwatch.Elapsed.TotalSeconds:N2}s");

			AnimatedImage.Play(gif);
		}
	}
}