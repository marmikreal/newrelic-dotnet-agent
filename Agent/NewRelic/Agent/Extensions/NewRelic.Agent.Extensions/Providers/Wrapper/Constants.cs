using System.Collections.Concurrent;

namespace NewRelic.Agent.Extensions.Providers.Wrapper
{
	/// <summary>
	/// This class defines constants that are accessible to both the Agent and the Wrappers.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// This is the key-part that the agent recognizes when trying to find a DistributedTracePayload, typically passed as a KeyValuePair in the header of a request.
		/// </summary>
		public const string DistributedTracePayloadKey = "Newrelic";

		public const string TraceParentHeaderKey = "traceparent";

		public const string TraceStateHeaderKey = "tracestate";
	}

	public enum WebTransactionType
	{
		Action,
		Custom,
		ASP,
		MVC,
		WCF,
		WebAPI,
		WebService,
		MonoRail,
		OpenRasta,
		StatusCode
	}

	public enum MessageBrokerDestinationType
	{
		Queue,
		Topic,
		TempQueue,
		TempTopic,
	}

	public enum MessageBrokerAction
	{
		Produce,
		Consume,
		Peek,
		Purge,
	}

	///<summary>This enum must be a sequence of values starting with 0 and incrementing by 1. See MetricNames.GetEnumerationFunc</summary>
	public enum DatastoreVendor
	{
		//Cassandra,
		Couchbase,
		//Derby,
		//Firebird,
		IBMDB2,
		//Informix,
		Memcached,
		MongoDB,
		MySQL,
		MSSQL,
		Oracle,
		Postgres,
		Redis,
		//SQLite,
		Other
	}

	public static class EnumNameCache<TEnum> // c# 7.3: where TEnum : System.Enum	
	{
		private static readonly ConcurrentDictionary<TEnum, string> Cache = new ConcurrentDictionary<TEnum, string>();
		private static readonly ConcurrentDictionary<TEnum, string> ToLowerCache = new ConcurrentDictionary<TEnum, string>();

		public static string GetName(TEnum enumValue)
		{
			return Cache.GetOrAdd(enumValue, (enumVal) => enumVal.ToString());
		}

		public static string GetNameToLower(TEnum enumValue)
		{
			return ToLowerCache.GetOrAdd(enumValue, (enumVal) => enumVal.ToString().ToLower());
		}
	}

	//This enumeration must exactly match the equivalent enumeration defined in NewRelic.Api.Agent.TransportType.
	public enum TransportType
	{
		Unknown = 0,
		HTTP = 1,
		HTTPS = 2,
		Kafka = 3,
		JMS = 4,
		IronMQ = 5,
		AMQP = 6,
		Queue = 7,
		Other = 8
	}
}