using UnityEngine;

public class TOD_MinAttribute : PropertyAttribute
{
	public float min;

	public TOD_MinAttribute(float min)
	{
		this.min = min;
	}
}

public class TOD_MaxAttribute : PropertyAttribute
{
	public float max;

	public TOD_MaxAttribute(float max)
	{
		this.max = max;
	}
}

public class TOD_RangeAttribute : PropertyAttribute
{
	public float min;
	public float max;

	public TOD_RangeAttribute(float min, float max)
	{
		this.min = min;
		this.max = max;
	}
}
