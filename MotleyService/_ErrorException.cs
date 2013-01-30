using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotleyService
{
	[Serializable]
	public class _ErrorException : Exception
	{
		public string ErrorMessage
		{
			get { return base.Message.ToString(); }
		}

		public _ErrorException(string errorMessage) : base(errorMessage) { }

		public _ErrorException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
	}
}
