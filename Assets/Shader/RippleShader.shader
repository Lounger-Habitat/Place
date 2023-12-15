Shader "Custom/RippleShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RippleOrigin ("Ripple Origin", Vector) = (0.5,0.5,0,0)
        _RippleStrength ("Ripple Strength", Float) = 0.5
        _RippleDistance ("Ripple Distance", Float) = 5.0
        _RippleColor ("Ripple Color", Color) = (0.5,0,0,1)
        _RippleShow ("Show Ripple", Float) = 1.0
        _RippleTime ("Ripple Time", Float) = -1000
        _RippleMaxRadius ("Ripple Max Radius", Float) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 _RippleOrigin;
        half _RippleStrength;
        half _RippleDistance;
        fixed4 _RippleColor;
        half _RippleShow;
        half _RippleTime;
        half _RippleMaxRadius;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //if (_RippleShow > 0.5) // 当 _RippleShow 大于 0.5 时，显示涟漪
            //{
            float3 worldDistance = IN.worldPos - _RippleOrigin;
            float dist = length(worldDistance.xz);
            
            float timeFactor = _Time.y - _RippleTime;
            if (timeFactor < 0) timeFactor = 0;

            float ripple = sin(dist * _RippleDistance - timeFactor * 3.14) * _RippleStrength;
            ripple *= smoothstep(_RippleMaxRadius, 0, dist); // 在涟漪半径外部开始衰减

            // 应用涟漪到颜色中
            fixed4 c = fixed4(ripple, ripple, ripple, 1 - _Time.y);
            // c.rgb = lerp(c.rgb, _RippleColor.rgb, _RippleShow);

            //}
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
