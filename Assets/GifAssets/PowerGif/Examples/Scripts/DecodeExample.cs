using System.IO;
using UnityEngine;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// Decoding GIF example.
	/// </summary>
	public class DecodeExample : ExampleBase
    {
		public AnimatedImage AnimatedImage;
        public byte[] Bytes;

        public void OnValidate()
        {
            Bytes = File.ReadAllBytes("Assets/GifAssets/PowerGif/Examples/Samples/Large.gif");
        }

		public void Start()
        {
            var gif = Gif.Decode(Bytes);

            AnimatedImage.Play(gif);
		}
	}
}