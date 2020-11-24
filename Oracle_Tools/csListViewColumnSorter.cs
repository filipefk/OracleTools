using System;
using System.Collections;
using System.Windows.Forms;

namespace Oracle_Tools
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class csListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public csListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            try
            {
                int compareResult;
                DateTime _data;
                double _double;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // Compare the two items
                // Se é double, compara como sendo double
                if (double.TryParse(listviewX.SubItems[ColumnToSort].Text, out _double))
                {
                    if (double.TryParse(listviewY.SubItems[ColumnToSort].Text, out _double))
                    {
                        // Ambos são decimais, compara como double
                        compareResult = ObjectCompare.Compare(double.Parse(listviewX.SubItems[ColumnToSort].Text), double.Parse(listviewY.SubItems[ColumnToSort].Text));
                    }
                    else
                    {
                        // O X é double mas o Y não é double
                        compareResult = -1;
                    }
                }
                else if (double.TryParse(listviewY.SubItems[ColumnToSort].Text, out _double))
                {
                    // O X não é double mas o Y é double
                    compareResult = 1;
                }
                // Se é uma data, compara como data
                else if (DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out _data))
                {
                    if (DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out _data))
                    {
                        // Ambos são datas, compara normalmente
                        compareResult = ObjectCompare.Compare(DateTime.Parse(listviewX.SubItems[ColumnToSort].Text), DateTime.Parse(listviewY.SubItems[ColumnToSort].Text));
                    }
                    else
                    {
                        // O X é data mas o Y não é data
                        compareResult = -1;
                    }
                }
                else if (DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out _data))
                {
                    // O X não é data mas o Y é data
                    compareResult = 1;
                }
                // Senão, compara como sendo string
                else
                {
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                }
                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }
}
