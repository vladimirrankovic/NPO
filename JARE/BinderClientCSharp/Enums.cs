using System;

namespace BinderClientCSharp
{
	public enum AccessType { BINDER=0, DIRECT, CUSTOM, UNKNOWN }
	public enum JobStatus  { SUBMITTED = 0, READY = 1, BUSY = 2, REUSABLE = 3, FINISHED = 4 }
	public enum RecoveryType {NONE, EJB }
	public enum SubmissionType { INTERNAL, SCRIPT }

	public enum ConnectionType
	{
		CLIENT = 0,
		/** Worker connected. */
		WORKER = 1,
		/* Client connected to query the status of matching CEs. */
		CE_QUERY = 2
	}
	public enum AuthzType
	{
		/** No authentication used. */
		NONE = 0,
		/** */
		VOMS = 1
	}

	public class Enums
	{
		public static AccessType toAccessType(int accessType)
		{
			switch (accessType)
			{
				case 0:
				return AccessType.BINDER;
				case 1:
				return AccessType.DIRECT;
				case 2:
				return AccessType.CUSTOM;
				default:
				return AccessType.UNKNOWN;
			}
		}

		public static AccessType toAccessType(string accessType)
		{
			if(AccessType.BINDER.ToString() == accessType)
				return AccessType.BINDER;
		    else if(AccessType.DIRECT.ToString() == accessType)
				return AccessType.DIRECT;
		    else if(AccessType.CUSTOM.ToString() == accessType)
				return AccessType.CUSTOM;
			else
				return AccessType.UNKNOWN;
		}

		public static JobStatus toJobStatus(int jobStatus)
		{
			switch (jobStatus)
			{
				case 0:
				return JobStatus.SUBMITTED;
				case 1:
				return JobStatus.READY;
				case 2:
				return JobStatus.BUSY;
				case 3:
				return JobStatus.REUSABLE;
				case 4:
				return JobStatus.FINISHED;
				default:
					throw new EnumConstantNotPresentException(jobStatus.ToString());
			}
		}

		public static SubmissionType toSubType(String subType) 
		{
			if (subType.Equals(SubmissionType.INTERNAL.ToString(),StringComparison.OrdinalIgnoreCase))
				return SubmissionType.INTERNAL;
			return SubmissionType.SCRIPT;
		}

		public static RecoveryType toRecType(String recType) 
		{
			if (recType.Equals ("ejb", StringComparison.OrdinalIgnoreCase))
				return RecoveryType.EJB;
			else
				return RecoveryType.NONE;

		}

		public static ConnectionType toConnType(int connType)
		{
			switch (connType)
			{
				case 1:
				return ConnectionType.WORKER;
				case 2:
				return ConnectionType.CE_QUERY;
				default:
				return ConnectionType.CLIENT;
			}
		}


		public static AuthzType toAuthzType(int authType) 
			{
			switch (authType) {
				case 1:
					return AuthzType.VOMS;
				default:
				return AuthzType.NONE;
			}
		}

		/**
	 * A method that converts a string value into an <code>AuthzType</code>
	 * enum.
	 * 
	 * @param authType
	 * @return Type of the authentication used, default is <code>NONE</code>
	 *         
	 */
		public static AuthzType toAuthzType(String authType) 
		{
			if (authType.Equals ("VOMS", StringComparison.OrdinalIgnoreCase))
				return AuthzType.VOMS;
				else
				return AuthzType.NONE;
		}
	}
}

