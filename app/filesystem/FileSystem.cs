using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class FileSystem : Panel
{
    // Todo: make a backend representation of the
    // directory that carries meta data.
    // Build the UI off the backend data.
    // Model -> View pattern.
    public Directory WorkingDirectory = new Directory();
    public Tree? FileSystemList;
    protected PopupMenu? ContextMenu;
    public string Path = "user://";
    public override void _Ready()
    {
        FileSystemList = GetNode<Tree>("FileSystemScroll/FileSystemList");
        ContextMenu = GetNode<PopupMenu>("ContextMenu");
        UpdateTree();
    }

    public void UpdateTree()
    {
        FileSystemList!.Clear();
        TreeItem treeRoot;
        treeRoot = FileSystemList.CreateItem();
        treeRoot.SetText(0, Path);
        if (WorkingDirectory.Open(Path) == Error.Ok)
        {
            CrawlDirectory(WorkingDirectory, treeRoot);
        }
        else
        {
            GD.PrintErr($"FileSystemPanel.UpdateTree: Error opening directory \"{Path}\"");
        }
        return;

        void CrawlDirectory(Directory dir, TreeItem root)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (!fileName.Equals(""))
            {
                if (fileName.Equals(".") || fileName.Equals(".."))
                {

                }
                else if (dir.CurrentIsDir())
                {
                    TreeItem itemBuffer = FileSystemList.CreateItem(root);
                    itemBuffer.SetText(0, $"{fileName}/");
                    Directory dirNext = new Directory();
                    dirNext.Open(dir.GetCurrentDir() + "/" + fileName);
                    CrawlDirectory(dirNext, itemBuffer);
                }
                else
                {
                    TreeItem itemBuffer = FileSystemList.CreateItem(root);
                    itemBuffer.SetText(0, $"{fileName}");
                }
                fileName = dir.GetNext();
            }
            return;
        }
    }

    public string GetSelectedPath()
    {
        string selectedPath = "";
        TreeItem selection = FileSystemList!.GetSelected();
        List<TreeItem> parents = new List<TreeItem>();
        if (selection != null)
        {
            TreeItem active = selection;
            bool isNotNull = true;

            while (isNotNull)
            {
                TreeItem parent = active.GetParent();
                if (parent == null)
                {
                    isNotNull = false;
                }
                else
                {
                    parents.Add(parent);
                    active = parent;
                }
            }
            for (int i = parents.Count - 1; i >= 0; i--)
            {
                selectedPath += parents[i].GetText(0);
            }
            selectedPath += selection.GetText(0);
        }
        return selectedPath;
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
                Vector2 contextSize = ContextMenu!.RectSize;
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
                ContextMenu!.SetGlobalPosition(contextPos);
                ContextMenu!.Show();
                ContextMenu!.GrabFocus();
            }
        }

        return;
    }

    protected void _OnContextMenuPopupHide()
    {

    }
}

internal class FileTree
{
    // This will be the backing data structure for the displayed
    // FileSystem Control. This will produce the items for the display tree
    // to be built from and manage what directories are loaded and actively being
    // viewed by the user.

    /*
        Basic flow?:
            Specify root directory, this is our reference point;
                If root does not exists or ever goes away, we have to abort;
                Don't need to crash the program, but the file system dock will have to display the error;
            While the directory exists:
                The user can view it;
                The user can perform operations:
                    Copy
                    Move
                    Rename
                    Delete
                    New File
                    New Directory
                NOTE: These operations CAN happen outside of this program.
                    Before any operation, we need to refresh our view of the system and make sure the
                    action is valid;
                    Or blindly try it and catch any errors that may occur;
                    Generally, we want the view to accurately reflect the actual file system.
            Operations can happen at any time, but are sparse. Can reasonably assume
                it won't change too much over time;
    */

    string dirRootSys = "";
    string dirRootGodot = "user://";

    internal FileTree()
    {
        Directory rootDir = new Directory();
        if (rootDir.Open(dirRootGodot) == Error.Ok)
        {
            dirRootSys = OS.GetUserDataDir();
        }

    }
}


internal class FileSystemEntry
{
    internal enum FileType { folder, file }
    protected FileType type;
    internal string path = "";
    internal string name = "";
    protected ulong DisplayTreeOID;
    internal DirEntry? parent; // Is null if root.

    // Todo: Finish filling out the relevant info
}

internal class FileEntry : FileSystemEntry
{
    internal FileEntry()
    {
        type = FileType.file;
    }
}

internal class DirEntry : FileSystemEntry
{
    internal DirEntry()
    {
        type = FileType.folder;
    }

    internal DirEntry(string path)
    {
        type = FileType.folder;
        this.path = path;
        this.name = path.Substring(path.FindLast("/", false) + 1);
        // TODO: Finish constructor
    }

    internal DirEntry(string path, DirEntry parent)
    {
        type = FileType.folder;
        // TODO: extract necessary info
    }

    internal List<FileEntry> files = new List<FileEntry>();
    internal List<DirEntry> folders = new List<DirEntry>();
}