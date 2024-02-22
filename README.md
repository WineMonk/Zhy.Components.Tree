# Zhy.Components.Configuration

## Doc

[主 页](https://shaoshao.net.cn)

[GitHub](https://github.com/WineMonk/Zhy.Components.Tree.git)

[API帮助文档](https://github.com/WineMonk/Zhy.Components.Tree/tree/master/Doc/Help/CHM)

可查询树结构

## Demo

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

TestTree.cs

```csharp
public class TestTree
{
    public TestTree() { }

    public Tree GetTree() 
    {
        Tree treeRoot = new Tree
        {
            Name = "0"
        };
        Tree tree01 = new Tree
        {
            Name = "0-1",
            Parent = treeRoot
        };
        tree01.Children = new ObservableCollection<Tree>
        {
            new Tree
            {
                Name = "0-1-1",
                Parent = tree01
            },
            new Tree
            {
                Name = "0-1-2",
                Parent = tree01
            },
            new Tree
            {
                Name = "0-1-3",
                Parent = tree01
            },
        };
        Tree tree02 = new Tree
        {
            Name = "0-2",
            Parent = treeRoot
        };
        tree02.Children = new ObservableCollection<Tree>
        {
            new Tree
            {
                Name = "0-2-1",
                Parent = tree02
            },
            new Tree
            {
                Name = "0-2-2",
                Parent = tree02
            },
            new Tree
            {
                Name = "0-2-3",
                Parent = tree02
            },
        };
        Tree tree03 = new Tree
        {
            Name = "0-3",
            Parent = treeRoot
        };
        tree03.Children = new ObservableCollection<Tree>
        {
            new Tree
            {
                Name = "0-3-1",
                Parent = tree03
            },
            new Tree
            {
                Name = "0-3-2",
                Parent = tree03
            },
            new Tree
            {
                Name = "0-3-3",
                Parent = tree03
            },
        };
        treeRoot.Children = new ObservableCollection<Tree> { tree01, tree02, tree03 };
        return treeRoot;
    }

    public void TestSearch()
    {
        Tree treeRoot = GetTree();
        Tree? tree = treeRoot.Search(n => n?.Name.Contains("-1") == true);
    }

    public void TestSafeTraversal()
    {
        Tree treeRoot = GetTree();
        treeRoot.SafeTraversal(t =>
                               {
                                   if (t?.Name.Contains("-2") == true)
                                   {
                                       t?.Parent?.Children?.Remove(t);
                                   }
                               });
    }

    public string TestToJson()
    {
        Tree treeRoot = GetTree();
        string jsonText = JsonSerializer.Serialize(treeRoot);
        return jsonText;
    }
    public Tree? TestFromJson(string jsonText)
    {
        Tree? treeRoot = TreeUtil.JsonToObservableTree<Tree>(jsonText);
        return treeRoot;
    }
```

