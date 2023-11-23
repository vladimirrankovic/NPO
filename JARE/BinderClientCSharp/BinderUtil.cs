using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BinderClientCSharp
{
	public class BinderUtil
	{
		private static RecoveryType recoveryType;
		private static AuthzType authzType;
		private static SubmissionType submissionType;

		public static readonly int PROTOCOL_VERSION = 8;

		private static String scriptString;
		private static String HTMLGstatLink;

		private static bool PERFORMANCE_MONITORING = false;
		/** Binder shutdown command */
		private static String shutdown = "SHUTDOWN";

		private static JavaProperties prop;
		//private static BinderSocketFactory socketFactory;
		private static bool useSSL = true;
		private static bool requireClientAuth = true;

		/** Dont allow instantiation. */
		private BinderUtil() 
		{

		}

		/**
	 * Initializes an <code>int</code> field from the properties. If not found,
	 * default value is used.
	 */
		public static int initIntField(String fieldName, int defaultValue) 
		{
			int n = defaultValue;
			try {
				String val = prop.GetProperty(fieldName);
				if (val != null)
					n = int.Parse(val);
			} 
			catch (FormatException ne) 
			{
			}

			return n;
		}

		/**
	 * Initializes a <code>double</code> field from the properties. If not
	 * found, default value is used.
	 */
		public static double initDoubleField(String fieldName, double defaultValue) {
			double n = defaultValue;
			try 
			{
				String val = prop.GetProperty(fieldName);
				if (val != null)
					n = Double.Parse(val);
			} 
			catch (FormatException ne) 
			{
			}

			return n;
		}

		public static void initService(String propPath) {
			//Djordje Krecar 03/03/15
			/*
			prop = new Properties();
			try {
				prop.load(new FileInputStream(new File(propPath)));
				scriptString = prop.getProperty("Script", System.getProperty("binder.home") + File.separator + "bin"
				                                + File.separator + "submitRemoteJob.sh");

				initSocketFactory();

				// RecoveryType init 
				initRecovery(prop.getProperty("RecoveryType", RecoveryType.NONE.toString()).toUpperCase());
				// Authz init 
				initAuthz(prop.getProperty("AuthzType", AuthzType.NONE.toString()).toUpperCase());
				if (authzType == AuthzType.VOMS) {
					// TODO this can be removed/changed
					CertUtils.init();
					VomsUtils.init();
				}
				initSubType(prop.getProperty("JobSubmissionType", SubmissionType.INTERNAL.toString()).toUpperCase());

				HTMLGstatLink = prop.getProperty("gstatHomeURL");
				shutdown = prop.getProperty("ShutDownCommand", "SHUTDOWN");
				String perfMonitoring = prop.getProperty("GeneratePerformanceMonitoringEvents", "false");
				PERFORMANCE_MONITORING = perfMonitoring.equalsIgnoreCase("true");
			} catch (IOException e) {
				Console.WriteLine("BinderUtil: could not open properties file.");
				e.printStackTrace();
				System.exit(1);
			} catch (GeneralSecurityException e) {
				Console.WriteLine("BinderUtil: could not initialize socket factories.");
				e.printStackTrace();
				System.exit(1);
			}

		}
			*/
		}

		// TODO maybe just use properties directly and support pkcs12, jks and pem
		// formats for certificates
		private static void initSocketFactory() /*throws IOException, GeneralSecurityException*/
		{
			//Djordje Krecar 03/03/15
			/*
			// create options needed for socket factories
			Properties config = new Properties();
			useSSL = prop.getProperty("UseSSL", "yes").equalsIgnoreCase("yes");
			config.setProperty("UseSSL", useSSL ? "yes" : "no");
			requireClientAuth = prop.getProperty("RequireClientAuth", "yes").equalsIgnoreCase("yes");
			config.setProperty("RequireClientAuth", requireClientAuth ? "yes" : "no");

			// credentials
			String binderCert = prop.getProperty("BinderCert");
			String binderCertPass = System.getenv("BINDER_CERT_PASS");
			if (binderCertPass == null) {
				binderCertPass = "";
			}
			config.setProperty(ContextWrapper.CREDENTIALS_STORE_FILE, binderCert);
			config.setProperty(ContextWrapper.CREDENTIALS_STORE_TYPE, "PKCS12");
			config.setProperty(ContextWrapper.CREDENTIALS_STORE_PASSWD, binderCertPass);

			// needed for glite-trustmanager
			String certDir = prop.getProperty("CertificatesDir", "/etc/grid-security/certificates");
			config.setProperty(ContextWrapper.TRUSTSTORE_DIR, certDir);
			String crlUpdateInterval = prop.getProperty("CrlUpdateInterval", "2h");
			config.setProperty(ContextWrapper.CRL_UPDATE_INTERVAL, crlUpdateInterval);
			String credentialsUpdateInterval = prop.getProperty("CredentialsUpdateInterval", "1h");
			config.setProperty(ContextWrapper.CREDENTIALS_UPDATE_INTERVAL, credentialsUpdateInterval);

			socketFactory = new BinderSocketFactory(config);
			*/
		}

		/**
	 * Initializes authentication type.
	 * 
	 * @param type
	 */
		private static void initAuthz(String type)
		{
			if (useSSL && requireClientAuth)
				authzType = Enums.toAuthzType(type);
			else
				authzType = AuthzType.NONE;
		}

		/**
	 * Initializes job submission type.
	 * 
	 * @param type
	 */
		private static void initSubType(String type) {
			submissionType = Enums.toSubType(type);
		}

		/**
	 * Initializes Recovery Module.
	 * 
	 * @param recType
	 */
		private static void initRecovery(String recType)
		{
			recoveryType = Enums.toRecType(recType);
		}

		public static String getProperty(String key) 
		{
			return prop.GetProperty(key);
		}

		public static String getProperty(String key, String defaultValue) 
		{
			return prop.GetProperty(key, defaultValue);
		}

		public static bool SSLEnabled() 
		{
			return useSSL;
		}

		public static String getScriptString()
		{
			return scriptString;
		}

		public static String getShutdown() 
		{
			return shutdown;
		}

		/**
	 * @return
	 */
		public static String getHTMLGstatLink()
		{
			return HTMLGstatLink;
		}

		/**
	 * @param string
	 */
		public static void setHTMLGstatLink(String stringLink) 
		{
			HTMLGstatLink = stringLink;
		}

		public static RecoveryType getRecoveryType()
		{
			return recoveryType;
		}

		public static AuthzType getAuthType() 
		{
			return authzType;
		}

		public static SubmissionType getSubmissionType() 
		{
			return submissionType;
		}

		public static bool isPerfMonEnabled() 
		{
			return PERFORMANCE_MONITORING;
		}

		/// <summary>
		/// Reads the string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="input">BinaryReader</param>
		public static String readString(BinaryReader input) /*throws EOFException, IOException*/ 
		{
			byte[] bytes = readBytes(input);

			if (bytes.Length == 0)
				return "";

			return System.Text.Encoding.UTF8.GetString(bytes);
		}

		/// <summary>
		/// Reads the int.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="input">BinaryReader</param>
		public static int readInt (BinaryReader input)
		{
			byte[] bytes = BitConverter.GetBytes (input.ReadInt32());

			if(BitConverter.IsLittleEndian)
				Array.Reverse (bytes);

			return BitConverter.ToInt32  (bytes,0);
		}

		/// <summary>
		/// Reads the double.
		/// </summary>
		/// <returns>The double.</returns>
		/// <param name="input">BinaryReader</param>
		public static double readDouble (BinaryReader input)
		{
			byte[] bytes = BitConverter.GetBytes (input.ReadDouble());

			if(BitConverter.IsLittleEndian)
				Array.Reverse (bytes);

			return BitConverter.ToDouble  (bytes,0);
		}

		/// <summary>
		/// Firstly writes string length and then writes the string.
		/// </summary>
		/// <param name="output">BinaryWritter.</param>
		/// <param name="s">string.</param>
		public static void writeString(BinaryWriter output, String s) /*throws IOException */
		{
			if (s == null)
				s = "";

			writeInt (output, s.Length);
			output.Write(System.Text.Encoding.UTF8.GetBytes(s));
		}

		/// <summary>
		/// Firstly reads bytes length and then reads the bytes.
		/// </summary>
		/// <returns>The bytes.</returns>
		/// <param name="input">BinaryReader</param>
		public static byte[] readBytes(BinaryReader input) /*throws EOFException, IOException*/
		{
			int len = BinderUtil.readInt (input);
			byte[] bytes = new byte[len];
			bytes = input.ReadBytes(len);
			return bytes;
		}

		/// <summary>
		/// Firstly writes bytes length and then writes the bytes.
		/// </summary>
		/// <param name="output">Output.</param>
		/// <param name="b">BinaryWritter</param>
		public static void writeBytes(BinaryWriter output, byte[] b) /*throws IOException*/
		{
			BinderUtil.writeInt (output,b.Length);
			output.Write(b);
		}

		/// <summary>
		/// Firstly reads the array length and than reads the doubles.
		/// </summary>
		/// <returns>The doubles.</returns>
		/// <param name="input">Input.</param>
		public static double[] readDoubles(BinaryReader input) /*throws EOFException, IOException*/
		{
			int len = BinderUtil.readInt (input);

			double[] b = new double[len];
			for (int i = 0; i < len; i++)
			{
				b[i] = BinderUtil.readDouble(input); 
			}

			return b;
		}

		/// <summary>
		/// Firstly writes the array length and then writes the doubles.
		/// </summary>
		/// <param name="output">Output.</param>
		/// <param name="b">BinaryWriter.</param>
		public static void writeDoubles(BinaryWriter output, double[] b) /*throws IOException */
		{
			BinderUtil.writeInt (output, b.Length);

			for (int i = 0; i < b.Length; i++) 
			{
				BinderUtil.writeDouble (output, b[i]);
			}
		}


		public static StreamWriter getFileOutput(String jobID, String modifier, String dir) /*throws FileNotFoundException*/
		{
			String outFile = getFileOutputName(jobID) + modifier;
			return new StreamWriter(dir + outFile);
		}

		public static String getFileOutputName(String jobID)
		{
			/* substitute :N for _N if there is one in jobID */
			return jobID.LastIndexOf(":") > 0 ? jobID.Replace(':', '_') : jobID;
		}

		/**
	 * Reads arguments from the string. Same as <code>readArgs(s, true)</code>.
	 * <p>
	 * NOTE: Empty string arguments will be ignored!
	 * 
	 * @see #readArgs(String, boolean)
	 * 
	 * @param s
	 *            The <code>String</code> containing arguments.
	 * @return The <code>String</code> array of arguments.
	 */
		public static String[] readArgs(String s)
		{
			return readArgs(s, true);
		}

		/**
	 * Reads arguments from the string.
	 * <p>
	 * NOTE: Empty string arguments will be ignored!
	 * 
	 * @param s
	 *            The <code>String</code> containing arguments.
	 * @param includeQuotes
	 *            If <code>true</code>, includes quotes in quoted arguments.
	 * @return The <code>String</code> array of arguments.
	 */
		public static String[] readArgs(String s, bool includeQuotes)
		{
			/* NOTE: This will also be accepted: '"aa""bb"' */
			s = " " + s.Trim() + " ";
			int lastPos = 0;
			bool insideQuote = false;
			List<String> results = new List<String>();
			for (int i = 1; i < s.Length; i++) {
				if (!insideQuote && (s[i] == ' ' || s[i] == '\t'))
				{
					if (i > lastPos + 1) /* to avoid adding empty strings */
						results.Add(s.Substring(lastPos + 1, i));
					lastPos = i;
				} else if (insideQuote && s[i] == '"') {
					if (i > lastPos + 1) /* to avoid adding empty strings */
						results.Add(includeQuotes ? s.Substring(lastPos, i + 1) : s.Substring(lastPos + 1, i));
					insideQuote = false;
					lastPos = i;
				} else if (!insideQuote && s[i] == '"') {
					insideQuote = true;
					lastPos = i;
				}
			}
			/*
		 * NOTE: If insideQuote == true after the loop, portion of the string
		 * will not be returned! Maybe throw some exception.
		 */
			/* Copy results from ArrayList to Array. */
			return results.ToArray();
		}

		/// <summary>
		/// Writes the bytes of int value.
		/// </summary>
		/// <param name="output">BinaryWriter.</param>
		/// <param name="value">Int value.</param>
		public static void writeInt (BinaryWriter output, int value)
		{
			byte[] bytes = BitConverter.GetBytes (value);

			if(BitConverter.IsLittleEndian)
				Array.Reverse (bytes);

			output.Write (bytes);
		}

		/// <summary>
		/// Writes the bytes of double value.
		/// </summary>
		/// <param name="output">BinaryWriter.</param>
		/// <param name="value">Double value.</param>
		public static void writeDouble (BinaryWriter output, double value)
		{
			byte[] bytes = BitConverter.GetBytes (value);

			if(BitConverter.IsLittleEndian)
				Array.Reverse (bytes);

			output.Write (bytes);
		}

		/// <summary>
		/// Writes the bytes of long value.
		/// </summary>
		/// <param name="output">BinaryWriter.</param>
		/// <param name="value">Long value.</param>
		public static void writeLong (BinaryWriter output, long value)
		{
			byte[] bytes = BitConverter.GetBytes (value);

			if(BitConverter.IsLittleEndian)
				Array.Reverse (bytes);

			output.Write (bytes);
		}
	}
}

