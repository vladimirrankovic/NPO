/// <summary> PESA2Selection.java</summary>
/// <author>  Juan J. Durillo
/// </author>
/// <version>  1.0
/// </version>
using System;
using JARE.Base;
using JARE.util.archive;
using Configuration = JARE.util.Configuration;
using SMException = JARE.util.SMException;
using PseudoRandom = JARE.util.PseudoRandom;
namespace JARE.Base.operators.selection
{
	
	/// <summary> This class implements a selection operator as the used in PESA-II 
	/// algorithm
	/// </summary>
	[Serializable]
	public class PESA2Selection:Selection
	{
		
		/// <summary> Performs the operation</summary>
		/// <param name="object">Object representing a SolutionSet. This solution set
		/// must be an instancen <code>AdaptiveGridArchive</code>
		/// </param>
		/// <returns> the selected solution
		/// </returns>
		/// <throws>  SMException  </throws>
		public override System.Object execute(System.Object obj)
		{
			try
			{
				AdaptiveGridArchive archive = (AdaptiveGridArchive) obj;
				int selected;
				int hypercube1 = archive.Grid.randomOccupiedHypercube();
				int hypercube2 = archive.Grid.randomOccupiedHypercube();
				
				if (hypercube1 != hypercube2)
				{
					if (archive.Grid.getLocationDensity(hypercube1) < archive.Grid.getLocationDensity(hypercube2))
					{
						
						selected = hypercube1;
					}
					else if (archive.Grid.getLocationDensity(hypercube2) < archive.Grid.getLocationDensity(hypercube1))
					{
						
						selected = hypercube2;
					}
					else
					{
						if (PseudoRandom.randDouble() < 0.5)
						{
							selected = hypercube2;
						}
						else
						{
							selected = hypercube1;
						}
					}
				}
				else
				{
					selected = hypercube1;
				}
				int b = PseudoRandom.randInt(0, archive.size() - 1);
				int cnt = 0;
				while (cnt < archive.size())
				{
					Solution individual = archive.getSolution((b + cnt) % archive.size());
					if (archive.Grid.location(individual) != selected)
					{
						cnt++;
					}
					else
					{
						return individual;
					}
				}
				return archive.getSolution((b + cnt) % archive.size());
			}
			catch (System.InvalidCastException)
			{
				Configuration.m_logger.WriteLog("PESA2Selection.execute: ClassCastException. " + "Found" + obj.GetType() + "Expected: AdaptativeGridArchive");
                //System.Type cls = typeof(System.String);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Class.getName' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                string name = this.GetType().FullName; 
                throw new SMException("Exception in " + name + ".execute()");
			}
		} //execute
	} // PESA2Selection
}