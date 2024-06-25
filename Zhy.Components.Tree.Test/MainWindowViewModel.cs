using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Zhy.Components.Tree.Extension;

namespace Zhy.Components.Tree.Test
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<TestTreeNode> _treeNodes;
        [ObservableProperty]
        private string _searchText;

        public MainWindowViewModel()
        {
            _treeNodes = new ObservableCollection<TestTreeNode>
            {
                new TestTreeNode
                {
                    Name = "资源目录",
                    Children = new ObservableCollection<TestTreeNode>
                    {
                        new TestTreeNode
                        {
                            Name = "矢量",
                            Children = new ObservableCollection<TestTreeNode>
                            {
                                new TestTreeNode
                                {
                                    Name = "行政区划",
                                    Children = new ObservableCollection<TestTreeNode>
                                    {
                                        new TestTreeNode
                                        {
                                            Name = "北京行政区划"
                                        },
                                        new TestTreeNode
                                        {
                                            Name = "天津行政区划"
                                        },
                                        new TestTreeNode
                                        {
                                            Name = "河北行政区划"
                                        },
                                    }
                                },
                                new TestTreeNode
                                {
                                    Name = "管线",
                                }
                            }
                        },
                        new TestTreeNode
                        {
                            Name = "栅格",
                            Children = new ObservableCollection<TestTreeNode>
                            {
                                new TestTreeNode
                                {
                                    Name = "正射影像",
                                    Children = new ObservableCollection<TestTreeNode>
                                    {
                                        new TestTreeNode
                                        {
                                            Name = "北京遥感影像"
                                        },
                                        new TestTreeNode
                                        {
                                            Name = "天津遥感影像"
                                        },
                                        new TestTreeNode
                                        {
                                            Name = "河北遥感影像"
                                        },
                                    }
                                },
                                new TestTreeNode
                                {
                                    Name = "DEM"
                                }
                            }
                        }
                    }
                },
            };
        }

        [RelayCommand]
        private void Search()
        {
            foreach (var item in TreeNodes)
            {
                item.Filter(n => n.Name.Contains(SearchText));
            }
        }
    }
}
