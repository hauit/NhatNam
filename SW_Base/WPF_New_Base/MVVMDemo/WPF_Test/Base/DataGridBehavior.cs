using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using WPF_Test.Models.Entity;

namespace WPF_Test.Base
{
    public class DataGridBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.RowEditEnding += OnRowEditEnding;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RowEditEnding -= OnRowEditEnding;
        }

        private void OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedItem = e.Row.DataContext as Products;
                var dataGrid = sender as DataGrid;
                var c = e.Row;
                var a = e.Row.Item as Products;
                var b = AssociatedObject.Columns;
                //var cellContent = e.Column.GetCellContent(e.Row);
                //var editedValue = (cellContent as TextBlock)?.Text;
                if (dataGrid != null)
                {
                    // Force the DataGrid to commit the edit
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                }
            }
        }
    }
}
