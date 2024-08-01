using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoGifTest : MonoBehaviour
{
   public TexturesToGIF_Demo gif1;
   public TexturesToGIF_Demo gif2;

   [ContextMenu("load")]
   public void LoadAll()
   {
      gif1.ConvertTex2DToGIF(() =>
      {
         Delay();
         //Invoke("Delay",1f);
      });
   }

   private void Delay()
   {
      gif2.ConvertTex2DToGIF();
   }
}
