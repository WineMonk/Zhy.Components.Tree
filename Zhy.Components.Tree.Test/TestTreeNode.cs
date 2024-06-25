using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Zhy.Components.Tree.Test
{
    public partial class TestTreeNode : ObservableObject, IObservableTree<TestTreeNode>
    {
        public TestTreeNode Parent { get; set; }

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private ObservableCollection<TestTreeNode> _children;
        //public ObservableCollection<TestTreeNode> Children 
        //{
        //    get => _children;
        //    set => SetProperty(ref _children, value);
        //}

        public TestTreeNode Clone()
        {
            TestTreeNode clone = new TestTreeNode
            {
                Name = _name,
            };
            if (Children?.Count > 0)
            {
                clone.Children = new ObservableCollection<TestTreeNode>();
                foreach (var child in Children)
                {
                    TestTreeNode subClone = child.Clone();
                    subClone.Parent = this;
                    clone.Children.Add(subClone);
                }
            }
            return clone;
        }
    }
}
