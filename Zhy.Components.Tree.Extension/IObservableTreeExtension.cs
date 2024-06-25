
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Zhy.Components.Tree.Extension
{
    /// <summary>
    /// 可视树扩展
    /// </summary>
    public static class IObservableTreeExtension
    {
        /// <summary>
        /// 可视化过滤（不改变树结构）
        /// </summary>
        /// <typeparam name="TTreeNode">可视树类型</typeparam>
        /// <param name="tree">可视树实例</param>
        /// <param name="expression">过滤方法</param>
        public static void Filter<TTreeNode>(this IObservableTree<TTreeNode> tree, Func<TTreeNode, bool> expression) where TTreeNode : class, IObservableTree<TTreeNode>
        {
            FilterRec(tree, expression);
        }

        private static bool FilterRec<TTreeNode>(IObservableTree<TTreeNode> tree, Func<TTreeNode, bool> expression) where TTreeNode : class, IObservableTree<TTreeNode>
        {
            if (tree == null || tree.Children == null)
            {
                return false;
            }
            List<TTreeNode> items = new List<TTreeNode>();
            if (tree.Children.Count > 0)
            {
                foreach (var child in tree.Children)
                {
                    bool filter = FilterRec(child, expression);
                    if (filter)
                    {
                        items.Add(child);
                    }
                }
            }
            ObservableCollection<TTreeNode> children = tree.Children;
            ICollectionView _collectionView = CollectionViewSource.GetDefaultView(children);
            if (_collectionView == null)
            {
                return false;
            }
            _collectionView.Filter = n =>
            {
                if (n is TTreeNode node)
                {
                    return items.Contains(node) || (expression != null && expression(node));
                }
                return false;
            };
            return !_collectionView.IsEmpty;
        }
    }
}
