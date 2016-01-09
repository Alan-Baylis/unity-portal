Shader "Custom/Portals/Portal Effect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Portal1Tex("Portal 1 View", 2D) = "white" {}
		_Portal2Tex("Portal 2 View", 2D) = "white" {}
	}
	SubShader {
		Pass {
			Stencil {
				Ref 0
				Comp Equal
			}
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;
 
			float4 frag(v2f_img i) : COLOR {
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
		Pass {
			Stencil {
				Ref 1
				Comp Equal
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _Portal1Tex;
			
			v2f_img vert(appdata_img input) {
				v2f_img output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.uv = half2(input.texcoord.x, 1.0 - input.texcoord.y);
				return output;
			}

			float4 frag(v2f_img input) : COLOR {
				return tex2D(_Portal1Tex, input.uv);
			}
			ENDCG
		}
		Pass {
			Stencil {
				Ref 2
				Comp Equal
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _Portal2Tex;
			
			v2f_img vert(appdata_img input) {
				v2f_img output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.uv = half2(input.texcoord.x, 1.0 - input.texcoord.y);
				return output;
			}

			float4 frag(v2f_img input) : COLOR {
				return tex2D(_Portal2Tex, input.uv);
			}
			ENDCG
		}
	}
}