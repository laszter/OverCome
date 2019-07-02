Shader "Unlit/UnlitTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
			Lighting Off
            ZWrite On
            Cull Back

			SetTexture [_MainTex] {
                constantColor [_Color]
                Combine texture * constant, texture * constant
            }

        }
    }
}
