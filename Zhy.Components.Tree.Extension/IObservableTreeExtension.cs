
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Zhy.Components.Tree.Extension
{
    /// <summary>
    /// ��������չ
    /// </summary>
    public static class IObservableTreeExtension
    {
        /// <summary>
        /// ���ӻ����ˣ����ı����ṹ��
        /// </summary>
        /// <typeparam name="TTreeNode">����������</typeparam>
        /// <param name="tree">������ʵ��</param>
        /// <param name="expression">���˷���</param>
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
