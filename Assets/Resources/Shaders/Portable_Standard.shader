Shader "Custom/Portable_Standard" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ClipPlaneNorm ("Clipping Plane Normal", Vector) = (0,1,0)
		_ClipPlanePos("Clipping Plane Position", Vector) = (0,0,0)
		_IsClipPlaneActive ("Is Clip Plane Active (boolean)", Int) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Stencil {
			Ref 0
			Comp Always
			Pass Replace
		}

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		uniform float3 _ClipPlaneNorm;
		uniform float3 _ClipPlanePos;
		uniform bool _IsClipPlaneActive;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			// clip fragments behind portal
			float3 clipPlaneToFrag = IN.worldPos - _ClipPlanePos;
			float side = dot(_ClipPlaneNorm, clipPlaneToFrag);
			if (side < 0 && _IsClipPlaneActive) {
				discard;
			}

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
