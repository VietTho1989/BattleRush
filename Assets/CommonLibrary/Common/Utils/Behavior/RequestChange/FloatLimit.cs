using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FloatLimit : Data
{

	public enum Type
	{
		No,
		Have
	}

	public abstract Type getType();


	#region No

	public class No : FloatLimit
	{

		#region Constructor

		public enum Property
		{

		}

		public No() : base()
		{

		}

		#endregion

		public override Type getType ()
		{
			return Type.No;
		}

	}

	#endregion

	#region Have

	public class Have : FloatLimit
	{

		public VO<float> min;

		public VO<float> max;

		#region Constructor

		public enum Property
		{
			min,
			max
		}

		public Have() : base()
		{
			this.min = new VO<float>(this, (byte)Property.min, 0);
			this.max = new VO<float>(this, (byte)Property.max, 0);
		}

		#endregion

		public override Type getType ()
		{
			return Type.Have;
		}

	}

	#endregion

}