using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ItsaDrag
{
    public partial class MainWindow : Window
    {
        private TreeViewItem sourceItem, targetItem;

        public MainWindow() => InitializeComponent();

        private void TreeViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            if (!(sender is TreeViewItem rootTreeViewItem))
            {
                return;
            }
            var treeView = FindParent<TreeView>(rootTreeViewItem);
            sourceItem = treeView.SelectedItem as TreeViewItem;
            var dropEffect = DragDrop.DoDragDrop(treeView, sourceItem, DragDropEffects.Move);
            if (dropEffect != DragDropEffects.Move || targetItem == null || sourceItem == targetItem)
            {
                return;
            }
            // Do something with drag result but app crashes before getting here.
        }

        private void TreeViewItem_DragOver(object sender, DragEventArgs e)
        {
            if (FindParent<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem overItem)
            {
                e.Handled = true;
                e.Effects = IsValidDropTarget(sourceItem, overItem) ? DragDropEffects.Move : DragDropEffects.None;
            }
        }

        private void TreeViewItem_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            if (sourceItem is null)
            {
                return;
            }
            if (FindParent<TreeViewItem>(sender as DependencyObject) is TreeViewItem dropTarget)
            {
                targetItem = dropTarget;
                e.Effects = DragDropEffects.Move;
            }
        }

        private static T FindParent<T>(DependencyObject child) where T: DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent is T desiredParent)
            {
                return desiredParent;
            }
            else if (parent is null)
            {
                return null;
            }
            return FindParent<T>(parent);
        }

        private static bool IsValidDropTarget(TreeViewItem draggedItem, TreeViewItem potentialDropItem)
        {
            return draggedItem != potentialDropItem;
        }
    }
}
