/*=============================================================================
 |   Assignment:  CS6326 Assignment 2
 |       Author:  Raunak Sabhani 
 |     Language:  C#
 |    File Name:  ListViewColumnSorter.cs
 |
 +-----------------------------------------------------------------------------
 |
 |       File Purpose: File contains sorter form sorting the list view columns
 *===========================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asg2_rxs160630
{
    public class ListViewColumnSorter : IComparer
    {
        // Specifies the column to be sorted
        private int ColumnSort;
        // Specifies the order in which to sort (i.e. 'Ascending').
        private SortOrder OrderSort;
        // Case insensitive comparer object
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            ColumnSort = 0;
            OrderSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        // Compares the two objects passed using a case insensitive comparison.
        public int Compare(object objectx, object objecty)
        {
            int result;
            ListViewItem listviewX, listviewY;
            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)objectx;
            listviewY = (ListViewItem)objecty;

            result = ObjectCompare.Compare(listviewX.SubItems[ColumnSort].Text, listviewY.SubItems[ColumnSort].Text);
            // Calculate correct return value based on object comparison
            if (OrderSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return result;
            }
            else if (OrderSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-result);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        // Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        public int SortColumn
        {
            set
            {
                ColumnSort = value;
            }
            get
            {
                return ColumnSort;
            }
        }

        // Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        public SortOrder Order
        {
            set
            {
                OrderSort = value;
            }
            get
            {
                return OrderSort;
            }
        }
    }
}
