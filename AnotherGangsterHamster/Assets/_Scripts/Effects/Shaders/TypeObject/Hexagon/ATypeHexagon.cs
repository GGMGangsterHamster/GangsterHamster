using Objects;
using Tween;
using UnityEngine;

namespace Effects.Shaders.TypeObject
{
   public class ATypeHexagon : MonoBehaviour
   {
      public const string WALLHEIGHT = "_WallHeight";
      public const string WALLSCALE = "_WallScale";
      public const string BLOOM = "_Bloom";
      public const string FADEINGSPEED = "_FadingSpeed";
      public const string MAXALPHA = "_MaxAlpha";
      public const string MINALPHA = "_MinAlpha";

      public float wallScale = 1.0f;
      public float bloom = 1.0f;
      public Vector3Int considerDirection = Vector3Int.down;
      public Transform baseTransform = null;

      private Material _mat;

      private float _height;


      private void Awake()
      {
         _mat = GetComponent<Renderer>().material;
         _height = 0;

         _height += (considerDirection.x != 0 ?
            Mathf.Abs(transform.position.x - baseTransform.position.x) : 0);

         _height += (considerDirection.y != 0 ?
            Mathf.Abs(transform.position.y - baseTransform.position.y) : 0);

         _height += (considerDirection.z != 0 ?
            Mathf.Abs(transform.position.z - baseTransform.position.z) : 0);

         _mat.SetFloat(WALLHEIGHT, _height / wallScale);
         _mat.SetFloat(WALLSCALE, 1.0f / wallScale);
      }

      private void Update()
      {
         _mat.SetFloat(BLOOM, bloom);
      }
   }
}