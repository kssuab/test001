Shader "EasyFlow/UnlitAlpha"
{
	Properties
	{
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Transparency (A)", 2D) = "white" {}
	}

	SubShader
	{
		ZWrite On
		Cull Off
		Tags { "Queue" = "Transparent" }
		
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
			Material
			{
				Diffuse [_Color]
			}
            Lighting Off
            SetTexture [_MainTex]
            {
                constantColor [_Color]
                Combine texture * constant, texture * constant 
            } 
        }
	} 
}