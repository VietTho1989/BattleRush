using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Record
{
	public class DataRecordTask : Data
	{

		public VO<ReferenceData<Data>> needRecordData;

		#region State

		public enum State
		{
			None,
			Start,
			Record,
			Finish
		}

		public VO<State> state;

		#endregion

		public DataRecord record = new DataRecord();

		#region Constructor

		public enum Property
		{
			needRecordData,
			state
		}

		public DataRecordTask() : base()
		{
            this.needRecordData = new VO<ReferenceData<Data>>(this, (byte)Property.needRecordData, ReferenceData<Data>.Null);
			this.state = new VO<State> (this, (byte)Property.state, State.None);
		}

		#endregion

	}
}