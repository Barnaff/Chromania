Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct vertInput {
                float4 pos : POSITION;
            }; 

            struct vertOutput {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

            vertOutput vert (vertInput input)
            {
                vertOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, input.pos);
                o.uv = mul(UNITY_MATRIX_MV, input.pos);
                return o;
            }

            fixed4 frag (vertOutput output) : SV_Target
            {   
                float2 paletteuv = output.uv.xy / output.uv.z;
                return float4(sin(paletteuv.x*10.0), sin(paletteuv.y*10.0), 0.0, 1.0);
            }
            ENDCG
        }
    }
}
