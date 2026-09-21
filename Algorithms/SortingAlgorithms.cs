using SortingVisualizer.Models;

namespace SortingVisualizer.Algorithms
{
    public static class SortingAlgorythms
    {
        public static IEnumerable<SortStep> BubbleSort(IList<int> values)
        {
            ///<summary>
            ///Sorts the list inplace and returns an enumerable for each step
            ///the list is not fully sorted untill the enumerable is ran to the end
            /// </summary>
            int lastUnsortedIndex = values.Count -1;
            while (lastUnsortedIndex > 0)
            {
                int lastSwap = 0;
                for (int i = 0; i < lastUnsortedIndex;i++)
                {
                    yield return new SortStep(i, i + 1, StepType.Compare);
                    if (values[i] > values[i + 1])
                    {
                        (values[i], values[i + 1]) = (values[i + 1], values[i]);
                        yield return new SortStep(i, i + 1, StepType.Swap);
                        lastSwap = i;
                    }

                }
                lastUnsortedIndex = lastSwap;
                

            }
            yield return new SortStep(-1, -1, StepType.Sorted);
            yield break;
        }

        public static IEnumerable<SortStep> InsertionSort(IList<int> values)
        {
            for (int i = 1; i < values.Count ; i++)
            {
                
                int j = i -1;
               for (; j >= 0; j--)
                {
                    yield return new SortStep(i, j, StepType.Compare);
                    if (values[i] > values[j])
                    {
                        break;
                    }
                }
               if(j != i -1)
                {
                    int curNum = values[i];
                    for(int x = i; x > j +1; x --)
                    {
                        values[x] = values[x - 1];
                    }
                    values[j + 1] = curNum;
                    yield return new SortStep(-1,j, StepType.Swap);
                }


            }
            yield return new SortStep(-1, -1, StepType.Sorted);
            yield break;
        }

        public static IEnumerable<SortStep> SelectionSort(IList<int> values)
        {

            for (int i = 0; i < values.Count; i++)
            {
                int indexMin = i;
                for(int j = i+1; j < values.Count; j++)
                { 
                    if (values[j] < values[indexMin])
                    {
                        indexMin = j;
                    }
                    yield return new SortStep(indexMin, j, StepType.Compare);
                }
                if (indexMin != i)
                {
                    int temp = values[i];
                    values[i] = values[indexMin];
                    values[indexMin] = temp;
                    yield return new SortStep(i, indexMin, StepType.Swap);
                }

            }
            yield return new SortStep(-1, -1, StepType.Sorted);
            yield break;
        }

        public static IEnumerable<SortStep> MergeSort(IList<int> values)
        {
            Stack<(int start, int end)> recurStack = new Stack<(int start, int end)>();
            recurStack.Push((0, 1));
            while(recurStack.Count > 0)
            {
                // end of list reached
                /* 
                 see if we are at end 
                if end then merge back all lists forced
                else 
                    add new list of len 1 
                    then double peak and merge back as far as it will go
                 */
                (int start, int end) sublist1 = recurStack.Peek();
                bool listEndReached = sublist1.end >= values.Count;
                (int start, int end) sublist2 = (sublist1.end, sublist1.end);
                if (!listEndReached)
                {
                    sublist2 = (sublist1.end, sublist1.end + 1);
                }

                while (recurStack.Count > 0 && (listEndReached || (sublist1.end -sublist1.start == sublist2.end - sublist2.start )))
                {
                    recurStack.Pop();
                    foreach (var step in MergeHelper(values, sublist1, sublist2))
                    {
                        yield return step;
                    }
                    sublist2 = (sublist1.start,sublist2.end);
                    recurStack.TryPeek(out sublist1);

                }
                if (!listEndReached)
                {
                    recurStack.Push(sublist2);
                }
                


            }
            yield return new SortStep(-1, -1, StepType.Sorted);
            yield break;
        }
        /// <summary>
        /// helper function to merge sublits together
        /// </summary>
        /// <param name="values"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static IEnumerable<SortStep> MergeHelper(IList<int> values,(int start, int end) list1, (int start, int end) list2)
        {

            int index1 = list1.start;
            int end1 = list1.end;
            int index2 = list2.start;
            int end2 = list2.end;
            while (index1 < end1 && index2 < end2)
            {
                yield return new SortStep(index1, index2,StepType.Compare);
                if (values[index1] <= values[index2])
                {
                    // no swap necessary
                    index1 += 1;
                }
                else
                {
                    // swap index2 in and shift array 1 left
                    int temp = values[index2];
                    for (int i = index2; i > index1; i--)
                    {
                        values[i] = values[i - 1];
                    }
                    values[index1] = temp;
                    index1 += 1;
                    end1 += 1;
                    index2 += 1;
                    yield return new SortStep(-1, index1, StepType.Swap);
                }
            }
            yield break;

        }
        public static IEnumerable<SortStep> QuickSort(IList<int> values)
        {
            var recurStack = new Stack<(int start, int end)>();
            recurStack.Push((0, values.Count -1));
            while (recurStack.Count > 0)
            {
                (int start,int end) = recurStack.Pop();
                // pick a pivet 
                if (end <= start)
                {
                    continue;
                }
                int pivot = values[end];
                yield return new SortStep(-1, end, StepType.Pivot);
                //sort around pivot
                int splitIndex = start;
                for (int i = start; i < end; i++)
                {
                    yield return new SortStep(i, end, StepType.Compare);
                    if (values[i] < pivot)
                    {
                        (values[i], values[splitIndex]) = (values[splitIndex], values[i]);
                        yield return new SortStep(i, splitIndex, StepType.Swap);
                        splitIndex++;
                    }

                }
                (values[splitIndex], values[end]) = (values[end], values[splitIndex]);
                yield return new SortStep(splitIndex, end, StepType.Swap);
                //recur on left, recur on right
                recurStack.Push((splitIndex + 1, end));
                recurStack.Push((start, splitIndex -1));

            }
            yield return new SortStep(-1, -1, StepType.Sorted);
            yield break;

        }


    }
    
}
