Shader "Lilith/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Outline Colour", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(1.0, 5.0)) = 1.2
    }
    SubShader
    {
        Tags { "Queue" = "Transparent+1" "RenderType" = "Transparent"}
        

        Pass //Outline
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            float4 _Color;
            float _OutlineWidth;

            struct MeshData
            {
                //a
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct Interpolators
            {
                float4 pos : POSITION;
                float4 colour : COLOR;
                //float3 normal : NORMAL;
            };

            Interpolators vert(MeshData v)
            {
                v.vertex.xyz *= _OutlineWidth;

                Interpolators o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.colour = _Color;
                return o;
            }

            float4 frag(Interpolators i) : COLOR
            {
                return i.colour;
            }
            ENDCG
              
        }

        //Pass //Object
        //{
        //    ZWrite On

        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    // make fog work
        //    #pragma multi_compile_fog

        //    #include "UnityCG.cginc"

        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float2 uv : TEXCOORD0;
        //    };

        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        UNITY_FOG_COORDS(1)
        //        float4 vertex : SV_POSITION;
        //    };

        //    sampler2D _MainTex;
        //    float4 _MainTex_ST;

        //    v2f vert (appdata v)
        //    {
        //        v2f o;
        //        o.vertex = UnityObjectToClipPos(v.vertex);
        //        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //        UNITY_TRANSFER_FOG(o,o.vertex);
        //        return o;
        //    }

        //    fixed4 frag (v2f i) : SV_Target
        //    {
        //        // sample the texture
        //        fixed4 col = tex2D(_MainTex, i.uv);
        //        // apply fog
        //        UNITY_APPLY_FOG(i.fogCoord, col);
        //        return col;
        //    }
        //    ENDCG
        //}
    }
}
