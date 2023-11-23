using System;
using System.Collections.Generic;
using System.Text;

using SolutionSet = JARE.Base.SolutionSet;
using OverallConstraintViolationComparator = JARE.Base.operators.comparator.OverallConstraintViolationComparator;
using DominanceComparator = JARE.Base.operators.comparator.DominanceComparator;

namespace JARE.util
{
    public class Ranking
    {

/**
 * This class implements some facilities for ranking solutions.
 * Given a <code>SolutionSet</code> object, their solutions are ranked 
 * according to scheme proposed in NSGA-II; as a result, a set of subsets 
 * are obtained. The subsets are numbered starting from 0 (in NSGA-II, the 
 * numbering starts from 1); thus, subset 0 contains the non-dominated 
 * solutions, subset 1 contains the non-dominated solutions after removing those
 * belonging to subset 0, and so on.
 */

  
  /**
   * The <code>SolutionSet</code> to rank
   */
  private SolutionSet m_solutionSet ;

  /**
   * An array containing all the fronts found during the search
   */
  private SolutionSet[] m_ranking  ;
  
  /**
   * stores a <code>Comparator</code> for dominance checking
   */
  private static readonly DominanceComparator m_dominance = new DominanceComparator();
  /**
   * stores a <code>Comparator</code> for Overal Constraint Violation Comparator
   * checking
   */
  private static readonly OverallConstraintViolationComparator m_constraint = new OverallConstraintViolationComparator();
    
  /** 
   * Constructor.
   * @param solutionSet The <code>SolutionSet</code> to be ranked.
   */
  public Ranking(SolutionSet solutionSet)
  {
      m_solutionSet = solutionSet;

      // dominateMe[i] contains the number of solutions dominating i        
      int[] dominateMe = new int[m_solutionSet.size()];

      // iDominate[k] contains the list of solutions dominated by k
      List<int>[] iDominate = new List<int>[m_solutionSet.size()];

      // front[i] contains the list of individuals belonging to the front i
    List<int>[] front = new List<int>[m_solutionSet.size() + 1];

      // flagDominate is an auxiliar variable
      int flagDominate;

      // Initialize the fronts 
      for (int i = 0; i < front.Length; i++)
          front[i] = new List<int>();

      //-> Fast non dominated sorting algorithm
      for (int p = 0; p < m_solutionSet.size(); p++)
      {
          // Initialice the list of individuals that i dominate and the number
          // of individuals that dominate me
          iDominate[p] = new List<int>();
          dominateMe[p] = 0;
          // For all q individuals , calculate if p dominates q or vice versa
          for (int q = 0; q < m_solutionSet.size(); q++)
          {
              //VISNJA : valjda treba porediti resenje p sa svim resenjima q??
             // flagDominate = m_constraint.Compare(solutionSet.getSolution(p), solutionSet.getSolution(p));
              flagDominate = m_constraint.Compare(solutionSet.getSolution(p), solutionSet.getSolution(q));

              if (flagDominate == 0)
              {
                  flagDominate = m_dominance.Compare(solutionSet.getSolution(p), solutionSet.getSolution(q));
              }

              if (flagDominate == -1)
              {
                  iDominate[p].Add(q);
              }
              else if (flagDominate == 1)
              {
                  dominateMe[p]++;
              }
          }

          // If nobody dominates p, p belongs to the first front
          if (dominateMe[p] == 0)
          {
              front[0].Add(p);
              solutionSet.getSolution(p).Rank = 0;
          }
      }

      //Obtain the rest of fronts
      int k = 0;
      //Iterator<Integer> it1, it2 ; // Iterators
      while (front[k].Count != 0)
      {
          k++;
          //it1 = front[i-1].iterator();
          foreach (int itemFront in front[k - 1])
          {
              foreach (int itemIDominate in iDominate[itemFront])
              {
                  int index = itemIDominate;
                  dominateMe[index]--;
                  if (dominateMe[index] == 0)
                  {
                      front[k].Add(index);
                      m_solutionSet.getSolution(index).Rank = k;
                  }
              }
          }
      }

          //<-

          m_ranking = new SolutionSet[k];
          //0,1,2,....,i-1 are front, then i fronts
          for (int j = 0; j < k; j++)
          {
              m_ranking[j] = new SolutionSet(front[j].Count);
              foreach (int item in front[j])
              {
                  m_ranking[j].add(solutionSet.getSolution(item));
              }
              //it1 = front[j].iterator();
              //while (it1.hasNext())
              //{
              //    m_ranking[j].add(solutionSet.get(it1.next().intValue()));
              //}
          }

      
  } // Ranking

  /**
   * Returns a <code>SolutionSet</code> containing the solutions of a given rank. 
   * @param rank The rank
   * @return Object representing the <code>SolutionSet</code>.
   */
  public SolutionSet getSubfront(int rank) {
    return m_ranking[rank];
  } // getSubFront

  /** 
  * Returns the total number of subFronts founds.
  */
  public int getNumberOfSubfronts() {
    return m_ranking.Length;
  } 
} 

    }

