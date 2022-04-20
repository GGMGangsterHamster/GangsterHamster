Shader "Hedgehog Team/Carrousel" {

    Properties {
        //_Tint ("Tint Color", Color) = (.9, .9, .9, 1.0)
        _Tint ("Tint Color", Color) = (.9, .9, .9, 1.0)
        _TexMat1 ("Base (RGB)", 2D) = "white" {}
        _TexMat2 ("Base (RGB)", 2D) = "white" {}
        _Blend ("Blend", Range(0.0,1.0)) = 0.0
    }
    
    Category {
	
        ZWrite On
        Alphatest Greater 0
        Tags {"Queue"="Transparent" "IgnoreProjector"="True"}
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Lighting Off
		
    SubShader {
        Pass {
            
            Material {
                Diffuse [_Tint]
                Ambient [_Tint]
            }
			
            SetTexture [_TexMat1] {
            	Combine   texture
            }

            
            SetTexture [_TexMat2] { constantColor (1,1,1,[_Blend]) combine texture lerp(constant) previous }  
			
			// Unity 4 and Unity 5
			//setTexture [primary] {combine texture +- previous , previous}
			setTexture [primary] {combine texture lerp (constant) previous}
			
        }
    } 
}

    FallBack " Diffuse", 1
}