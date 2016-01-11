Shader "Custom/Portals/Portal 1" {
	Properties{}
	SubShader {
		Pass {
			Tags { "Queue"="Geometry-100" }
			Stencil {
				Ref 1
				Comp Always
				Pass Replace
			}
			//ColorMask 0
			ZWrite Off
		}
	} 
}
