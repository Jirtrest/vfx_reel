Shader "Test/AlphaAddBlend"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {} 
		_ColorMultiplier("Color Multiplier", color) = (1,1,1,1)
		//[MaterialToggle]
		_Blend("Additive/AlphaBlend", float) = 1
		
	}
		SubShader{
			Tags{
			"IgnoreProjector" = "True"
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
			Pass{
			ZWrite Off
			Cull Off // culling, cull
			Blend One OneMinusSrcAlpha 
			// alpha blend // color = RGB_SOURCE * ALPHA_SOURCE + RGB_DESTINATION * (1 - ALPHA_SOURCE); // (1, ALPHA_SOURCE, RGB_SOURCE, RGB_DESTINATION)
			// additive blend // color = RGB_SOURCE * 1 + RGB_DESTINATION * 1; Blend One One
			// super blend // color = RGB_SOURCE * 1 + RGB_DESTINATION * (1 - ALPHA_SOURCE)


			CGPROGRAM
				#pragma vertex blalblavla_BEPCNHA
				#pragma fragment pixel_shader

			
				#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;

			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			
			float _Blend;
			float4 _ColorMultiplier; 


			v2f blalblavla_BEPCNHA(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);// multiply, add
				o.color = v.color;
				return o;
			}
			
			fixed4 pixel_shader (v2f inputFromVertexShader) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, inputFromVertexShader.uv);
				col = col * inputFromVertexShader.color;
				col.rgb = col.rgb * col.a;
				return fixed4(col.rgb, col.a * _Blend);
			}
			ENDCG
		}
	}
}
