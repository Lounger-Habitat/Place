Shader "Custom/HighlightUIShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _HighlightAmount ("Highlight Amount", Range(0, 1)) = 0.5
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        LOD 100

        Pass
        {
            CGPROGRAM
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.rgb += _Color.rgb * _HighlightAmount;
                return col;
            }
            ENDCG
        }
    }
}
