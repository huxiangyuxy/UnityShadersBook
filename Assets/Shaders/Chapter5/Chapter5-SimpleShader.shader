// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unity Shaders Book/Chapter 5/Simple Shader" {
	Properties {
        // 声明一个 Color 类型的属性
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
	}
	SubShader {
        Pass {
            CGPROGRAM

            // 声明顶点着色器和片元着色器的函数名称
            #pragma vertex vert
            #pragma fragment frag
            
            // 定义一个与属性名称和类型都匹配的变量
            uniform fixed4 _Color;

            // 顶点着色器的输入
			struct a2v {
                // 模型空间的顶点坐标
                float4 vertex : POSITION;
                // 模型空间的法线方向
				float3 normal : NORMAL;
                // 模型的第一套纹理坐标
				float4 texcoord : TEXCOORD0;
            };
            
            // 顶点着色器的输出
            struct v2f {
                // 顶点在裁剪空间中的位置信息
                float4 pos : SV_POSITION;
                // 存储颜色信息
                fixed3 color : COLOR0;
            };
            
            v2f vert(a2v v) {
            	v2f o;
            	o.pos = UnityObjectToClipPos(v.vertex);
            	o.color = v.normal * 0.5 + fixed3(0.5, 0.5, 0.5);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
            	fixed3 c = i.color;
                // 使用 _Color 属性来控制输出颜色
            	c *= _Color.rgb;
                return fixed4(c, 1.0);
            }

            ENDCG
        }
    }
}
