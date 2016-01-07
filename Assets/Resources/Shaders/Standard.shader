Shader "Custom/Standard" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass {
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			void surf (Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}

		Pass {
			Tags{ "RenderType" = "Opaque" }
			CGPROGRAM
		
			// Compiler Directives
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex VS_MAIN
			#pragma fragment FS_MAIN
			
			// Predefined variables and helper functions (Unity specific)
			#include "UnityCG.cginc"
			
			// Uniform Variables (Properties)
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			// Input Structs
			struct FS_INPUT {
				float4 pos		: SV_POSITION;
				half2 uv		: TEXCOORD0;
			};
			
			// VERTEX FUNCTION
			FS_INPUT VS_MAIN (appdata_base input) {
				FS_INPUT output;
				
				// Setting FS_MAIN input struct
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
				
				return output;
			}
			
			// FRAGMENT FUNCTION
			fixed4 FS_MAIN (FS_INPUT input) : COLOR {
				
				return tex2D(_MainTex, input.uv) * _Color;
			}
		
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
