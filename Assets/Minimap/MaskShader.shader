Shader "Minimap/TextureMask" {
   Properties
   {
      _Mask ("Culling Mask", 2D) = "white" {}
      _Cutoff ("Alpha cutoff", Range (0,1)) = 0.5
   }
 
   SubShader
   {
        Tags {"Queue" = "Background"}
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        ZWrite On
        ZTest Always
        Alphatest LEqual [_Cutoff]
        Pass
        {
            SetTexture [_Mask] {combine texture}
        }
   }
}