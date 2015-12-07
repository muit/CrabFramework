#if UNITY_4_0||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
#define UNITY_4
#endif

using UnityEngine;

/// Moon position types.
public enum TOD_MoonPositionType
{
	OppositeToSun,
	Realistic
}

/// Stars position types.
public enum TOD_StarsPositionType
{
	Static,
	Rotating
}

/// Fog adjustment types.
public enum TOD_FogType
{
	None,
	Color,
	Directional
}

/// Ambient light types.
public enum TOD_AmbientType
{
#if UNITY_4
	None,
	Color
#else
	None,
	Color,
	Gradient,
	Spherical
#endif
}

/// Reflection cubemap types.
public enum TOD_ReflectionType
{
#if UNITY_4
	None
#else
	None,
	Cubemap
#endif
}

/// Color space types.
public enum TOD_ColorSpaceType
{
	Auto,
	Linear,
	Gamma
}

/// Dynamic color range types.
public enum TOD_ColorRangeType
{
	Auto,
	HDR,
	LDR
}

/// Cloud rendering qualities.
public enum TOD_CloudQualityType
{
	Fastest,
	Density,
	Bumped
}

/// Mesh vertex count levels.
public enum TOD_MeshQualityType
{
	Low,
	Medium,
	High
}

/// Sky rendering qualities.
public enum TOD_SkyQualityType
{
	PerVertex,
	PerPixel
}

/// Cloud coverage types.
public enum TOD_CloudType
{
	Custom,
	None,
	Few,
	Scattered,
	Broken,
	Overcast
}

/// Weather types.
public enum TOD_WeatherType
{
	Custom,
	Clear,
	Storm,
	Dust,
	Fog
}
