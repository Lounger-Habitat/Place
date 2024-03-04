Shader "Custom/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ExpandingSpeed ("Expanding Speed", Range(0.1, 1.0)) = 0.1
        _TrajectoryTex ("Trajectory Texture", 2D) = "black" {}
        _AimPointX ("AimPointX", Range(0,1)) = 0.5
        _AimPointY ("AimPointY", Range(0,1)) = 0.5
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 euv : TEXCOORD1;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 euv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.euv = v.euv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _TrajectoryTex;
            float _ExpandingSpeed;
            float _AimPointX;
            float _AimPointY;
            float startTime;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 trajectory =  tex2D(_TrajectoryTex, i.euv);
                if (trajectory.a != 0)
                {
                    if (startTime == 0)
                    {
                        // 记录开始时间
                        startTime = _Time.y;
                    }
                    // 大小 ，随时间 由1 到 0，scale 0.1 -> 0
                    // float size = smoothstep(1, 0, _Time.y, 2 * _ExpandingSpeed) * 0.1;
                    // 计算经过的时间
                    float elapsedTime = _Time.y - startTime;

                    // 如果经过的时间小于1秒，执行倒计时逻辑
                    float countdown = smoothstep(0, 1, elapsedTime);
                    float size = smoothstep(1, 0, countdown * _ExpandingSpeed) * 0.1;


                    // 目标点 与 图 其他位置 的 距离
                    // float d = distance(i.uv, float2(_AimPointX, _AimPointY));
                    float d = distance(i.uv, i.euv);

                    // Calculate the smoothstep value for controlling intensity decrease
                    // 强度 递减 , 1 - 距离 ， 距离远，强度小 0.9 到 1
                    float intensityDecrease = 1.0 - smoothstep(0, size, d);

                    // If the distance is less than the size, set color to white, otherwise set to transparent
                    fixed4 col = float4(0, 0, 0, 0);
                    if (intensityDecrease == 0)
                    {
                        col = tex2D(_MainTex, i.uv);
                    }else
                    {
                        col = tex2D(_TrajectoryTex, i.uv);
                    }
                    

                    // Return the calculated color
                    return col;

                }
                return tex2D(_MainTex, i.uv);

            }
            ENDCG
        }
    }
}
