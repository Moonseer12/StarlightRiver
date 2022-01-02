float time;
float air;

texture sampleTexture0;
sampler2D samplerTex0 = sampler_state { texture = <sampleTexture0>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

texture sampleTexture1;
sampler2D samplerTex1 = sampler_state { texture = <sampleTexture1>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture2;
sampler2D samplerTex2 = sampler_state { texture = <sampleTexture2>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

texture sampleTexture3;
sampler2D samplerTex3 = sampler_state { texture = <sampleTexture3>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 st = coords;
    float2 st1 = st;

    float freeze = max(0.0, air - tex2D(samplerTex3, st).x);

    float value = tex2D(samplerTex1, st1).x * freeze;
    float value2 = tex2D(samplerTex2, st1 + float2(time, time) * 0.05).x * 0.2 * freeze;

    float distort = (value2) * 0.5;

    st.xy += float2(cos(value * 6.28), sin(value * 6.28)) * distort;

    float4 color = tex2D(samplerTex0, st);
    color += value2;

    float progress = st.x + st.y;
    float r = 10.0 * (1.0 + sin(time + progress * 0.6));
    float g = 14.0 * (1.0 + cos(time + progress * 3.0));
    float b = 18.0;

    color += float4(r, g, b, 0) * (value + value2 * value) * 0.05 * tex2D(samplerTex2, st1).x;

    return color;
}

technique Technique1
{
    pass GlowingDustPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}