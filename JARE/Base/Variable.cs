/// <summary> Variable.java
/// 
/// </summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
namespace JARE.Base
{
	
	/// <summary> This abstract class is the base for defining new types of variables.
	/// Many methods of <code>Variable</code> (<code>getValue</code>,
	/// <code>setValue</code>,<code>
	/// getLowerLimit</code>,<code>setLowerLimit</code>,<code>getUpperLimit</code>,
	/// <code>setUpperLimit</code>)
	/// are not applicable to all the subclasses of <code>Variable</code>.
	/// For this reason, they are defined by default as giving a fatal error.
	/// </summary>
	[Serializable]
	public abstract class Variable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Sets the type of the variable. The types are defined in class Problem.</summary>
		/// <summary> Gets the type of the variable. The types are defined in class Problem.</summary>
		/// <returns> The type of the variable
		/// </returns>
		virtual public Type VariableType
		{
			/*
			public void setVariableType(m_variableType variableType) {
			type_ = variableType ;
			} // setVariableType*/
			
			get
			{
				return this.GetType();
			}
			// getVariableType
			
		}
		
		//private m_variableType type_;
		
		/// <summary> Creates an exact copy of a <code>Variable</code> object.</summary>
		/// <returns> the copy of the object.
		/// </returns>
		public abstract Variable deepCopy();
		
		/// <summary> Gets the double value representign the variable. 
		/// It is used in subclasses of <code>Variable</code> (i.e. <code>Real</code> 
		/// and <code>Int</code>).
		/// As not all objects belonging to a subclass of <code>Variable</code> have a 
		/// double value, a call to this method it is considered a fatal error by 
		/// default, and the program is terminated. Those classes requiring this method 
		/// must redefine it.
		/// </summary>
        /// 

		public virtual double getValue()
		{
            //System.Type cls = typeof(System.String);
            ////UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //System.String name = cls.FullName;
            //Configuration.logger_.severe("Class " + name + " does not implement " + "method getValue");
            //throw new SMException("Exception in " + name + ".getValue()");
            
            string name = this.GetType().FullName;
            Configuration.m_logger.WriteLog("Class " + name + " does not implement " + "method getValue");
            throw new NotImplementedException();
		}
		
		/// <summary> Sets a double value to a variable in subclasses of <code>Variable</code>. 
		/// As not all objects belonging to asubclass of <code>Variable</code> have a 
		/// double value, a call to this method it is considered a fatal error by 
		/// default, and the program is terminated. Those classes requiring this method 
		/// must redefine it.
		/// </summary>
		public virtual void  setValue(double val)
		{
            //System.Type cls = typeof(System.String);
            ////UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //System.String name = cls.FullName;
            //Configuration.logger_.severe("Class " + name + " does not implement " + "method setValue");
            //throw new SMException("Exception in " + name + ".setValue()");

            string name = this.GetType().FullName;
            Configuration.m_logger.WriteLog("Class " + name + " does not implement " + "method getValue");
            throw new NotImplementedException();

		} 
		
		/// <summary> Gets the lower bound value of a variable. As not all
		/// objects belonging to a subclass of <code>Variable</code> have a lower bound,
		/// a call to this method is considered a fatal error by default,
		/// and the program is terminated.
		/// Those classes requiring this method must redefine it.
		/// </summary>
		public virtual double getLowerBound()
		{
            //System.Type cls = typeof(System.String);
            ////UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //System.String name = cls.FullName;
            //Configuration.logger_.severe("Class " + name + " does not implement method getLowerBound()");
            //throw new SMException("Exception in " + name + ".getLowerBound()");

            string name = this.GetType().FullName;
            Configuration.m_logger.WriteLog("Class " + name + " does not implement " + "method getValue");
            throw new NotImplementedException();

		} 
		
		/// <summary> Gets the upper bound value of a variable. As not all
		/// objects belonging to a subclass of <code>Variable</code> have an upper 
		/// bound, a call to this method is considered a fatal error by default, and the 
		/// program is terminated. Those classes requiring this method mustredefine it.
		/// </summary>
		public virtual double getUpperBound()
		{
            //System.Type cls = typeof(System.String);
            ////UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //System.String name = cls.FullName;
            //Configuration.logger_.severe("Class " + name + " does not implement method getUpperBound()");
            //throw new SMException("Exception in " + name + ".getUpperBound()");

            string name = this.GetType().FullName;
            Configuration.m_logger.WriteLog("Class " + name + " does not implement " + "method getValue");
            throw new NotImplementedException();

		} 
		
		/// <summary> Sets the lower bound for a variable. As not all objects beloging to a
		/// subclass of <code>Variable</code> have a lower bound, a call to this method 
		/// is considered a fatal error by defaultm and the program is terminated.
		/// Those classes requiring this method must to redefine it.
		/// </summary>
		public virtual void  setLowerBound(double lowerBound)
		{
            //System.Type cls = typeof(System.String);
            ////UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //System.String name = cls.FullName;
            //Configuration.logger_.severe("Class " + name + " does not implement method setLowerBound()");
            //throw new SMException("Exception in " + name + ".setLowerBound()");

            string name = this.GetType().FullName;
            Configuration.m_logger.WriteLog("Class " + name + " does not implement " + "method getValue");
            throw new NotImplementedException();

		} 
		
		/// <summary> Sets the upper bound for a variable. As not all objects belongig to a 
		/// subclass of <code>Variable</code> have an upper bound, a call to this method
		/// is considered a fatal error by default, and the program is terminated. 
		/// Those classes requiring this method must redefine it.
		/// </summary>
		public virtual void  setUpperBound(double upperBound)
		{
            //System.Type cls = typeof(System.String);
            ////UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            //System.String name = cls.FullName;
            //Configuration.logger_.severe("Class " + name + " does not implement method setUpperBound()");
            //throw new SMException("Exception in " + name + ".setUpperBound()");

            string name = this.GetType().FullName;
            Configuration.m_logger.WriteLog("Class " + name + " does not implement " + "method getValue");
            throw new NotImplementedException();

		} 
	} 
}