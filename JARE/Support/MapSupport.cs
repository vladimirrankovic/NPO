using System;
using System.Collections.Generic;
using System.Text;

namespace JARE.support
{
    /*******************************/
    /// <summary>
    /// Provides functionality not found in .NET map-related interfaces.
    /// </summary>
    public class MapSupport
    {
        /// <summary>
        /// Determines whether the SortedList contains a specific value.
        /// </summary>
        /// <param name="d">The dictionary to check for the value.</param>
        /// <param name="obj">The object to locate in the SortedList.</param>
        /// <returns>Returns true if the value is contained in the SortedList, false otherwise.</returns>
        public static bool ContainsValue(System.Collections.IDictionary d, System.Object obj)
        {
            bool contained = false;
            System.Type type = d.GetType();

            //Classes that implement the SortedList class
            if (type == System.Type.GetType("System.Collections.SortedList"))
            {
                contained = (bool)((System.Collections.SortedList)d).ContainsValue(obj);
            }
            //Classes that implement the Hashtable class
            else if (type == System.Type.GetType("System.Collections.Hashtable"))
            {
                contained = (bool)((System.Collections.Hashtable)d).ContainsValue(obj);
            }
            else
            {
                //Reflection. Invoke "containsValue" method for proprietary classes
                try
                {
                    System.Reflection.MethodInfo method = type.GetMethod("containsValue");
                    contained = (bool)method.Invoke(d, new Object[] { obj });
                }
                catch (System.Reflection.TargetInvocationException e)
                {
                    throw e;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
            }

            return contained;
        }


        /// <summary>
        /// Determines whether the NameValueCollection contains a specific value.
        /// </summary>
        /// <param name="d">The dictionary to check for the value.</param>
        /// <param name="obj">The object to locate in the SortedList.</param>
        /// <returns>Returns true if the value is contained in the NameValueCollection, false otherwise.</returns>
        /// 

       // visnja: stavljeno pod komentar jer ga niko ne poziva
 
        //public static bool ContainsValue(System.Collections.Generic.IDictionary<string,string> d, System.Object obj)
        //{
        //    bool contained = false;
        //    System.Type type = d.GetType();

        //    for (int i = 0; i < d.Count && !contained; i++)
        //    {
        //       string[] values = d.GetValues(i);
        //        if (values != null)
        //        {
        //            foreach (System.String val in values)
        //            {
        //                if (val.Equals(obj))
        //                {
        //                    contained = true;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return contained;
        //}

        /// <summary>
        /// Copies all the elements of d to target.
        /// </summary>
        /// <param name="target">Collection where d elements will be copied.</param>
        /// <param name="d">Elements to copy to the target collection.</param>
        public static void PutAll(System.Collections.IDictionary target, System.Collections.IDictionary d)
        {
            if (d != null)
            {
                System.Collections.ArrayList keys = new System.Collections.ArrayList(d.Keys);
                System.Collections.ArrayList values = new System.Collections.ArrayList(d.Values);

                for (int i = 0; i < keys.Count; i++)
                    target[keys[i]] = values[i];
            }
        }

        /// <summary>
        /// Returns a portion of the list whose keys are less than the limit object parameter.
        /// </summary>
        /// <param name="l">The list where the portion will be extracted.</param>
        /// <param name="limit">The end element of the portion to extract.</param>
        /// <returns>The portion of the collection whose elements are less than the limit object parameter.</returns>
        public static System.Collections.SortedList HeadMap(System.Collections.SortedList l, System.Object limit)
        {
            System.Collections.Comparer comparer = System.Collections.Comparer.Default;
            System.Collections.SortedList newList = new System.Collections.SortedList();

            for (int i = 0; i < l.Count; i++)
            {
                if (comparer.Compare(l.GetKey(i), limit) >= 0)
                    break;

                newList.Add(l.GetKey(i), l[l.GetKey(i)]);
            }

            return newList;
        }

        /// <summary>
        /// Returns a portion of the list whose keys are greater that the lowerLimit parameter less than the upperLimit parameter.
        /// </summary>
        /// <param name="l">The list where the portion will be extracted.</param>
        /// <param name="limit">The start element of the portion to extract.</param>
        /// <param name="limit">The end element of the portion to extract.</param>
        /// <returns>The portion of the collection.</returns>
        public static System.Collections.SortedList SubMap(System.Collections.SortedList list, System.Object lowerLimit, System.Object upperLimit)
        {
            System.Collections.Comparer comparer = System.Collections.Comparer.Default;
            System.Collections.SortedList newList = new System.Collections.SortedList();

            if (list != null)
            {
                if ((list.Count > 0) && (!(lowerLimit.Equals(upperLimit))))
                {
                    int index = 0;
                    while (comparer.Compare(list.GetKey(index), lowerLimit) < 0)
                        index++;

                    for (; index < list.Count; index++)
                    {
                        if (comparer.Compare(list.GetKey(index), upperLimit) >= 0)
                            break;

                        newList.Add(list.GetKey(index), list[list.GetKey(index)]);
                    }
                }
            }

            return newList;
        }

        /// <summary>
        /// Returns a portion of the list whose keys are greater than the limit object parameter.
        /// </summary>
        /// <param name="l">The list where the portion will be extracted.</param>
        /// <param name="limit">The start element of the portion to extract.</param>
        /// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
        public static System.Collections.SortedList TailMap(System.Collections.SortedList list, System.Object limit)
        {
            System.Collections.Comparer comparer = System.Collections.Comparer.Default;
            System.Collections.SortedList newList = new System.Collections.SortedList();

            if (list != null)
            {
                if (list.Count > 0)
                {
                    int index = 0;
                    while (comparer.Compare(list.GetKey(index), limit) < 0)
                        index++;

                    for (; index < list.Count; index++)
                        newList.Add(list.GetKey(index), list[list.GetKey(index)]);
                }
            }

            return newList;
        }
    }
}
