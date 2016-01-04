Shader "Custom/Portal/Portal-Viewer" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Pass {
			Stencil {
				Ref 1
				Comp Equal
			}

			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;
 
			float4 frag(v2f_img input) : COLOR {
				return tex2D(_MainTex, input.uv) * float4(0, 1.0, 0, 1.0);
			}

			ENDCG
		}

		Pass {
			Stencil {
				Ref 2
				Comp Equal
			}

			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;
 
			float4 frag(v2f_img input) : COLOR {
				return tex2D(_MainTex, input.uv) * float4(1.0, 0, 0, 1.0);
			}

			ENDCG
		}
	}
}

