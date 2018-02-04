float4x4 WorldViewProjection;
Texture theTexture;
Texture theMask;
float4 Color;
float Alpha;

sampler ColoredTextureSampler = sampler_state {
	texture = <theTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = POINT;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler ColoredTextureMask = sampler_state {
	texture = <theMask>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = POINT;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 textureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 textureCoordinates : TEXCOORD0;
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(input.Position, WorldViewProjection);
	output.textureCoordinates = input.textureCoordinates;

    return output;
}


struct PixelShaderInput {
	float2 textureCoordinates : TEXCOORD0;
};


//�ݒ肵���摜��ݒ�F��t���ĕ`�悷��
float4 PixelShaderFunction1(PixelShaderInput input) : COLOR0
{
	float4 color = tex2D(ColoredTextureSampler, input.textureCoordinates);
	color.r = Color.r * color.a;
	color.g = Color.g * color.a;
	color.b = Color.b * color.a;
    return color;
}

//�F�t�����Ȃ��āAAlpha�l�ݒ肠��
float4 PixelShaderFunction2(PixelShaderInput input) : COLOR0
{
	float4 color = tex2D(ColoredTextureSampler, input.textureCoordinates);
	color.r = color.r * color.a * Alpha* 1.3f;
	color.g = color.g * color.a * Alpha* 1.3f;
	color.b = color.b * color.a * Alpha* 1.3f;
	color.a *= Alpha;
	return color;
}

//Mask����
float4 PixelShaderFunction3(PixelShaderInput input) : COLOR0
{
	float4 color = tex2D(ColoredTextureSampler, input.textureCoordinates);
	float4 colorMask = tex2D(ColoredTextureMask, input.textureCoordinates);
	color.r = color.r * color.a * (1 - colorMask.a);
	color.g = color.g * color.a * (1 - colorMask.a);
	color.b = color.b * color.a * (1 - colorMask.a);
	color.a *= (1 - colorMask.a);
	return color;
}


//�ݒ肵���摜��ݒ�F��t���ĕ`�悷��
technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction1();
    }
}

//�F�t�����Ȃ��āA���̂܂ܐݒ肵���摜��`��
technique Technique2
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction2();
	}
}

//Mask����
technique Technique3
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction3();
	}
}