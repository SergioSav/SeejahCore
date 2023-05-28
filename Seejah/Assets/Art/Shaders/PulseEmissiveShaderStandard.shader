Shader "Standard/PulseEmmisiveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	_Color ("Color", Color) = (0.0,0.0,0.0,0.0)
		_Power ("Power", Range(0.1,2.0)) = 1.0
		_Ampl ("Amplitude", float) = 80
	    _PulsePadding ("Pulse Padding", Range(0.1,0.9)) = 0.35
	    _AlbedoMod ("Albedo multiplier", Range(0.0,1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300

		CGPROGRAM
		#pragma surface surf Standard
		
		sampler2D _MainTex;
		float4 _Color;
		float _Power;
		float _Ampl;
		float _PulsePadding;
		float _AlbedoMod;

		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			half sinValue = 0.5 * sin(_Ampl * _Time.x) + 0.5;
			half pulseMultiplier = sinValue * ( 1 - _PulsePadding ) + _PulsePadding;
			o.Emission = c.rgb * _Color.rgb * pulseMultiplier * _Power;
			o.Albedo = c.rgb * _AlbedoMod;
			o.Alpha = c.a;
		}
		ENDCG
    }
	FallBack "Diffuse"
}
