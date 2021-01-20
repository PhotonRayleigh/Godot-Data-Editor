using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FileSystem : Panel
{
    /*
        Custom FileSystem Control for Godot.
        Used for browsing and operating on a local working directory in the OS filesystem.

        ToDo:
            User selectable root folder
            Implement file operations
            Automatic refresh on filesystem changes (only when main window is back in focus)
    */
    protected bool userEditable = false;
    public FSViewTree? userWorkingTree;
    public Tree? FileSystemListNode;
    protected PopupMenu? ContextMenuNode;
    protected LineEdit? FilePathNode;
    public string path = "user://";
    protected System.Collections.Generic.Dictionary<TreeItem, FSViewTree.Node> FSAssocList
        = new System.Collections.Generic.Dictionary<TreeItem, FSViewTree.Node>();
    public override void _Ready()
    {
        FileSystemListNode = GetNode<Tree>("FileSystemScroll/FileSystemList");
        ContextMenuNode = GetNode<PopupMenu>("ContextMenu");
        FilePathNode = GetNode<LineEdit>("HBoxContainer/FilePath");
        userWorkingTree = new FSViewTree(path);
        FilePathNode.Text = userWorkingTree.userRootDir!.path;
        // This is a best example of how these classes should be used
        // Just run blocking operations in their own threads. 
        Task.Run(() =>
        {
            userWorkingTree.RefreshDirectories();
            UpdateTree();
        });
    }

    bool isUpdating = false;
    public void UpdateTree()
    {
        if (isUpdating) return;

        userEditable = false;
        isUpdating = true;

        FileSystemListNode!.Clear();
        FSAssocList.Clear();
        TreeItem treeRoot;
        treeRoot = FileSystemListNode.CreateItem();
        treeRoot.SetText(0, userWorkingTree!.userRootDir!.name + "/");
        FSAssocList.Add(treeRoot, userWorkingTree.userRootDir);

        TreeItem workingTreeItem = treeRoot;
        FSViewTree.DirNode workingDirNode = userWorkingTree.userRootDir;
        Stack<int[]> counters = new Stack<int[]>();
        Stack<TreeItem> treeItemStack = new Stack<TreeItem>();
        int[] currentCounter = { 0, workingDirNode.folders.Count - 1, 0 }; // [0] = current index, [1] = final index, [2] = files processed
        bool scanning = true;

        while (scanning)
        {
            if (currentCounter[0] <= currentCounter[1])
            {
                FSViewTree.DirNode currentFolder = workingDirNode.folders[currentCounter[0]];
                TreeItem treeBuffer = FileSystemListNode.CreateItem(workingTreeItem);
                treeBuffer.SetText(0, currentFolder.name + "/");
                FSAssocList.Add(treeBuffer, currentFolder);
                if (currentFolder.isOpen) treeBuffer.Collapsed = false;
                else treeBuffer.Collapsed = true;

                if (currentFolder.folders.Count > 0)
                {
                    counters.Push(currentCounter);
                    treeItemStack.Push(workingTreeItem);
                    workingTreeItem = treeBuffer;
                    workingDirNode = currentFolder;
                    currentCounter = new int[] { 0, workingDirNode.folders.Count - 1 };
                    continue;
                }
            }

            if (currentCounter[0] >= currentCounter[1])
            {
                foreach (FSViewTree.FileNode file in workingDirNode.files)
                {
                    TreeItem treeBuffer = FileSystemListNode.CreateItem(workingTreeItem);
                    treeBuffer.SetText(0, file.name);
                    FSAssocList.Add(treeBuffer, file);
                }
            }

            if ((currentCounter[0] >= currentCounter[1]) && workingDirNode.parent == null)
            {
                scanning = false;
            }
            else if ((currentCounter[0] >= currentCounter[1]) && workingDirNode.parent != null)
            {
                currentCounter = counters.Pop();
                workingDirNode = workingDirNode.parent;
                workingTreeItem = treeItemStack.Pop();
            }
            currentCounter[0]++;
        }
        userEditable = true;
        isUpdating = false;

        return;
    }

    public string? GetSelectedPath()
    {
        TreeItem selection = FileSystemListNode!.GetSelected();
        return FSAssocList[selection].path;
    }

    protected void _OnFileSystemListGuiInput(InputEvent e)
    {
        if (e is InputEventMouseButton mouseEvent /*&& mouseEvent.Pressed*/)
        {
            /*
            switch ((ButtonList)mouseEvent.ButtonIndex)
            {
                case ButtonList.Right:
                    GD.Print("Right mouse button was pressed.");
                    break;
            }
            */
            if (Input.IsActionJustReleased("mouse_right_click"))
            {
                //GD.Print("Right mouse button was released.");
                Vector2 view = GetViewport().GetVisibleRect().Size;
                Vector2 mousePos = mouseEvent.GlobalPosition;
                Vector2 contextSize = ContextMenuNode!.RectSize;
                Vector2 contextPos = new Vector2(mousePos);
                if (mousePos.x + contextSize.x > view.x)
                {
                    contextPos.x = view.x - contextSize.x;
                }
                else if (mousePos.x < 0)
                {
                    contextPos.x = 0;
                }
                if (mousePos.y + contextSize.y > view.y)
                {
                    contextPos.y = view.y - contextSize.y;
                }
                else if (mousePos.y < 0)
                {
                    mousePos.y = 0;
                }
                ContextMenuNode!.SetGlobalPosition(contextPos);
                ContextMenuNode!.Show();
                ContextMenuNode!.GrabFocus();
            }
        }

        return;
    }

    protected void _OnContextMenuPopupHide()
    {

    }

    bool FSCollapseRunning = false;
    protected void _OnFileSystemListItemCollapsed(TreeItem item)
    {
        if (!userEditable) return;
        if (FSCollapseRunning) return;
        FSCollapseRunning = true;
        Task.Run(() =>
        {
            //GD.Print($"TreeItem, {item.GetText(0)}, collapsed!");
            FSViewTree.DirNode? fsNode = FSAssocList[item] as FSViewTree.DirNode;
            if (fsNode!.parent != null)
            {
                if (item.Collapsed == true)
                {
                    userWorkingTree!.CloseDirectory(fsNode);
                }
                else
                {
                    userWorkingTree!.OpenDirectory(fsNode);
                }
                userWorkingTree.RefreshDirectories();
                UpdateTree();
            }
            FSCollapseRunning = false;
        });
        return;
    }

    protected void _OnRefreshButtonPressed()
    {
        if (isUpdating) return;
        if (userWorkingTree!.IsRefreshing) return;
        Task.Run(() =>
            {
                userWorkingTree!.RefreshDirectories();
                UpdateTree();
            });
    }
}