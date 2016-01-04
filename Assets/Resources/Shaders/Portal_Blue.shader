Shader "Custom/Portal/Portal_Blue" {

	Properties {}

	SubShader {
		Pass {
			Tags { "RenderType"="Opaque" "Queue"="Geometry-100" }
			Stencil {
				Ref 2
				Comp Always
				Pass Replace
			}
			LOD 200
			ColorMask 0
			ZWrite Off

			CGPROGRAM

			// Compiler Directives
			#pragma vertex VS_MAIN
			#pragma fragment FS_MAIN

			// Predefined variables and helper functions (Unity specific)
			#include "UnityCG.cginc"

			// Input Structs
			struct FS_INPUT {
				float4 pos	: SV_POSITION;
			};

			// VERTEX FUNCTION
			FS_INPUT VS_MAIN(appdata_base input) {
				FS_INPUT output;
				// Setting FS_MAIN input struct
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				return output;
			}

			// FRAGMENT FUNCTION
			float4 FS_MAIN(FS_INPUT input) : COLOR {
				return float4(0.0, 0.0, 0.0, 1.0);
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
