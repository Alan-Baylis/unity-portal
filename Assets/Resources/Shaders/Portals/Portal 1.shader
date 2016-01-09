Shader "Custom/Portals/Portal 1" {
	Properties{}
	SubShader {
		Pass {
			Stencil {
				Ref 1
				Comp Always
				Pass Replace
			}
		}
	} 
}
