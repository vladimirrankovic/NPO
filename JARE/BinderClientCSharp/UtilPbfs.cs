using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
//using GLib;
using log4net;

namespace BinderClientCSharp
{
	public class UtilPbfs
	{
		const int BUFFER_LENGTH=1024;


		/** Don't allow instantiation. */
		private UtilPbfs ()
		{

		}
		
		/// <summary>
		/// ZIP archive containing files taken as arguments in String array.
		/// Mainly used by server process in order to pack large resulting files.
		/// </summary>
		/// <param name="outFilename">Out filename.</param>
		/// <param name="filenames">Filenames.</param>
		public static void createZIP(String outFilename, String[] filenames)
		{
			byte[] buf = new byte[BUFFER_LENGTH];
			int len;

			if (File.Exists (outFilename)) 
			{
				logger.Warn ("File: " + outFilename + " already exists so existing file will be deleted!");
				File.Delete (outFilename);
			}

			ZipOutputStream output = new ZipOutputStream(File.Create(outFilename));

			foreach (String filename in filenames) 
			{
				if (!File.Exists (filename)) 
				{
					logger.Warn ("File: " + filename + " does not exist!");
					continue;
				}

				BinaryReader input = new BinaryReader(File.Open(filename, FileMode.Open));

				output.PutNextEntry(new ZipEntry(filename));
				while ((len = input.Read (buf, 0, BUFFER_LENGTH)) > 0)
					output.Write(buf, 0, len);

				output.CloseEntry();
				input.Close();
			}

			output.Close();
		}

		/// <summary>
		/// Method used to unpack ZIP archive, mainly used by client class
		/// </summary>
		/// <param name="zipfilename">Zipfilename.</param>
		public static void unZIP(String zipfilename)
		{
			BufferedStream dest = null;

			if (!File.Exists (zipfilename)) 
			{
				logger.Warn ("File: " + zipfilename + " does not exist!");
				return;
			}

			BinaryReader fis = new BinaryReader(File.Open(zipfilename, FileMode.Open));
			ZipInputStream zis = new ZipInputStream(fis.BaseStream);
			ZipEntry entry;

			while((entry = zis.GetNextEntry()) != null) 
			{
				Console.WriteLine("Extracting: " + entry);
				int count;
				byte[] data = new byte[BUFFER_LENGTH];

				if (File.Exists (entry.Name)) 
				{
					logger.Warn ("File: " + entry.Name + " (in ZIP:" + zipfilename + ") already exists so existing file will be deleted!");
					File.Delete (entry.Name);
				}

				BinaryWriter fos = new BinaryWriter(File.Create(entry.Name));
				dest = new BufferedStream(fos.BaseStream, BUFFER_LENGTH);
	
				while ((count = zis.Read(data, 0, BUFFER_LENGTH)) != -1 && count != 0)
					dest.Write(data,0,count);

				dest.Flush();
				dest.Close();
			}
			zis.Close();
		}


		/// <summary>
		/// Method used to send file to "out" stream
		/// IMPORTANT: begins with "read" operation, ends with "write" operation
		/// </summary>
		/// <param name="filename">Filename.</param>
		/// <param name="input">Input.</param>
		/// <param name="output">Output.</param>
		public static void sendFile(String filename, BinaryReader input, BinaryWriter output)
		{
			if (!File.Exists (filename)) 
			{
				logger.Warn ("File: " + filename + " does not exist!");
				return;
			}

			BinaryReader fileStream = new BinaryReader(File.Open(filename, FileMode.Open));

			byte[] buffer = new byte[BUFFER_LENGTH];
			int duzina, i;

			while ((duzina=fileStream.Read(buffer,0,1024)) > 0) 
			{
				BinderUtil.readString(input);
				byte[] buffer1 = new byte[duzina];
				for (i=0; i<buffer1.Length; i++) 
					buffer1[i] = buffer[i];

				BinderUtil.writeBytes(output,buffer1);
				// Null buffer
				for (i=0; i<BUFFER_LENGTH; i++) 
					buffer[i] = Convert.ToByte('\0');
			}

			fileStream.Close();
			BinderUtil.readString(input);
			BinderUtil.writeString( output, "-finished-" );
		}

		/// <summary>
		/// Method used to receive file from "in"
		/// IMPORTANT: begins with "write" operation, ends with "read" operation
		/// </summary>
		/// <param name="filename">Filename.</param>
		/// <param name="input">Input.</param>
		/// <param name="output">Output.</param>
		public static void receiveFile(String filename, BinaryReader input, BinaryWriter output)
		{
			String line = "";

			if (File.Exists (filename)) 
			{
				logger.Warn ("File: " + filename + " already exists so existing file will be deleted!");
				File.Delete (filename);
			}

			BinaryWriter fileStream = new BinaryWriter(File.Create(filename));	

			while ( !line.Equals("-finished-", StringComparison.OrdinalIgnoreCase))
			{
				BinderUtil.writeString(output, "-continue-");
				byte[] buffer = BinderUtil.readBytes(input);
				line = System.Text.Encoding.UTF8.GetString(buffer);

				if ( !line.Equals("-finished-", StringComparison.OrdinalIgnoreCase)) 
					fileStream.Write(buffer);
			}
			fileStream.Close();		
		}

		private static readonly ILog logger = 
			LogManager.GetLogger(typeof(UtilPbfs));

	}
}

