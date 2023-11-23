using System;
using System.Collections;
using System.IO;


namespace BinderClientCSharp
{
	public class JavaProperties : Hashtable
	{
		protected Hashtable defaults;

		public JavaProperties()
		{
		}

		public JavaProperties( Hashtable defaults )
		{
			this.defaults = defaults;
		}

		public void Load( Stream streamIn )
		{
			JavaPropertyReader reader = new JavaPropertyReader( this );
			reader.Parse( streamIn );
			streamIn.Close ();
		}

		public string GetProperty( string key )
		{
			Object objectValue = this[ key ];
			if( objectValue != null )
			{
				return AsString( objectValue );
			}
			else if( defaults != null )
			{
				return AsString( defaults[ key ] );
			}

			return null;
		}

		public string GetProperty( string key, string defaultValue )
		{
			string val = GetProperty( key );
			return (val == null) ? defaultValue : val;
		}

		public string SetProperty( string key, string newValue )
		{
			string oldValue = AsString( this[ key ] );
			this[ key ] = newValue;
			return oldValue;
		}

		private string AsString( Object o )
		{
			if( o == null )
			{
				return null;
			}

			return o.ToString();
		}

	}
}

