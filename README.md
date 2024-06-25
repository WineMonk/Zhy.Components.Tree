# Zhy.Components.Tree

## 1 Document

[GitHub](https://github.com/WineMonk/Zhy.Components.Tree.git)

[API Document](https://github.com/WineMonk/Zhy.Components.Tree/tree/master/Doc/Help/CHM)

## 2 Demo - Zhy.Components.Tree

Tree.cs

```csharp
public class Tree : ObservableObject,IObservableTree<Tree>
{
    private string _name;
    public string Name 
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    //序列化时必须忽略
    [JsonIgnore]
    public Tree? Parent { get; set; }
    private ObservableCollection<Tree>? _children;
    public ObservableCollection<Tree>? Children
    {
        get => _children;
        set => SetProperty(ref _children, value);
    }

    public Tree Clone()
    {
        Tree clone = new Tree();
        clone.Name = Name;
        if (Children?.Count > 0)
        {
            clone.Children = new ObservableCollection<Tree>();
            foreach (var child in Children)
            {
                Tree childClone = child.Clone();
                childClone.Parent = clone;
                clone.Children.Add(childClone);
            }
        }
        return clone;
    }
}
```

## 3 Demo - Zhy.Components.Tree.Extension - ObservableTree.Filter

![Zhy.Components.ObservableTree.Filter](https://raw.githubusercontent.com/WineMonk/images/master/blog/post/202406251036706.gif)

TestTreeNode.cs

```csharp
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
```

MainWindow.xaml

```xaml
<Window
    x:Class="Zhy.Components.Tree.Test.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:local="clr-namespace:Zhy.Components.Tree.Test"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    Title="MainWindow"
    Width="800"
    Height="450"
    mc:Ignorable="d">
    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="auto" />
            <RowDefinition />
        </Grid.RowDefinitions>
        <DockPanel>
            <Button
                Command="{Binding SearchCommand}"
                Content="查  询"
                Cursor="Hand"
                DockPanel.Dock="Right" />
            <TextBox Text="{Binding SearchText}" />
        </DockPanel>
        <TreeView
            Grid.Row="1"
            HorizontalContentAlignment="Stretch"
            VerticalContentAlignment="Stretch"
            ItemsSource="{Binding TreeNodes}">
            <TreeView.ItemContainerStyle>
                <Style TargetType="TreeViewItem">
                    <Setter Property="IsExpanded" Value="True" />
                </Style>
            </TreeView.ItemContainerStyle>
            <TreeView.ItemTemplate>
                <HierarchicalDataTemplate ItemsSource="{Binding Children}">
                    <DockPanel x:Name="dp" Margin="0,2,0,2">
                        <TextBlock
                            VerticalAlignment="Center"
                            FontSize="14"
                            IsHitTestVisible="True"
                            Text="{Binding Name}" />
                        <TextBlock IsHitTestVisible="True" />
                    </DockPanel>
                </HierarchicalDataTemplate>
            </TreeView.ItemTemplate>
        </TreeView>
    </Grid>
</Window>
```

MainWindowViewModel.cs

```csharp
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
```

