using System;
using System.Collections.Generic;
using System.Text;

namespace Zhy.Components.Tree
{
    internal class InternalUtil
    {
        internal static TTreeNode GetNode<TTreeNode>(List<TTreeNode> traverseds, List<TTreeNode> nodes)
        {
            int idx = 0;
            while (nodes?.Count > idx)
            {
                TTreeNode treeNode = nodes[idx];
                bool contains = traverseds.Contains(treeNode);
                if (!contains)
                {
                    return treeNode;
                }
                idx++;
            }
            return default;
        }
    }
}
