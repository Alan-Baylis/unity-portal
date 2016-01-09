Shader "Custom/Portals/Portal 2" {
	Properties{}
	SubShader {
		Pass {
			Stencil {
				Ref 2
				Comp Always
				Pass Replace
			}
		}
	} 
}
